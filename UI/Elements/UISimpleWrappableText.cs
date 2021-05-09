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
	// Yeah this started out simple and got a little out of hand.
	// I would fix it and make it more suitable but at this point I'm quite tired of user interface.
	// Bad mentality, maybe. Saving my own sanity, yes. I'm choosing my sanity kthx
	public class UISimpleWrappableText : UIElement
	{
		private TextSnippet[] _array;
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
		private bool centered;
		protected bool _wrappable;
		private Color _colour;
		public bool Centered { get => centered; set { centered = value; UpdateText(); } }
		public Color Colour { get => _colour; set { _colour = value; UpdateText(); } }
		public float Scale { get; set; }
		public int MaxLines { get; set; } = 99;
		public bool Large { get; set; }
		public bool Border { get; set; }
		public int Page { get => page; set { page = value; UpdateText(); } }
		public int MaxPages { get; protected set; }
		public bool Wrappable { get => _wrappable; set { _wrappable = value; UpdateText(); } }
		public Color BorderColour { get; set; }
		public DynamicSpriteFont Font => Large ? Main.fontDeathText : Main.fontMouseText;
		private float _drawOffsetX;
		private int _startLine;
		private int page = 0;

		public UISimpleWrappableText(string text, float textScale = 1f, bool large = false, bool wrappable = false)
		{
			_text = text;
			_wrappable = wrappable;
			_colour = Color.White;
			Scale = textScale;
			Large = large;

			UpdateText();
		}

		public void UpdateText()
		{
			if (string.IsNullOrEmpty(_text))
			{
				_array = null;
				return;
			}

			string[] lines = _text.Split('\n');
			MaxPages = 1 + lines.Length / MaxLines;
			// sort by lines
			if (lines.Length > MaxLines)
			{
				int startLine = MaxLines * Page;
				StringBuilder builder = new StringBuilder();
				for (int i = startLine; i < startLine + MaxLines && i < lines.Length; i++)
				{
					builder.AppendLine(lines[i]);
				}
				_text = builder.ToString();
			}

			_array = ChatManager.ParseMessage(_text, Colour).ToArray();
			ChatManager.ConvertNormalSnippets(_array);

			if (Centered)
			{
				_drawOffsetX = ChatManager.GetStringSize(Font, _array, new Vector2(Scale), Wrappable ? GetDimensions().Width : -1f).X * 0.5f;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_array == null)
			{
				// no text to draw
				return;
			}

			// get positions
			var style = GetDimensions();
			Vector2 tl = style.Position();
			if (Centered) tl = new Vector2(style.Center().X, tl.Y);
			Vector2 pos = tl - Vector2.UnitX * _drawOffsetX;

			float maxWidth = Wrappable ? style.Width : -1f;

			// draw
			if (Border)
			{
				DrawColorCodedStringShadow(spriteBatch, Font, _array, pos, BorderColour, 0f, Vector2.Zero, new Vector2(Scale), maxWidth);
			}
			DrawColorCodedString(spriteBatch, Font, _array, pos, Colour, 0f, Vector2.Zero, new Vector2(Scale), out int h, maxWidth);
		}

		public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
		{
			int num;
			for (int i = 0; i < (int)ChatManager.ShadowDirections.Length; i++)
			{
				DrawColorCodedString(spriteBatch, font, snippets, position + (ChatManager.ShadowDirections[i] * spread), baseColor, rotation, origin, baseScale, out num, maxWidth, true);
			}
		}

		public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth, bool ignoreColors = false)
		{
			Vector2 vector2;
			int num = -1;
			Vector2 vector21 = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			Vector2 x = position;
			Vector2 vector22 = x;
			float single = font.MeasureString(" ").X;
			Color visibleColor = baseColor;
			float single1 = 0f;
			for (int i = 0; i < (int)snippets.Length; i++)
			{
				TextSnippet textSnippet = snippets[i];
				textSnippet.Update();
				if (!ignoreColors)
				{
					visibleColor = textSnippet.GetVisibleColor();
				}
				float scale = textSnippet.Scale;
				if (!textSnippet.UniqueDraw(false, out vector2, spriteBatch, x, visibleColor, scale))
				{
					string[] strArrays = textSnippet.Text.Split(new char[] { '\n' });
					string[] strArrays1 = strArrays;
					for (int j = 0; j < (int)strArrays1.Length; j++)
					{
						string[] strArrays2 = strArrays1[j].Split(new char[] { ' ' });
						for (int k = 0; k < (int)strArrays2.Length; k++)
						{
							if (k != 0)
							{
								ref float singlePointer = ref x.X;
								singlePointer = singlePointer + single * baseScale.X * scale;
							}
							if (maxWidth > 0f && x.X - position.X + font.MeasureString(strArrays2[k]).X * baseScale.X * scale > maxWidth)
							{
								x.X = position.X;
								ref float y = ref x.Y;
								y = y + (float)font.LineSpacing * single1 * baseScale.Y;
								vector22.Y = Math.Max(vector22.Y, x.Y);
								single1 = 0f;
							}
							if (single1 < scale)
							{
								single1 = scale;
							}
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, strArrays2[k], x, visibleColor, rotation, origin, (baseScale * textSnippet.Scale) * scale, SpriteEffects.None, 0f);
							Vector2 vector23 = font.MeasureString(strArrays2[k]);
							if (vector21.Between(x, x + vector23))
							{
								num = i;
							}
							ref float x1 = ref x.X;
							x1 = x1 + vector23.X * baseScale.X * scale;
							vector22.X = Math.Max(vector22.X, x.X);
						}
						if ((int)strArrays.Length > 1 && j < strArrays1.Length - 1)
						{
							ref float lineSpacing = ref x.Y;
							lineSpacing = lineSpacing + (float)font.LineSpacing * single1 * baseScale.Y;
							x.X = position.X;
							vector22.Y = Math.Max(vector22.Y, x.Y);
							single1 = 0f;
						}
					}
				}
				else
				{
					if (vector21.Between(x, x + vector2))
					{
						num = i;
					}
					ref float singlePointer1 = ref x.X;
					singlePointer1 = singlePointer1 + vector2.X * baseScale.X * scale;
					vector22.X = Math.Max(vector22.X, x.X);
				}
			}
			hoveredSnippet = num;
			return vector22;
		}
	}
}
