using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace SpiritMod.UI.Elements
{
	public class UISimpleWrappableText : UIElement
	{
		protected string _text;
		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				UpdateText();
			}
		}
		public Color Colour { get; set; }
		public float Scale { get; set; }
		public bool Large { get; set; }
		public bool Centered { get; set; }
		public bool Border { get; set; }
		public bool UseChatManager { get; set; }
		public Color BorderColour { get; set; }
		public DynamicSpriteFont Font { get => Large ? Main.fontDeathText : Main.fontMouseText; }
		private float _drawOffsetX;
		protected bool _wrappable;

		public UISimpleWrappableText(string text, float textScale = 1f, bool large = false, bool wrappable = false)
		{
			_wrappable = wrappable;
			Colour = Color.White;
			Scale = textScale;
			Large = large;
			Text = text;
		}

		public void UpdateText()
		{
			if (_wrappable)
			{
				if (UseChatManager)
				{
					SpiritMod.Instance.Logger.Debug("Trying to use ChatManager for SimpleWrappableText isn't possible at the moment.");
				}
				else
				{
					WrapText();
				}
			}
			if (Centered)
			{
				if (UseChatManager)
				{
					_drawOffsetX = ChatManager.GetStringSize(Font, _text, new Vector2(Scale)).X;
				}
				else
				{
					_drawOffsetX = Font.MeasureString(_text).X * 0.5f * Scale;
				}
			}
		}

		protected void WrapText()
		{
			float width = GetDimensions().Width;
			SpiritMod.Instance.Logger.Debug(width);
			string textNoLines = Text.Replace('\n', ' ');
			string[] words = textNoLines.Split(' ');
			StringBuilder final = new StringBuilder();
			float currentWidth = 0;
			DynamicSpriteFont font = Large ? Main.fontDeathText : Main.fontMouseText;
			float spaceWidth = font.MeasureString(" ").X * Scale;
			for (int i = 0; i < words.Length; i++)
			{
				float wordWidth = font.MeasureString(words[i]).X * Scale;
				if (currentWidth + spaceWidth + wordWidth >= width)
				{
					// add to next line
					final.Append("\n");
					currentWidth = 0;
				}
				else if (i > 0)
				{
					final.Append(" ");
					currentWidth += spaceWidth;
				}
				final.Append(words[i]);
				currentWidth += wordWidth;
			}
			_text = final.ToString();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 tl = GetDimensions().Position();
			if (Centered) tl = new Vector2(GetDimensions().Center().X, tl.Y);

			Vector2 pos = tl - Vector2.UnitX * _drawOffsetX;
			if (UseChatManager)
			{
				TextSnippet[] array = ChatManager.ParseMessage(Text, Colour).ToArray();
				ChatManager.ConvertNormalSnippets(array);
				if (Border)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Font, array, pos, 0f, Vector2.Zero, new Vector2(Scale), out int hov);
				}
				else
				{
					ChatManager.DrawColorCodedString(spriteBatch, Font, array, pos, Colour, 0f, Vector2.Zero, new Vector2(Scale), out int hov2, -1f);
				}
				return;
			}
			if (Border)
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, pos + Vector2.UnitX, BorderColour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, pos - Vector2.UnitX, BorderColour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, pos + Vector2.UnitY, BorderColour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, pos - Vector2.UnitY, BorderColour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, Font, Text, pos, Colour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
		}
	}
}
