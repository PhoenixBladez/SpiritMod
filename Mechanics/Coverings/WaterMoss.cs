using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Mechanics.Coverings
{
    public class WaterMoss : LiquidCovering
    {
        protected override Texture2D Texture => _texture;
        private Texture2D _texture;

        public override void StaticLoad()
        {
			if (Main.netMode != NetmodeID.Server)
				_texture = Mod.GetTexture("Mechanics/Coverings/WaterMoss");
        }
    }
}
