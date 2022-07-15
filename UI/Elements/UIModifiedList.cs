using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria;

namespace SpiritMod.UI.Elements
{
	// modified to remove forced ordering because ffs that shit fucks me up
	public class UIModifiedList : UIElement
	{
		public List<UIElement> _items = new List<UIElement>();

		protected UIScrollbar _scrollbar;

		internal UIElement _innerList = new UIModifiedList.UIInnerList();

		private float _innerListHeight;

		public float ListPadding = 5f;

		public int Count => this._items.Count;

		public UIModifiedList()
		{
			_innerList.OverflowHidden = false;
			_innerList.Width.Set(0f, 1f);
			_innerList.Height.Set(0f, 1f);
			OverflowHidden = true;
			Append(_innerList);
		}

		public virtual void Add(UIElement item)
		{
			_items.Add(item);
			_innerList.Append(item);
			_innerList.Recalculate();
		}

		public virtual void AddRange(IEnumerable<UIElement> items)
		{
			_items.AddRange(items);
			foreach (UIElement item in items)
			{
				_innerList.Append(item);
			}
			_innerList.Recalculate();
		}

		public virtual void Clear()
		{
			_innerList.RemoveAllChildren();
			_items.Clear();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_scrollbar != null)
			{
				_innerList.Top.Set(-_scrollbar.GetValue(), 0f);
			}
			Recalculate();
		}

		public override List<SnapPoint> GetSnapPoints()
		{
			SnapPoint snapPoint;
			List<SnapPoint> snapPoints = new List<SnapPoint>();
			if (GetSnapPoint(out snapPoint))
			{
				snapPoints.Add(snapPoint);
			}
			foreach (UIElement _item in _items)
			{
				snapPoints.AddRange(_item.GetSnapPoints());
			}
			return snapPoints;
		}

		public float GetTotalHeight()
		{
			return _innerListHeight;
		}

		public void Goto(UIList.ElementSearchMethod searchMethod)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				if (searchMethod(_items[i]))
				{
					_scrollbar.ViewPosition = _items[i].Top.Pixels;
					return;
				}
			}
		}

		public override void Recalculate()
		{
			base.Recalculate();
			UpdateScrollbar();
		}

		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			float height = 0f;
			for (int i = 0; i < _items.Count; i++)
			{
				_items[i].Top.Set(height, 0f);
				_items[i].Recalculate();
				height = height + (_items[i].GetOuterDimensions().Height + ListPadding);
			}
			_innerListHeight = height;
		}

		public virtual bool Remove(UIElement item)
		{
			_innerList.RemoveChild(item);
			return _items.Remove(item);
		}

		public override void Update(GameTime gameTime)
		{
			for (int i = 0; i < Elements.Count; i++)
			{
				Elements[i].Update(gameTime);
			}
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			if (_scrollbar != null)
			{
				UIScrollbar viewPosition = _scrollbar;
				viewPosition.ViewPosition = viewPosition.ViewPosition - evt.ScrollWheelValue;
			}
		}

		public void SetScrollbar(UIScrollbar scrollbar)
		{
			_scrollbar = scrollbar;
			UpdateScrollbar();
		}

		private void UpdateScrollbar()
		{
			if (_scrollbar == null)
			{
				return;
			}
			_scrollbar.SetView(GetInnerDimensions().Height, _innerListHeight);
		}

		public delegate bool ElementSearchMethod(UIElement element);

		private class UIInnerList : UIElement
		{
			public UIInnerList()
			{
			}

			public override bool ContainsPoint(Vector2 point)
			{
				return true;
			}

			protected override void DrawChildren(SpriteBatch spriteBatch)
			{
				Vector2 vector2 = Parent.GetDimensions().Position();
				Vector2 vector21 = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
				for (int i = 0; i < Elements.Count; i++)
				{
					UIElement element = Elements[i];
					Vector2 vector22 = element.GetDimensions().Position();
					Vector2 vector23 = new Vector2(element.GetDimensions().Width, element.GetDimensions().Height);
					if (!Collision.CheckAABBvAABBCollision(vector2, vector21, vector22, vector23))
						continue;
					element.Draw(spriteBatch);
				}
			}
		}
	}
}
