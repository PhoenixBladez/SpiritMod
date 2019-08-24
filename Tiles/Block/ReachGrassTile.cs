using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SpiritMod.Tiles.Block
{
	public class ReachGrassTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new ReachTree());
			Main.tileMerge[Type][mod.TileType("ReachGrassTile")] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileLighted[Type] = true;
			AddMapEntry(new Color(0, 255, 167));
			drop = mod.ItemType("ReachGrass");
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
			if (!Framing.GetTileSafely(i, j-1).active() && Main.rand.Next(10) == 0)
			{
				switch (Main.rand.Next(7))
				{
					case 0:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA1"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA1"), 0, 0, -1, -1);
						break;
					case 1:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA2"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA2"), 0, 0, -1, -1);
						break;
					case 2:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA3"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA3"), 0, 0, -1, -1);
						break;
					case 3:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA4"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA4"), 0, 0, -1, -1);
						break;
					case 4:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA5"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA5"), 0, 0, -1, -1);
						break;
					case 5:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA6"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA6"), 0, 0, -1, -1);
						break;

					default:
						ReachGrassTile.PlaceObject(i, j-1, mod.TileType("ReachGrassA7"));
						NetMessage.SendObjectPlacment(-1, i, j-1, mod.TileType("ReachGrassA7"), 0, 0, -1, -1);
						break;
				}

			}


		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return mod.TileType("ReachSapling");
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.028f;
			g = 0.153f;
			b = 0.081f;
		}
	}
}

