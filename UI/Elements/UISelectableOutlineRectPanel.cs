using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.UI.QuestUI;
using Terraria.UI;

namespace SpiritMod.UI.Elements
{
    public class UISelectableOutlineRectPanel : UIPanel
    {
        public bool IsSelected { get; set; }
        
        public Color HoverOutlineColour { get; set; }
        public Color SelectedOutlineColour { get; set; }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering || IsSelected)
            {
                QuestUtils.DrawRectangleBorder(spriteBatch, GetDimensions().ToRectangle(), IsSelected ? SelectedOutlineColour : HoverOutlineColour);
            }
        }
    }
}
