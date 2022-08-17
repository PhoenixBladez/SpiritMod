using Terraria;
using Terraria.IO;
using Terraria.ID;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.World.Micropasses
{
	internal class BoulderMicropass
	{
		static WeightedRandom<int> StoneType
		{
			get
			{
				WeightedRandom<int> types = new WeightedRandom<int>(WorldGen.genRand);
				types.Add(TileID.Stone, 1f);
				types.Add(TileID.Amethyst, 0.001f);
				types.Add(TileID.Topaz, 0.0008f);
				types.Add(TileID.Sapphire, 0.0006f);
				types.Add(TileID.Emerald, 0.002f);
				types.Add(TileID.Ruby, 0.002f);
				types.Add(TileID.Diamond, 0.001f);
				return types;
			}
		}

		public static void Run(GenerationProgress progress, GameConfiguration config)
		{
			float worldSize = Main.maxTilesX / 4200f;
			for (int i = 0; i < 8 * worldSize; i++)
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

				if (WorldGen.genRand.NextBool(4))
					PlaceSingleBoulder(x, y);
				else
					PlaceSingleBoulderFlatFaced(x, y);
			}
		}

		internal static void PlaceSingleBoulderFlatFaced(int x, int y)
		{
			Vector2 origin = new Vector2(x, y);
			List<float> sections = new();
			int sectCount = WorldGen.genRand.Next(5, 10);

			for (int i = 1; i < sectCount; i++)
			{
				float offset = WorldGen.genRand.NextFloat(MathHelper.TwoPi / sectCount / -2, MathHelper.TwoPi / sectCount / 2);
				sections.Add(MathHelper.TwoPi / i + offset);
			}

			sections.Reverse(); //So we get the whole thing in the right order
			float currentSect = 0;
			float nextSect = sections.First();

			List<int> sectSizes = new List<int>();

			for (int i = 0; i < sections.Count; ++i)
			{
				if (i == 0)
					sectSizes.Add(WorldGen.genRand.Next(4, 11));
				else
				{
					int newSize = sectSizes[i - 1] + WorldGen.genRand.Next(-2, 3);

					if (newSize < 2)
						newSize = 2;
					else if (newSize > 12)
						newSize = 12;

					sectSizes.Add(newSize);
				}
			}

			float curLength = sectSizes[0];
			float nextLength = sectSizes[1];

			for (float radians = 0; radians < MathHelper.TwoPi; radians += MathHelper.TwoPi * 0.005f)
			{
				float length = MathHelper.Lerp(curLength, nextLength, (radians - currentSect) / nextLength);
				Utils.PlotTileLine(origin.ToWorldCoordinates(), (origin + new Vector2(length, 0).RotatedBy(radians)).ToWorldCoordinates(), 2f, PlaceTilesInLine);
			}
		}

		private static bool PlaceTilesInLine(int x, int y)
		{
			ushort type = (ushort)StoneType;
			Tile tile = Main.tile[x, y];

			if (tile.HasTile)
				WorldGen.ReplaceTile(x, y, type, 0);
			else
				WorldGen.PlaceTile(x, y, type, true, true);
			tile.Slope = SlopeType.Solid;
			tile.IsHalfBlock = false;
			return true;
		}

		private static void PlaceSingleBoulder(int x, int y, bool doRepeat = true, int sizeOverride = -1)
		{
			int radius = WorldGen.genRand.Next(3, 6);
			int checkRadius = radius;

			if (sizeOverride > -1)
				radius = sizeOverride;

			if (radius <= 0)
				return;

			for (int i = x - radius; i < x + radius; ++i)
			{
				for (int j = y - radius; j < y + radius; ++j)
				{
					Tile tile = Main.tile[i, j];
					if (Vector2.DistanceSquared(new(i, j), new(x, y)) < checkRadius * checkRadius && TileID.Sets.CanBeClearedDuringGeneration[tile.TileType])
					{
						ushort type = (ushort)StoneType;
						if (tile.HasTile)
							WorldGen.ReplaceTile(i, j, type, 0);
						else
							WorldGen.PlaceTile(i, j, type, true, true);
						tile.Slope = SlopeType.Solid;
						tile.IsHalfBlock = false;
					}
				}
			}

			PlaceSingleBoulder(x - WorldGen.genRand.Next(-4, 5), y - WorldGen.genRand.Next(-4, 5), false, radius - WorldGen.genRand.Next(2));
		}
	}
}
