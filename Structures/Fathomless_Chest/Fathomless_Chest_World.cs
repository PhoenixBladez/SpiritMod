using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Block;

namespace SpiritMod.Structures.Fathomless_Chest
{
	public class Fathomless_Chest_World : ModSystem
	{
		public static bool isThereAChest = false;

		public override void OnWorldLoad() => isThereAChest = false;

		private void PlaceShrineMiscs(int i, int j, int[,] ShrineArray)
		{
			for (int y = 0; y < ShrineArray.GetLength(0); y++)
			{ // Third Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.TileType != 41 || tile.TileType != 43 || tile.TileType != 44 || tile.TileType != 226))
					{
						switch (ShrineArray[y, x])
						{
							case 8:
								WorldGen.PlaceObject(k, l, 42, false, 2); //Lantern
								break;
						}
					}
				}
			}
		}

		private void ClearCircle(int i, int j)
		{
			const int BaseRadius = 12;
			int radius = BaseRadius;

			for (int y = j - radius; y <= j + radius; y++)
			{
				for (int x = i - radius; x <= i + radius + 1; x++)
				{
					if ((int)Vector2.Distance(new Vector2(x, y), new Vector2(i, j)) <= radius)
						WorldGen.KillTile(x, y);
				}

				radius = BaseRadius - WorldGen.genRand.Next(-1, 2);
			}
		}

