using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.QuestSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SpiritMod.UI.Elements;
using ReLogic.Graphics;
using Terraria.UI.Chat;

namespace SpiritMod.Utilities
{
    public static class QuestUtils
    {
		public enum QuestInvLocation
		{
			Minimap,
			Trashcan,
			FarLeft
		}

        public static void DrawRectangleBorder(SpriteBatch spriteBatch, Rectangle rect, Color colour)
        {
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(rect.X, rect.Y, rect.Width, 1), null, colour);
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(rect.X, rect.Y + 1, 1, rect.Height - 2), null, colour);
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(rect.Right - 1, rect.Y + 1, 1, rect.Height - 2), null, colour);
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(rect.X, rect.Bottom - 1, rect.Width, 1), null, colour);
        }

		public static string GetPluralEnding(int value, string word)
		{
			if (value > 1)
			{
				if (word.Last() != 's') return "s";
				return "\'";
			}
			return "";
		}

		public static bool TryUnbox<T>(object obj, out T value, string what = null)
		{
			if (!(obj is T val))
			{
				value = default(T);

				if (what != null) 
					SpiritMod.Instance.Logger.Warn("Error unboxing object! " + what + " is not a " + typeof(T).Name);
				else
					SpiritMod.Instance.Logger.Warn("Error unboxing object!");

				return false;
			}
			value = val;
			return true;
		}

		public static string WrapText(DynamicSpriteFont font, TextSnippet[] snippets, string fullText, float maxLineWidth, float fontScale = 1f)
		{
			var regex = new System.Text.RegularExpressions.Regex(@"\s+(?![^\[]*\])");

			string baseText = "";
			foreach (TextSnippet snippet in snippets)
			{
				baseText += snippet.Text;
			}

			if (string.IsNullOrEmpty(baseText)) return "";

			string[] lines = baseText.Split('\n');
			string[] actualLines = fullText.Split('\n');
			string newText = "";
			float currentWidth = 0f;
			float spaceWidth = font.MeasureString(" ").X * fontScale;

			for (int i = 0; i < Math.Min(lines.Length, actualLines.Length); i++)
			{
				string line = lines[i];
				string[] words = line.Split(' ');
				string[] actualWords = regex.Split(actualLines[i]);
				for (int k = 0; k < Math.Min(words.Length, actualWords.Length); k++)
				{
					string word = words[k];
					string actualWord = actualWords[k];
					Vector2 wordSize = font.MeasureString(word) * fontScale;

					if (currentWidth + wordSize.X < maxLineWidth)
					{
						newText += actualWord + " ";
						currentWidth += wordSize.X + spaceWidth;
						continue;
					}

					newText += Environment.NewLine + actualWord + " ";
					currentWidth = wordSize.X + spaceWidth;
				}
				currentWidth = 0f;
				if (i < lines.Length - 1)
				{
					newText += "\n";
				}
			}

			return newText;
		}

		public static string WrapText(DynamicSpriteFont font, string text, float maxLineWidth, float fontScale = 1f)
		{
			if (string.IsNullOrEmpty(text)) return "";

			string[] lines = text.Split('\n');
			string newText = "";
			float currentWidth = 0f;
			float spaceWidth = font.MeasureString(" ").X * fontScale;

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				string[] words = line.Split(' ');
				foreach (string word in words)
				{
					Vector2 wordSize = font.MeasureString(word) * fontScale;

					if (currentWidth + wordSize.X < maxLineWidth)
					{
						newText += word + " ";
						currentWidth += wordSize.X + spaceWidth;
						continue;
					}

					newText += Environment.NewLine + word + " ";
					currentWidth = wordSize.X + spaceWidth;
				}
				currentWidth = 0f;
				if (i < lines.Length - 1)
				{
					newText += "\n";
				}
			}

			return newText;
		}
	}
}
