using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritMod.Mechanics.Coverings
{
    public class WaterMoss : LiquidCovering
    {
        protected override Texture2D Texture => _texture;
        private Texture2D _texture;

        public override void StaticLoad()
        {
            _texture = Mod.GetTexture("Mechanics/Coverings/WaterMoss");
        }
    }
}
