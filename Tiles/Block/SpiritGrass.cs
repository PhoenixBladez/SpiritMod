using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SpiritMod.Tiles.Block
{
	public class SpiritGrass : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new SpiritTree());
			Main.tileMerge[Type][mod.TileType("SpiritDirt")] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(0, 191, 255));
			drop = mod.ItemType("SpiritDirtItem");
		}

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			TileObject toBePlaced;
			if (!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false))
			{
				return false;
			}
			toBePlaced.random = random;
			if (TileObject.Place(toBePlaced) && !mute)
			{
				WorldGen.SquareTileFrame(x, y, true);
				//   Main.PlaySound(0, x * 16, y * 16, 1, 1f, 0f);
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (!Framing.GetTileSafely(i, j-1).active() && Main.rand.Next(40) == 0)
			{
				switch (Main.rand.Next(5))
				{
					case 0:
						SpiritGrass.PlaceObject(i, j-1, mod.TileType("SpiritGrassA1"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("SpiritGrassA1"), 0, 0, -1, -1);
						break;
					case 1:
						SpiritGrass.PlaceObject(i, j-1, mod.TileType("SpiritGrassA2"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("SpiritGrassA2"), 0, 0, -1, -1);
						break;
					case 2:
						SpiritGrass.PlaceObject(i, j-1, mod.TileType("SpiritGrassA3"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("SpiritGrassA3"), 0, 0, -1, -1);
						break;
					case 3:
						SpiritGrass.PlaceObject(i, j-1, mod.TileType("SpiritGrassA4"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("SpiritGrassA4"), 0, 0, -1, -1);
						break;

					default:
						SpiritGrass.PlaceObject(i, j-1, mod.TileType("SpiritGrassA5"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("SpiritGrassA5"), 0, 0, -1, -1);
						break;
				}

			}


		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return mod.TileType("SpiritSapling");
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.4f;
			g = 0.6f;
			b = 1.4f;
		}

	}
}

