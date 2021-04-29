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

		public int Count
		{
			get
			{
				return this._items.Count;
			}
		}

		public UIModifiedList()
		{
			this._innerList.OverflowHidden = false;
			this._innerList.Width.Set(0f, 1f);
			this._innerList.Height.Set(0f, 1f);
			this.OverflowHidden = true;
			base.Append(this._innerList);
		}

		public virtual void Add(UIElement item)
		{
			this._items.Add(item);
			this._innerList.Append(item);
			this._innerList.Recalculate();
		}

		public virtual void AddRange(IEnumerable<UIElement> items)
		{
			this._items.AddRange(items);
			foreach (UIElement item in items)
			{
				this._innerList.Append(item);
			}
			this._innerList.Recalculate();
		}

		public virtual void Clear()
		{
			this._innerList.RemoveAllChildren();
			this._items.Clear();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._scrollbar != null)
			{
				this._innerList.Top.Set(-this._scrollbar.GetValue(), 0f);
			}
			this.Recalculate();
		}

		public override List<SnapPoint> GetSnapPoints()
		{
			SnapPoint snapPoint;
			List<SnapPoint> snapPoints = new List<SnapPoint>();
			if (base.GetSnapPoint(out snapPoint))
			{
				snapPoints.Add(snapPoint);
			}
			foreach (UIElement _item in this._items)
			{
				snapPoints.AddRange(_item.GetSnapPoints());
			}
			return snapPoints;
		}

		public float GetTotalHeight()
		{
			return this._innerListHeight;
		}

		public void Goto(UIList.ElementSearchMethod searchMethod)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (searchMethod(this._items[i]))
				{
					this._scrollbar.ViewPosition = this._items[i].Top.Pixels;
					return;
				}
			}
		}

		public override void Recalculate()
		{
			base.Recalculate();
			this.UpdateScrollbar();
		}

		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			float height = 0f;
			for (int i = 0; i < this._items.Count; i++)
			{
				this._items[i].Top.Set(height, 0f);
				this._items[i].Recalculate();
				height = height + (this._items[i].GetOuterDimensions().Height + this.ListPadding);
			}
			this._innerListHeight = height;
		}

		public virtual bool Remove(UIElement item)
		{
			this._innerList.RemoveChild(item);
			return this._items.Remove(item);
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			if (this._scrollbar != null)
			{
				UIScrollbar viewPosition = this._scrollbar;
				viewPosition.ViewPosition = viewPosition.ViewPosition - (float)evt.ScrollWheelValue;
			}
		}

		public void SetScrollbar(UIScrollbar scrollbar)
		{
			this._scrollbar = scrollbar;
			this.UpdateScrollbar();
		}

		private void UpdateScrollbar()
		{
			if (this._scrollbar == null)
			{
				return;
			}
			this._scrollbar.SetView(base.GetInnerDimensions().Height, this._innerListHeight);
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
				Vector2 vector2 = this.Parent.GetDimensions().Position();
				Vector2 vector21 = new Vector2(this.Parent.GetDimensions().Width, this.Parent.GetDimensions().Height);
				foreach (UIElement element in this.Elements)
				{
					Vector2 vector22 = element.GetDimensions().Position();
					Vector2 vector23 = new Vector2(element.GetDimensions().Width, element.GetDimensions().Height);
					if (!Collision.CheckAABBvAABBCollision(vector2, vector21, vector22, vector23))
					{
						continue;
					}
					element.Draw(spriteBatch);
				}
			}
		}
	}
}
