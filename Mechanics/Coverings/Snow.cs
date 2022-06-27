using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;

namespace SpiritMod.Mechanics.Coverings
{
    public class Snow : TextureTileCovering
    {
        protected override Texture2D Texture => _texture;
        private Texture2D _texture;

        public override void StaticLoad()
        {
			if (Main.netMode != NetmodeID.Server)
				_texture = Mod.GetTexture("Mechanics/Coverings/Snow");
        }

        public override bool IsValidAt(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return WorldGen.SolidOrSlopedTile(tile);
        }

        protected override Point GetFrame(int variation, int orientation)
        {
            // we have to mod 4 here as the first two bits in the variation value for snow represent the alpha
            return base.GetFrame(variation % 4, orientation);
        }

        public override void Update(GameTime gameTime, int x, int y, int variation, int orientation)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            bool isSnowTile = TileID.Sets.IcesSnow[tile.TileType];

            // if we're not in the tundra and this block isn't a snow tile of any kind, randomly remove
            if (!isSnowTile && !Main.LocalPlayer.ZoneSnow && Main.rand.Next(300) == 0)
            {
                int alpha = GetAlphaFromVariation(variation);

                // if our new alpha is going to be -1, remove this covering
                if (alpha == 0)
                {
                    CoveringsManager.RemoveAt(x, y);
                    return;
                }

                int newVariation = ChangeAlphaOnVariation(variation, -1);
                CoveringsManager.SetVariation(x, y, newVariation);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y, int variation, int orientation)
        {
            if (orientation == 0) return;

            Tile tile = Framing.GetTileSafely(x, y);
            byte slope = tile.Slope;
            bool hb = tile.IsHalfBlock;

            Color clr = Lighting.GetColor(x, y, Color);
            clr *= (GetAlphaFromVariation(variation) + 1) / 4f;

            DrawCoveringsOnTileVariations(spriteBatch, x, y, clr, hb, slope, variation, orientation);
        }

        public static int MakeVariation(int frameVariation, int alpha)
        {
            return (alpha << 2) | (frameVariation % 4);
        }

        public static int GetAlphaFromVariation(int variation) => variation >> 2;

        public static int ChangeAlphaOnVariation(int variation, int deltaAlpha)
        {
            int alpha = GetAlphaFromVariation(variation);
            alpha += deltaAlpha;
            // clamp between 0 and 3 inclusive
            alpha = Math.Min(3, Math.Max(0, alpha));
            return (alpha << 2) | (variation % 4);
        }
    }
}
