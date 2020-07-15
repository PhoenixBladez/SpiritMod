using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Tiles.Block
{
	public class SpiritGrass : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new SpiritTree());
			Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(0, 191, 255));
			dustType = 187;
			drop = ModContent.ItemType<SpiritDirtItem>();
		}

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			TileObject toBePlaced;
			if(!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false)) {
				return false;
			}
			toBePlaced.random = random;
			if(TileObject.Place(toBePlaced) && !mute) {
				WorldGen.SquareTileFrame(x, y, true);
				//   Main.PlaySound(SoundID.Dig, x * 16, y * 16, 1, 1f, 0f);
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if(!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(40) == 0) {
				switch(Main.rand.Next(5)) {
					case 0:
						SpiritGrass.PlaceObject(i, j - 1, mod.TileType("SpiritGrassA1"));
						NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("SpiritGrassA1"), 0, 0, -1, -1);
						break;
					case 1:
						SpiritGrass.PlaceObject(i, j - 1, mod.TileType("SpiritGrassA2"));
						NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("SpiritGrassA2"), 0, 0, -1, -1);
						break;
					case 2:
						SpiritGrass.PlaceObject(i, j - 1, mod.TileType("SpiritGrassA3"));
						NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("SpiritGrassA3"), 0, 0, -1, -1);
						break;
					case 3:
						SpiritGrass.PlaceObject(i, j - 1, mod.TileType("SpiritGrassA4"));
						NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("SpiritGrassA4"), 0, 0, -1, -1);
						break;

					default:
						SpiritGrass.PlaceObject(i, j - 1, mod.TileType("SpiritGrassA5"));
						NetMessage.SendObjectPlacment(-1, i, j - 1, mod.TileType("SpiritGrassA5"), 0, 0, -1, -1);
						break;
				}

			}


		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);
			Tile tile2 = Framing.GetTileSafely(i, j-1);
            if (tile.slope() == 0 && !tile.halfBrick())
            {
                if (!Main.tileSolid[tile2.type] || !tile2.active())
                {
                    Color colour = Color.White * MathHelper.Lerp(0.2f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));

                    Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/Block/SpiritGrass_Glow");
                    Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

                    spriteBatch.Draw(glow, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.frameX, tile.frameY, 16, 16), colour);
                }
            }
        }
        public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<SpiritSapling>();
		}

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile2 = Framing.GetTileSafely(i, j - 1);
            if (!Main.tileSolid[tile2.type] || !tile2.active())
            {
                r = 0.3f;
                g = 0.45f;
                b = 1.05f;
            }
		}

	}
}

