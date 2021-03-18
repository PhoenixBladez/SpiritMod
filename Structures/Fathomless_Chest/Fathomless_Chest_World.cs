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
	public class Fathomless_Chest_World : ModWorld
	{
		public static bool isThereAChest = false;

		public override void Initialize()
		{
			isThereAChest = false;
		}
		private void PlaceShrineMiscs(int i, int j, int[,] ShrineArray) 
		{
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // Third Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.type != 41 || tile.type != 43 || tile.type != 44 || tile.type != 226))
					{					
						switch (ShrineArray[y, x]) {
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
			int radius = 14;
			for(int y = j - radius; y <= j + radius; y++) {
				for(int x = i - radius; x <= i + radius + 1; x++) {
					if ((int)Vector2.Distance(new Vector2(x, y), new Vector2(i, j)) <= radius)
						WorldGen.KillTile(x, y);
				}
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
					if (WorldGen.InWorld(k, l, 30) && (tile.type != 41 || tile.type != 43 || tile.type != 44 || tile.type != 226))
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
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // Second Loop Places Blocks
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					Tile tile = Framing.GetTileSafely(k, l);
					if (WorldGen.InWorld(k, l, 30) && (tile.type != 41 || tile.type != 43 || tile.type != 44 || tile.type != 226)) {
						switch (ShrineArray[y, x]) {
							case 0:
								break;
							case 1:				
								break;
							case 2:
								
								WorldGen.PlaceTile(k, l, mod.TileType("Black_Stone")); // Dirt
								WorldGen.PlaceWall(k, l, 1); // Stone Wall	
								tile.active(true);
								break;
							case 3:
								
								WorldGen.PlaceTile(k, l, 311); // Dynasty Wood							
								tile.active(true);
								break;
							case 4:
								
								WorldGen.PlaceWall(k, l, 139); // Rich Mahogany Fence	
								WorldGen.PlaceObject(k, l, 105, false, 31); // Tree Statue								
								break;
							case 5:
								
								WorldGen.PlaceTile(k, l, 313); // Blue Dynasty Shingles			
								tile.active(true);
								break;
							case 6:
								
								WorldGen.PlaceTile(k, l, 313); // Blue Dynasty Shingles
								tile.active(true);
								tile.slope(0);
								break;
							case 7:
								WorldGen.PlaceTile(k, l, 311); // Dynasty Wood
								WorldGen.PlaceWall(k, l, 139); // Rich Mahogany Fence
								
								tile.active(true);
								break;
						}
					}
				}
			}
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // Third Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x]) {
							case 8:
								
								WorldGen.PlaceObject(k, l, 105, false, 31); // Tree Statue
								break;
							case 9:
								
								WorldGen.PlaceTile(k, l, mod.TileType("Fathomless_Chest")); // Blue Dynasty Shingles
								break;
						}
					}
				}
			}
		}

		private void PlaceBeams(int i, int j)
		{
			for(int beam = -2; beam <= 2; beam++) {
				if (beam == 0)
					continue;

				int x = i + (beam * 3) + ((beam > 0) ? 1 : 0);
				int y = j + 5 + ((Math.Abs(beam) == 1) ? 1 : 0);
				bool reachedsolidtile = false;
				while(!reachedsolidtile) { //loop until it reaches a solid tile
					if (WorldGen.SolidOrSlopedTile(Framing.GetTileSafely(x, y))) 
						reachedsolidtile = true;
					
					else {
						Framing.GetTileSafely(x, y).ClearTile();
						WorldGen.PlaceTile(x, y, TileID.WoodenBeam); // Rich Mahogany Fence
						y++;
					}
				}
			}
		}

		public override void PostWorldGen()
        {
			int numberOfShrines = (Main.maxTilesX - 200) / 200;
			int xSegment = ((Main.maxTilesX - 200) / numberOfShrines);
			for (int i = 0; i < numberOfShrines; i++) {
				int min = Math.Max(xSegment * i, 100);
				int max = Math.Min(xSegment * (i + 1), Main.maxTilesX - 100);
				finalShrineGen(WorldGen.genRand.Next(min, max), WorldGen.genRand.Next(Convert.ToInt32(Main.rockLayer), Convert.ToInt32(Main.rockLayer + 500)));
			}
		}

		public void finalShrineGen(int spawnposXa, int spawnposYa)
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

			for (int y = spawnposYa - radius; y <= spawnposYa + radius; y++) {
				for (int x = spawnposXa - radius; x <= spawnposXa + radius + 1; x++) {
					Tile checktile = Framing.GetTileSafely(x, y);

					if (forbiddentiles.Contains(checktile.type) || forbiddenwalls.Contains(checktile.wall) || TileID.Sets.BasicChest[checktile.type] || TileID.Sets.BasicChestFake[checktile.type])
						safetyCheck = true;
				}
			}

			while (safetyCheck)
            {
                spawnposXa = Main.rand.Next(100, Main.maxTilesX - 100);
                spawnposYa = WorldGen.genRand.Next(Convert.ToInt32(Main.rockLayer), Convert.ToInt32(Main.rockLayer + 500));
				safetyCheck = false;
				for (int y = spawnposYa - radius; y <= spawnposYa + radius; y++) {
					for (int x = spawnposXa - radius; x <= spawnposXa + radius + 1; x++) {
						if (forbiddentiles.Contains(Framing.GetTileSafely(x, y).type) || forbiddenwalls.Contains(Framing.GetTileSafely(x, y).wall))
							safetyCheck = true;
					}
				}

			}

			ClearCircle(spawnposXa + 3, spawnposYa);
            PlaceShrine(spawnposXa, spawnposYa, Structures.Fathomless_Chest.Fathomless_Chest_Arrays.ShrineShape1);
            PlaceShrineMiscs(spawnposXa, spawnposYa, Structures.Fathomless_Chest.Fathomless_Chest_Arrays.Miscs);
			PlaceBeams(spawnposXa + 3, spawnposYa);
		}
	}
}