using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace SpiritMod.Mechanics.Coverings
{
    public abstract class LiquidCovering : TextureTileCovering
    {
		public override RenderLayers Layer => RenderLayers.ForegroundWater;

		// Liquid coverings only have a top layer
		protected override Point GetFrame(int variation, int orientation)
        {
            return new Point(variation * 18, 0);
        }

        public override bool IsValidAt(int x, int y)
        {
            // if this tile has no liquid and the one below doesn't either, it's not valid anymore
            Tile tile = Framing.GetTileSafely(x, y);
            Tile above = Framing.GetTileSafely(x, y - 1);
            if (tile.liquid == 0)
            {
                Tile below = Framing.GetTileSafely(x, y + 1);
                if (below.liquid == 0)
                {
                    return false;
                }
            }

            // this should stop a few weird situations where tiles are directly above a covering
            if (tile.liquid > 240 && WorldGen.SolidOrSlopedTile(above)) return false;

            // otherwise, as long as this tile is empty we gucci
            return !WorldGen.SolidOrSlopedTile(x, y);
        }

        public override void Update(GameTime gameTime, int x, int y, int variation, int orientation)
        {
            // try move upwards
            Tile above = Framing.GetTileSafely(x, y - 1);
            if (above != null && above.liquid > 0)
            {
                CoverData data = CoveringsManager.GetData(x, y);
                CoveringsManager.RemoveAt(x, y);
                CoveringsManager.SetData(x, y - 1, data);

                // update that tile
                Update(gameTime, x, y - 1, variation, orientation);
                return; // return quick
            }

            // try move downwards if our current tile is empty
            Tile tile = Framing.GetTileSafely(x, y);
            if (tile.liquid == 0)
            {
                CoverData data = CoveringsManager.GetData(x, y);
                // because IsValidAt(x,y) returned true, we know the tile below has liquid in it
                // so just move this covering to that tile
                CoveringsManager.RemoveAt(x, y);
                CoveringsManager.SetData(x, y + 1, data);
                
                // update that tile
                Update(gameTime, x, y + 1, variation, orientation);
                return; // return quick
            }

            base.Update(gameTime, x, y, variation, orientation);
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y, int variation, int orientation)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            float percent = tile.liquid / 255f;
            float drawY = (y + 1) * 16f - 16f * percent - 4f; // subtracting 4f here just to make it sit slightly on top by default

            Color clr = Lighting.GetColor(x, y, Color);
            var frame = GetFrame(variation, orientation);
            spriteBatch.Draw(Texture, new Vector2(x * 16f, drawY) - Main.screenPosition, new Rectangle(frame.X, frame.Y, 16, 16), clr);
        }
    }
}
