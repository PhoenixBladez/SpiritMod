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

namespace SpiritMod.UI.QuestUI
{
    public static class QuestUtils
    {
        public static void DrawRectangleBorder(SpriteBatch spriteBatch, Rectangle rect, Color colour)
        {
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.X, rect.Y, rect.Width, 1), null, colour);
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.X, rect.Y + 1, 1, rect.Height - 2), null, colour);
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.Right - 1, rect.Y + 1, 1, rect.Height - 2), null, colour);
            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(rect.X, rect.Bottom - 1, rect.Width, 1), null, colour);
        }

        public static UISelectableQuest QuestAsPanel(Quest quest)
        {
			UISelectableQuest panel = new UISelectableQuest(quest);
            panel.SelectedOutlineColour = new Color(102, 86, 67);
            panel.HoverOutlineColour = new Color(102, 86, 67) * 0.5f;
            panel.Height.Set(22f, 0f);
            panel.Width.Set(0f, 1f);

			// icon
			QuestType baseType = GetBaseQuestType(quest.QuestType);
            UIImageFramed image = new UIImageFramed(SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/Icons"), new Rectangle(Log2((int)baseType) * 18, 0, 18, 18));
            image.Left.Set(-10f, 0f);
            image.Top.Set(-10f, 0f);
            panel.Append(image);

            // text
            UISimpleWrappableText title = new UISimpleWrappableText(quest.QuestName, 0.7f);
            title.Left.Set(14f, 0f);
            title.Top.Set(-8f, 0f);
            title.Colour = new Color(43, 28, 17);
            panel.Append(title);

            // difficulty stars
            float pixels = -5f;
            Texture2D starImage = SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/Star");
            for (int i = 0; i < quest.Difficulty; i++)
            {
				UIImageFramed star = new UIImageFramed(starImage, starImage.Bounds);
                star.Left.Set(pixels, 1f);
                star.Top.Set(-8f, 0f);
                pixels -= 13f;
                panel.Append(star);
				panel.Stars.Add(star);
            }

			panel.Icon = image;
			panel.Title = title;

            return panel;
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
			{QuestType.Main, new Color(102, 160, 255)},
			{QuestType.Slayer, new Color(188, 38, 38)},
			{QuestType.Forager, new Color(68, 163, 112)},
			{QuestType.Explorer, new Color(153, 137, 196)},
			{QuestType.Designer, new Color(183, 151, 62)},
			{QuestType.Other, new Color(183, 71, 125)}
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
	}
}
