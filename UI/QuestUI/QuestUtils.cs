using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Town.QuestSystem;
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

        public static UIElement QuestAsPanel(Quest quest)
        {
            UISelectableOutlineRectPanel panel = new UISelectableOutlineRectPanel();
            panel.SelectedOutlineColour = new Color(102, 86, 67);
            panel.HoverOutlineColour = new Color(102, 86, 67) * 0.5f;
            panel.Height.Set(22f, 0f);
            panel.Width.Set(0f, 1f);

            // icon
            UIImage image = new UIImage(SpiritMod.Instance.GetTexture("UI/QuestUI/Textures/SlayerIcon"));
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
                UIImage star = new UIImage(starImage);
                star.Left.Set(pixels, 1f);
                star.Top.Set(-8f, 0f);
                pixels -= 13f;
                panel.Append(star);
            }

            return panel;
        }
    }
}
