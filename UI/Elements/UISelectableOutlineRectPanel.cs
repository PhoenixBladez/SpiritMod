using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.UI.QuestUI;
using Terraria;
using Terraria.UI;

namespace SpiritMod.UI.Elements
{
    public class UISelectableOutlineRectPanel : UIPanel
    {
        public bool IsSelected { get; set; }
		public bool DrawFilled { get; set; }
		public bool DrawBorder { get; set; }

		public Color HoverFillColour { get; set; }
        public Color SelectedFillColour { get; set; }

		public Color NormalOutlineColour { get; set; }
		public Color HoverOutlineColour { get; set; }
		public Color SelectedOutlineColour { get; set; }

		public UISelectableOutlineRectPanel()
		{
			DrawBorder = false;
			DrawFilled = true;
		}

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering || IsSelected)
            {
				if (DrawFilled) spriteBatch.Draw(Main.blackTileTexture, GetDimensions().ToRectangle(), IsSelected ? SelectedFillColour : HoverFillColour);
				if (DrawBorder) QuestUtils.DrawRectangleBorder(spriteBatch, GetDimensions().ToRectangle(), IsSelected ? SelectedOutlineColour : HoverOutlineColour);
            }
			else if (DrawBorder)
			{
				QuestUtils.DrawRectangleBorder(spriteBatch, GetDimensions().ToRectangle(), NormalOutlineColour);
			}
        }
    }
}
