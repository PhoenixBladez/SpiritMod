using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace SpiritMod.UI.Elements
{
	public class UIQuestBookScrollBar : UIScrollbar
	{
		private UserInterface _userInterface;
		private FieldInfo _viewPosField;
		private FieldInfo _viewSizeField;
		private FieldInfo _viewMaxSizeField;

		private bool _isDragging;

		private bool _isHoveringOverHandle;

		private float _dragYOffset;

		public Color Colour { get; set; }

		protected float ViewPos { get => (float)_viewPosField.GetValue(this); set => _viewPosField.SetValue(this, value); }
		protected float ViewSize { get => (float)_viewSizeField.GetValue(this); set => _viewSizeField.SetValue(this, value); }
		protected float MaxViewSize { get => (float)_viewMaxSizeField.GetValue(this); set => _viewMaxSizeField.SetValue(this, value); }

		// based on the FixedUIScrollbar and UIScrollbar class, but changing the style. no comments because it's just ripped from terraria source with minor changes.
		public UIQuestBookScrollBar(UserInterface ui)
		{
			_userInterface = ui;
			_viewPosField = typeof(UIScrollbar).GetField("_viewPosition", BindingFlags.NonPublic | BindingFlags.Instance);
			_viewSizeField = typeof(UIScrollbar).GetField("_viewSize", BindingFlags.NonPublic | BindingFlags.Instance);
			_viewMaxSizeField = typeof(UIScrollbar).GetField("_maxViewSize", BindingFlags.NonPublic | BindingFlags.Instance);

			this.Width.Set(8f, 0f);
			this.MaxWidth.Set(8f, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			UserInterface activeInstance = UserInterface.ActiveInstance;
			UserInterface.ActiveInstance = _userInterface;

			CalculatedStyle dimensions = base.GetDimensions();
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			if (this._isDragging)
			{
				float y = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - this._dragYOffset;
				ViewPos = MathHelper.Clamp(y / innerDimensions.Height * MaxViewSize, 0f, MaxViewSize - ViewSize);
			}
			Rectangle handleRectangle = this.GetHandleRectangle();
			Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
			bool flag = this._isHoveringOverHandle;
			this._isHoveringOverHandle = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
			if (!flag && this._isHoveringOverHandle && Main.hasFocus)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
			}
			spriteBatch.Draw(TextureAssets.BlackTile.Value, handleRectangle, Colour * (this._isDragging || this._isHoveringOverHandle ? 1f : 0.85f));

			UserInterface.ActiveInstance = activeInstance;
		}

		private Rectangle GetHandleRectangle()
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			if (MaxViewSize == 0f && ViewSize == 0f)
			{
				ViewSize = 1f;
				MaxViewSize = 1f;
			}
			return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (ViewPos / MaxViewSize)) - 3, 8, (int)(innerDimensions.Height * (ViewSize / MaxViewSize)) + 7);
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			UserInterface activeInstance = UserInterface.ActiveInstance;
			UserInterface.ActiveInstance = _userInterface;

			//base.MouseDown(evt);
			if (evt.Target == this)
			{
				Rectangle handleRectangle = this.GetHandleRectangle();
				if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
				{
					this._isDragging = true;
					this._dragYOffset = evt.MousePosition.Y - (float)handleRectangle.Y;
					return;
				}
				CalculatedStyle innerDimensions = base.GetInnerDimensions();
				float y = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (float)(handleRectangle.Height >> 1);
				ViewPos = MathHelper.Clamp(y / innerDimensions.Height * MaxViewSize, 0f, MaxViewSize - ViewSize);
			}

			UserInterface.ActiveInstance = activeInstance;
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			base.MouseUp(evt);
			this._isDragging = false;
		}
	}
}