		private void PlaceShrine(int i, int j, int[,] ShrineArray)
		{
			for (int y = 0; y < ShrineArray.GetLength(0); y++)
			{ // First loop is here to clear tiles properly, and not delete objects afterwards.
				for (int x = 0; x < ShrineArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.TileType != 41 || tile.TileType != 43 || tile.TileType != 44 || tile.TileType != 226))
					{
						switch (ShrineArray[y, x])
						{
							case 4:
								Framing.GetTileSafely(k, l).ClearEverything();
								break;

							default:
								break;
						}
					}
				}
			}

			for (int y = 0; y < ShrineArray.GetLength(0); y++)
			{ // Second Loop Places Blocks
				for (int x = 0; x < ShrineArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.TileType != 41 || tile.TileType != 43 || tile.TileType != 44 || tile.TileType != 226))
					{
						switch (ShrineArray[y, x])
						{
							case 0:
							case 1:
								break;
							case 2:
								WorldGen.PlaceTile(k, l, Mod.Find<ModTile>("Black_Stone").Type); // Dirt
								WorldGen.PlaceWall(k, l, 1); // Stone Wall	
								tile.HasTile = true;
								break;
							case 3:
								WorldGen.PlaceTile(k, l, 311); // Dynasty Wood							
								tile.HasTile = true;
								break;
							case 4:
								WorldGen.PlaceWall(k, l, 139); // Rich Mahogany Fence	
								WorldGen.PlaceObject(k, l, 105, false, 31); // Tree Statue								
								break;
							case 5:
								WorldGen.PlaceTile(k, l, 313); // Blue Dynasty Shingles			
								tile.HasTile = true;
								break;
							case 6:
								WorldGen.PlaceTile(k, l, 313); // Blue Dynasty Shingles
								tile.HasTile = true;
								tile.Slope = 0;
								break;
							case 7:
								WorldGen.PlaceTile(k, l, 311); // Dynasty Wood
								WorldGen.PlaceWall(k, l, 139); // Rich Mahogany Fence
								tile.HasTile = true;
								break;
						}
					}
				}
			}

			for (int y = 0; y < ShrineArray.GetLength(0); y++)
			{ // Third Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						switch (ShrineArray[y, x])
						{
							case 8:
								WorldGen.PlaceObject(k, l, 105, false, 31); // Tree Statue
								break;
							case 9:
								WorldGen.PlaceTile(k, l, Mod.Find<ModTile>("Fathomless_Chest").Type); // Blue Dynasty Shingles
								break;
						}
					}
				}
			}
		}

		private void PlaceBeams(int i, int j)
		{
			for (int beam = -2; beam <= 2; beam++)
			{
				if (beam == 0)
					continue;

				int x = i + (beam * 3) + ((beam > 0) ? 1 : 0);
				int y = j + 5 + ((Math.Abs(beam) == 1) ? 1 : 0);
				bool reachedsolidtile = false;
				while (!reachedsolidtile) // Loop until it reaches a solid tile
				{
					if (WorldGen.SolidOrSlopedTile(Framing.GetTileSafely(x, y)) || TileID.Sets.BasicChest[Framing.GetTileSafely(x, y).TileType] || TileID.Sets.BasicChestFake[Framing.GetTileSafely(x, y).TileType])
						reachedsolidtile = true;
					else
					{
						Framing.GetTileSafely(x, y).ClearTile();
						WorldGen.PlaceTile(x, y, TileID.WoodenBeam);
						y++;
					}
				}
			}
		}

		public override void PostWorldGen()
		{
			int numberOfShrines = (Main.maxTilesX - 200) / 200;
			int xSegment = (Main.maxTilesX - 200) / numberOfShrines;

			for (int i = 0; i < numberOfShrines; i++)
			{
				int min = Math.Max(xSegment * i, 100);
				int max = Math.Min(xSegment * (i + 1), Main.maxTilesX - 100);
				FinalShrineGen(WorldGen.genRand.Next(min, max), WorldGen.genRand.Next((int)Main.rockLayer, (int)Main.rockLayer + 500));
			}
		}

		public void FinalShrineGen(int spawnX, int spawnY)
		{
			isThereAChest = true;
			bool safetyCheck = false;
			int radius = 14;

			int[] forbiddentiles = new int[]
			{
				TileID.LihzahrdBrick,
				TileID.Hive,
				TileID.BlueDungeonBrick,
				TileID.GreenDungeonBrick,
				TileID.PinkDungeonBrick,
				ModContent.TileType<BloodBlossom>(),
				ModContent.TileType<ReachGrassTile>()
			};

			ushort[] forbiddenwalls = new ushort[]
			{
				WallID.LihzahrdBrickUnsafe,
				WallID.HiveUnsafe,
				7,
				8,
				9,
				WallID.BlueDungeonUnsafe,
				WallID.GreenDungeonUnsafe,
				WallID.PinkDungeonUnsafe,
				WallID.BlueDungeonSlabUnsafe,
				WallID.GreenDungeonSlabUnsafe,
				WallID.PinkDungeonSlabUnsafe,
				WallID.BlueDungeonTileUnsafe,
				WallID.GreenDungeonTileUnsafe,
				WallID.PinkDungeonTileUnsafe
			};

			for (int y = spawnY - radius; y <= spawnY + radius; y++)
			{
				for (int x = spawnX - radius; x <= spawnX + radius + 1; x++)
				{
					Tile tile = Framing.GetTileSafely(x, y);

					if (forbiddentiles.Contains(tile.TileType) || forbiddenwalls.Contains(tile.WallType) || TileID.Sets.BasicChest[tile.TileType] || 
						TileID.Sets.BasicChestFake[tile.TileType] || !TileID.Sets.CanBeClearedDuringGeneration[tile.TileType])
						safetyCheck = true;
				}
			}

			while (safetyCheck)
			{
				spawnX = Main.rand.Next(100, Main.maxTilesX - 100);
				spawnY = WorldGen.genRand.Next((int)Main.rockLayer, (int)Main.rockLayer + 500);
				safetyCheck = false;

				for (int y = spawnY - radius; y <= spawnY + radius; y++)
				{
					for (int x = spawnX - radius; x <= spawnX + radius + 1; x++)
					{
						Tile tile = Framing.GetTileSafely(x, y);
						if (forbiddentiles.Contains(tile.TileType) || forbiddenwalls.Contains(tile.WallType) || !TileID.Sets.CanBeClearedDuringGeneration[tile.TileType])
							safetyCheck = true;
					}
				}
			}

			ClearCircle(spawnX + 4, spawnY);
			PlaceShrine(spawnX, spawnY, Fathomless_Chest_Arrays.ShrineShape1);
			PlaceShrineMiscs(spawnX, spawnY, Fathomless_Chest_Arrays.Miscs);
			PlaceBeams(spawnX + 3, spawnY);
		}
	}
}