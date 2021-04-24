using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.GameContent.UI.Elements;
using ReLogic.Graphics;
using SpiritMod.UI.QuestUI;

namespace SpiritMod.UI.Elements
{
	public class UIQuestBookButtonTextPanel : UISelectableOutlineRectPanel
	{
		private object _text;

		private bool _isLarge;

		private bool _textAffectsSize;

		public float LeftSideTextPadding;

		public string Text
		{
			get
			{
				if (this._text == null)
				{
					return "";
				}
				return this._text.ToString();
			}
		}

		public Color TextColor { get; set; }

		public float TextScale { get; set; }

		public Vector2 TextSize { get; protected set; }

		public UIQuestBookButtonTextPanel(object text, float textScale = 1f, bool large = false, bool textAffectsSize = false)
		{
			_textAffectsSize = textAffectsSize;

			SelectedOutlineColour = new Color(102, 86, 67);
			HoverOutlineColour = new Color(102, 86, 67) * 0.5f;

			this.SetText(text, textScale, large);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			DynamicSpriteFont font = _isLarge ? Main.fontDeathText : Main.fontMouseText;
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, Text, base.GetDimensions().Center() - TextSize * 0.5f, new Color(43, 28, 17), 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
		}

		public override void Recalculate()
		{
			this.SetText(this._text, this.TextScale, this._isLarge);
			base.Recalculate();
		}

		public void SetText(object text)
		{
			this.SetText(text, this.TextScale, this._isLarge);
		}

		public virtual void SetText(object text, float textScale, bool large)
		{
			Vector2 stringSize = ChatManager.GetStringSize((large ? Main.fontDeathText : Main.fontMouseText), text.ToString(), new Vector2(textScale), -1f);
			stringSize.Y = (large ? 32f : 16f) * textScale;
			this._text = text;
			TextScale = textScale;
			TextSize = stringSize;
			this._isLarge = large;
			if (_textAffectsSize)
			{
				this.MinWidth.Set(stringSize.X + this.PaddingLeft + this.PaddingRight, 0f);
				this.MinHeight.Set(stringSize.Y + this.PaddingTop + this.PaddingBottom, 0f);
			}
		}
	}
}
