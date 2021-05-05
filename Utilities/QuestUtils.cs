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

		// https://stackoverflow.com/questions/15967240/fastest-implementation-of-log2int-and-log2float
		public static int Log2(int v)
		{
			int r = 0xFFFF - v >> 31 & 0x10;
			v >>= r;
			int shift = 0xFF - v >> 31 & 0x8;
			v >>= shift;
			r |= shift;
			shift = 0xF - v >> 31 & 0x4;
			v >>= shift;
			r |= shift;
			shift = 0x3 - v >> 31 & 0x2;
			v >>= shift;
			r |= shift;
			r |= (v >> 1);
			return r;
		}

		private static Dictionary<QuestType, Color> ColorByType = new Dictionary<QuestType, Color>()
		{
			{QuestType.Main, new Color(234, 194, 107)},
			{QuestType.Slayer, new Color(196, 66, 77)},
			{QuestType.Forager, new Color(153, 196, 102)},
			{QuestType.Explorer, new Color(186, 141, 117)},
			{QuestType.Designer, new Color(125, 183, 224)},
			{QuestType.Other, new Color(173, 117, 198)}
		};
		private static QuestType[] QuestTypes = (QuestType[])Enum.GetValues(typeof(QuestType));

		public static QuestType GetBaseQuestType(QuestType type)
		{
			for (int i = 0; i < QuestTypes.Length; i++)
			{
				if ((type & QuestTypes[i]) != 0)
				{
					return QuestTypes[i];
				}
			}
			return QuestType.Other;
		}

		public static (Color, string) GetCategoryInfo (QuestType type)
		{
			QuestType baseType = GetBaseQuestType(type);

			return (ColorByType[baseType], baseType.ToString());
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
