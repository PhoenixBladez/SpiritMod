using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace SpiritMod.UI.Elements
{
    public class UISolid : UIElement
    {
        public Color Color { get; set; }

        public UISolid()
        {
            Color = Color.White;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureAssets.BlackTile.Value, base.GetDimensions().ToRectangle(), null, Color);
        }
    }
}
