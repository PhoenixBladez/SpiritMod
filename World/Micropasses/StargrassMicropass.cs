using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SpiritMod.World.Micropasses
{
	internal class StargrassMicropass
	{
		public static void Run()
		{
			float worldSize = Main.maxTilesX / 4200f;
			for (int i = 0; i < 4 * worldSize; i++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = (int)(Main.worldSurface * 0.35f);
				bool fail = false;

				while (!Main.tile[x, y].HasTile || Main.tile[x, y].TileType != TileID.Grass)
				{
					y++;

					if (y > Main.worldSurface)
					{
						fail = true;
						break;
					}
				}

				if (fail)
				{
					i--;
					continue;
				}

				SpreadStargrass(x, y);
			}
		}

		private static void SpreadStargrass(int x, int y)
		{
			int size = WorldGen.genRand.Next(12, 20);

			for (int i = x - size; i < x + size; ++i)
			{
				for (int j = y - size; j < y + size; ++j)
				{
					if (Vector2.DistanceSquared(new Vector2(x, y), new Vector2(i, j)) < size * size)
					{
						Tile tile = Main.tile[i, j];

						if (tile.TileType == TileID.Grass)
							tile.TileType = (ushort)Terraria.ModLoader.ModContent.TileType<Tiles.Block.Stargrass>();
					}
				}
			}
		}
	}
}
