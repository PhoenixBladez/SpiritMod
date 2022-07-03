using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;

namespace SpiritMod.Mechanics.Coverings
{
    public abstract class TextureTileCovering : TileCovering
    {
        protected virtual Color Color => Color.White;
        protected virtual Texture2D Texture => throw new NotImplementedException();

        protected virtual Point GetFrame(int variation, int orientation)
        {
            int frameX = variation * 18;
            // orientation converts straight to the y frame, you just have to subtract 1
            int frameY = (orientation - 1) * 18;

            return new Point(frameX, frameY);
        }

        public override void Update(GameTime gameTime, int x, int y, int variation, int orientation)
        {
            ReadableOrientation readableOrientation = new ReadableOrientation(orientation);

            if (WorldGen.SolidTile(x - 1, y)) readableOrientation.Left = false;
            if (WorldGen.SolidTile(x + 1, y)) readableOrientation.Right = false;
            if (WorldGen.SolidTile(x, y - 1)) readableOrientation.Up = false;
            if (WorldGen.SolidTile(x, y + 1)) readableOrientation.Down = false;

            CoveringsManager.SetOrientation(x, y, (int)readableOrientation);
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y, int variation, int orientation)
        {
            if (orientation == 0) return;

            Tile tile = Framing.GetTileSafely(x, y);
            byte slope = (byte)tile.Slope;
            bool hb = tile.IsHalfBlock;

            Color clr = Lighting.GetColor(x, y, Color);

            DrawCoveringsOnTileVariations(spriteBatch, x, y, clr, hb, slope, variation, orientation);
        }

        protected void DrawCoveringsOnTileVariations(SpriteBatch spriteBatch, int x, int y, Color clr, bool halfBrick, byte slope, int variation, int orientation)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            Point frame = GetFrame(variation, orientation);
            ReadableOrientation readOri = (ReadableOrientation)orientation;

            // if this has an up/left orientation and so does the one to bottom left
            if ((readOri.Left || (tile.Slope == Terraria.ID.SlopeType.SlopeDownRight && (readOri.Up || readOri.Left))) && ((ReadableOrientation)CoveringsManager.GetData(x - 1, y + 1).Orientation).Up)
            {
                DrawSpecificYFrame(spriteBatch, x, y + 1, frame, 20, clr);
                if (halfBrick) readOri.Left = true;
            }

            // if this has an up orientation and so does the one to bottom right
            if ((readOri.Right || (tile.Slope == Terraria.ID.SlopeType.SlopeDownLeft && (readOri.Up || readOri.Right))) && ((ReadableOrientation)CoveringsManager.GetData(x + 1, y + 1).Orientation).Up)
            {
                DrawSpecificYFrame(spriteBatch, x, y + 1, frame, 20, clr, SpriteEffects.FlipHorizontally);
                if (halfBrick) readOri.Right = true;
            }

            // if this has an down orientation and so does the one to bottom left
            if (readOri.Down && ((ReadableOrientation)CoveringsManager.GetData(x - 1, y + 1).Orientation).Down)
            {
                DrawSpecificYFrame(spriteBatch, x - 1, y, frame, 20, clr, SpriteEffects.FlipVertically);
            }

            // if this has an down orientation and so does the one to bottom left
            if (readOri.Down && ((ReadableOrientation)CoveringsManager.GetData(x + 1, y + 1).Orientation).Down)
            {
                DrawSpecificYFrame(spriteBatch, x + 1, y, frame, 20, clr, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
            }

            // we do this so any changes made to the orientation from the above hacks take effect
            frame = GetFrame(variation, (int)readOri);

            if (halfBrick)
            {
                switch ((int)readOri)
                {
                    case 4:
                        break;
                    case 1: // draw top half of texture moved down a bit
                    case 2:
                    case 3:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        spriteBatch.Draw(Texture, new Vector2(x, y) * 16f - Main.screenPosition + Vector2.UnitY * 8f, new Rectangle(frame.X, frame.Y, 16, 8), clr, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        return;
                    case 5:
                    case 7:
                    case 13:
                    case 15:
                        // draw bottom half first
                        spriteBatch.Draw(Texture, new Vector2(x, y) * 16f - Main.screenPosition + Vector2.UnitY * 8f, new Rectangle(frame.X, frame.Y + 8, 16, 8), clr, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        // draw top half moved down a bit
                        spriteBatch.Draw(Texture, new Vector2(x, y) * 16f - Main.screenPosition + Vector2.UnitY * 8f, new Rectangle(frame.X, frame.Y, 16, 8), clr, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        return;
                    case 6: // draw bottom half
                    case 12:
                    case 14:
                        spriteBatch.Draw(Texture, new Vector2(x, y) * 16f - Main.screenPosition + Vector2.UnitY * 8f, new Rectangle(frame.X, frame.Y + 8, 16, 8), clr, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        return;
                }
            }

            if (slope > 0)
            {
                switch ((int)readOri)
                {
                    case 1:
                        if (slope == 1)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                            return;
                        }
                        if (slope == 2)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                            return;
                        }
                        break;
                    case 2:
                        if (slope == 1)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                            return;
                        }
                        if (slope == 3)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                            return;
                        }
                        break;
                    case 3:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 2, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 1, clr);
                                return;
                        }
                        break;
                    case 4:
                        if (slope == 3)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                            return;
                        }
                        if (slope == 4)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                            return;
                        }
                        break;
                    case 5:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 4, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 4, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 1, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 1, clr);
                                return;
                        }
                        break;
                    case 6:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 4, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 2, clr);
                                return;
                        }
                        break;
                    case 7:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 4, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 6, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 1, clr); // BR
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 3, clr);
                                return;
                        }
                        break;
                    case 8:
                        if (slope == 2)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // BR
                            return;
                        }
                        if (slope == 4)
                        {
                            DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                            return;
                        }
                        break;
                    case 9:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 8, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 1, clr);
                                return;
                        }
                        break;
                    case 10:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 8, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 2, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 8, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 2, clr);
                                return;
                        }
                        break;
                    case 11:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 8, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 2, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 9, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 3, clr);
                                return;
                        }
                        break;
                    case 12:
                        switch (slope)
                        {
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 4, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 8, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                return;
                        }
                        break;
                    case 13:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 12, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 4, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 9, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 1, clr);
                                return;
                        }
                        break;
                    case 14:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 12, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 6, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 8, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 2, clr);
                                return;
                        }
                        break;
                    case 15:
                        switch (slope)
                        {
                            case 1:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 17, clr); // TR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 12, clr);
                                return;
                            case 2:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 16, clr); // TL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 6, clr);
                                return;
                            case 3:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 18, clr); // BR
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 9, clr);
                                return;
                            case 4:
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 19, clr); // BL
                                DrawSpecificYFrame(spriteBatch, x, y, frame, 3, clr);
                                return;
                        }
                        break;
                }
            }

            spriteBatch.Draw(Texture, new Vector2(x, y) * 16f - Main.screenPosition, new Rectangle(frame.X, frame.Y, 16, 16), clr, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        private void DrawSpecificYFrame(SpriteBatch spriteBatch, int x, int y, Point oldFrame, int yFrameBlock, Color clr, SpriteEffects se = SpriteEffects.None)
        {
            spriteBatch.Draw(Texture, new Vector2(x, y) * 16f - Main.screenPosition, new Rectangle(oldFrame.X, (yFrameBlock - 1) * 18, 16, 16), clr, 0f, Vector2.Zero, 1f, se, 0f);
        }
    }
}
