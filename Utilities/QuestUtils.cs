using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.QuestSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SpiritMod.UI.Elements;

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
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.X, rect.Y, rect.Width, 1), null, colour);
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.X, rect.Y + 1, 1, rect.Height - 2), null, colour);
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.Right - 1, rect.Y + 1, 1, rect.Height - 2), null, colour);
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.X, rect.Bottom - 1, rect.Width, 1), null, colour);
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
	}
}
