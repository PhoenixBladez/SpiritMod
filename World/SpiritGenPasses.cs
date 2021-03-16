using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.IceSculpture.Hostile;
using SpiritMod.Tiles.Ambient.SpaceCrystals;
using SpiritMod.Tiles.Ambient.SurfaceIce;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Furniture;
using SpiritMod.Tiles.Piles;
using SpiritMod.Tiles.Walls.Natural;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace SpiritMod.World
{
	public static class SpiritGenPasses
	{
		// Please put all your methods in regions
		// Genpass methods should have their region name be "GENPASS: CAPITALIZED GENPASS NAME"
		// Non-genpass utility methods that are used by genpasses can be named anything else

		#region GENPASS: SPIRIT MICROS
		public static void MicrosPass(GenerationProgress progress)
		{
			bool success = false;
			int attempts = 0;
			while (!success) {
				attempts++;
				if (attempts > 1000) {
					success = true;
					continue;
				}

				progress.Message = "Spirit Mod: Adding Microstructures...";

				if (Main.rand.Next(2) == 0) {
					if (WorldGen.genRand.Next(2) == 0) {
						GenerateGrave();
					}
				}
				else {
					if (WorldGen.genRand.Next(2) == 0) {
						GenerateCampsite();
					}
				}

				if (Main.rand.Next(2) == 0) {
					MyWorld.gennedBandits = new BanditHideout().Generate();
					//GenerateBanditHideout();
					//gennedBandits = true;
				}
				else {
					MyWorld.gennedTower = new GoblinTower().Generate();
					//gennedTower = GenerateTower();
				}

				int num584 = 1;
				if (Main.maxTilesX == 4200) {
					num584 = Main.rand.Next(7, 10);
				}
				else if (Main.maxTilesX == 6400) {
					num584 = Main.rand.Next(12, 14);
				}
				else if (Main.maxTilesX == 8400) {
					num584 = Main.rand.Next(15, 19);
				}

				for (int k = 0; k < num584 - 2; k++) {
					GenerateCrateStash();
				}

				for (int k = 0; k < (num584 / 2 + 1); k++) {
					GenerateCrateStashJungle();
				}

				for (int k = 0; k < (num584 / 3 * 2 + 2); k++) {
					GenerateBismiteCavern();
				}

				if (WorldGen.genRand.Next(2) == 0) {
					for (int k = 0; k < (num584 / 4); k++) {
						GenerateStoneDungeon();
					}
				}

				for (int k = 0; k < Main.rand.Next(5, 7); k++) {
					GenerateGemStash();
				}

				int num8827 = 1;
				if (Main.maxTilesX == 4200) {
					num8827 = 2;
				}
				else if (Main.maxTilesX == 6400) {
					num8827 = 3;
				}
				else if (Main.maxTilesX == 8400) {
					num8827 = 4;
				}

				for (int i = 0; i < num8827; i++) {
					GenerateBoneIsland(num8827, i);
				}

				GeneratePagoda();
				GenerateZiggurat();

				success = true;

			}
		}
		#endregion Spirit Micros

		#region Grave
		private static void GenerateGrave()
		{
			int[,] GraveShape = new int[,]
			{
				{0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0},
				{0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,0,0,2,0,0,2,0,0,2,0,0,2,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0},
			};

			int[,] GraveWalls = new int[,]
			{
				{0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0},
				{0,0,5,5,4,5,5,4,5,5,5,5,5,5,5,5,5,5,0},
				{0,0,5,4,4,5,5,4,4,5,5,5,4,4,5,5,5,5,0},
				{0,0,4,4,2,3,3,3,2,3,3,3,2,2,2,3,3,5,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
			};

			bool placed = false;
			while (!placed) {
				// Select a place in the first 6th of the world
				int towerX = Main.spawnTileX + Main.rand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
																		  // 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}

				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}

				// If we went under the world's surface, try again
				if (towerY > Main.worldSurface) {
					continue;
				}

				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if (tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone) {
					continue;
				}

				// place the tower
				PlaceGrave(towerX, towerY + 2, GraveShape, GraveWalls);

				placed = true;
			}
		}

		private static void PlaceGrave(int i, int j, int[,] ShrineArray, int[,] WallArray)
		{
			for (int y = 0; y < WallArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < WallArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						switch (WallArray[y, x]) {
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.KillWall(k, l);
								break;

							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.KillWall(k, l);
								WorldGen.PlaceWall(k, l, 106);
								break;

							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.KillWall(k, l);
								if (Main.rand.Next(2) == 0) {
									WorldGen.PlaceWall(k, l, 106);
								}
								else {
									WorldGen.PlaceWall(k, l, 63);
								}
								break;

							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.KillWall(k, l); {
									WorldGen.PlaceWall(k, l, 63);
								}
								break;

							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.KillWall(k, l);
								break;
						}
					}
				}
			}

			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						switch (ShrineArray[y, x]) {
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceTile(k, l, 0); // Dirt
								break;

							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceTile(k, l, 0); // Dirt
								break;

							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}

			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						switch (ShrineArray[y, x]) {
							case 1:
								WorldGen.PlaceTile(k, l, 2); // Dirt
								break;

							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceObject(k, l, 85, true, Main.rand.Next(0, 5));
								break;
						}
					}
				}
			}

			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						switch (ShrineArray[y, x]) {
							case 2:
								int num256 = Sign.ReadSign(k, l, true);
								if (num256 >= 0) {
									switch (Main.rand.Next(6)) {
										case 0:
											Sign.TextSign(num256, "The grave of a long-forgotten soul lies here.");
											break;

										case 1:
											Sign.TextSign(num256, "The text is too faded to read.");
											break;

										case 2:
											Sign.TextSign(num256, "");
											break;

										case 3:
											Sign.TextSign(num256, "");
											break;

										case 4:
											Sign.TextSign(num256, "The grave seems to be covered in indecipherable runes.");
											break;

										case 5:
											Sign.TextSign(num256, "The grave lists the burial practices of an ancient civilization.");
											break;

										default:
											Sign.TextSign(num256, "The text is too faded to read.");
											break;
									}
								}
								break;
						}
					}
				}
			}
		}
		#endregion Grave

		#region Campsite
		private static void GenerateCampsite()
		{
			int[,] CampShape1 = new int[,]
			{
				{6,6,6,0,0,0,0,0,0,0,6,6,6,6},
				{6,6,0,0,0,0,0,0,0,0,0,6,6,6},
				{6,0,0,0,0,0,0,0,0,0,0,0,6,6},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,3,0,0,0,0,2,0,0,0,4,0,5,0},
				{1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			};

			bool placed = false;
			while (!placed) {
				// Select a place in the first 6th of the world
				int towerX = Main.spawnTileX + Main.rand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
																		  // 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}

				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}

				// If we went under the world's surface, try again
				if (towerY > Main.worldSurface) {
					continue;
				}

				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if (tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone) {
					continue;
				}

				// place the tower
				PlaceCampsite(towerX, towerY + 1, CampShape1);

				placed = true;
			}
		}

		private static void PlaceCampsite(int i, int j, int[,] BlocksArray)
		{
			for (int y = 0; y < BlocksArray.GetLength(0); y++) {
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);

						switch (BlocksArray[y, x]) {
							case 0:
								tile.ClearTile();
								break;

							case 1:
								tile.ClearTile();
								tile.type = 0;
								tile.active(true);
								break;

							case 2:
								tile.ClearTile();
								break;

							case 3:
								tile.ClearTile();
								break;

							case 4:
								tile.ClearTile();
								break;

							case 5:
								tile.ClearTile();
								break;

							case 6:
								break;
						}
					}
				}
			}

			for (int y = 0; y < BlocksArray.GetLength(0); y++) {
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);

						switch (BlocksArray[y, x]) {
							case 0:
								break;

							case 1:
								WorldGen.PlaceTile(k, l, 2);
								break;

							case 2:
								WorldGen.PlaceObject(k, l, 215, true, 0);
								break;

							case 3:
								WorldGen.PlaceTile(k, l, ModContent.TileType<TentOpposite>());
								break;

							case 4:
								WorldGen.PlaceObject(k, l, 187, true, 26, 1, -1, -1);
								break;

							case 5:
								WorldGen.PlaceTile(k, l, 28);  // Pot
								tile.active(true);
								break;
						}
					}
				}
			}
		}
		#endregion Campsite

		#region Crate Stashes
		private static void GenerateCrateStash()
		{
			bool placed = false;
			while (!placed) {
				int hideoutX = Main.rand.Next(300, Main.maxTilesX - 300); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 450);
				Tile tile = Main.tile[hideoutX, hideoutY];

				if (!tile.active() || tile.type != TileID.Stone) {
					continue;
				}

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();

				StructureLoader.GetStructure("CrateStashRegular").PlaceForce(hideoutX, hideoutY, out containers);

				placed = true;
			}
		}

		private static void GenerateCrateStashJungle()
		{
			bool placed = false;
			while (!placed) {
				int hideoutX = Main.rand.Next(300, Main.maxTilesX - 300); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 450);
				Tile tile = Framing.GetTileSafely(hideoutX, hideoutY);

				if (!tile.active() || tile.type != 60) {
					continue;
				}

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();

				StructureLoader.GetStructure("CrateStashJungle").PlaceForce(hideoutX, hideoutY, out containers);

				placed = true;
			}
		}
		#endregion Crate Stashes

		#region Bismite Cavern
		private static void GenerateBismiteCavern()
		{
			bool placed = false;
			while (!placed) {
				int hideoutX = Main.rand.Next(300, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				Tile tile = Framing.GetTileSafely(hideoutX, hideoutY);

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();

				if (!tile.active() || tile.type != TileID.Stone) {
					continue;
				}

				if (WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("BismiteCavern1").PlaceForce(hideoutX, hideoutY, out containers);
				}
				else if (WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("BismiteCavern2").PlaceForce(hideoutX, hideoutY, out containers);
				}
				else {
					StructureLoader.GetStructure("BismiteCavern3").PlaceForce(hideoutX, hideoutY, out containers);
				}

				placed = true;
			}
		}
		#endregion Bismite Cavern

		#region Stone Dungeon
		private static void GenerateStoneDungeon()
		{
			bool placed = false;
			while (!placed) {
				int hideoutX = Main.rand.Next(50, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY);
				Tile tile = Framing.GetTileSafely(hideoutX, hideoutY);

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();

				if (!tile.active() || tile.type != TileID.Stone) {
					continue;
				}

				if (WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("StoneDungeon1").PlaceForce(hideoutX, hideoutY, out containers);
				}
				else if (WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("StoneDungeon2").PlaceForce(hideoutX, hideoutY, out containers);
				}
				else {
					StructureLoader.GetStructure("StoneDungeon3").PlaceForce(hideoutX, hideoutY, out containers);
				}

				placed = true;
			}
		}
		#endregion Stone Dungeon

		#region Gem Stash
		private static void GenerateGemStash()
		{
			int[,] StashRoomMain = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,2,2,2,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,0,1,1,1,0,0},
				{0,0,1,4,4,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,4,4,4,1,0,0},
				{0,0,1,4,4,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,4,4,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,4,1,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,2,2,2,2,0,0,0},
				{0,0,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,0},
				{0,0,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,0},
				{0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};

			int[,] StashRoomMain1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,2,2,2,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,0,1,1,1,0,0},
				{0,0,1,4,4,0,0,0,0,0,4,4,4,4,4,4,0,0,0,0,0,4,4,4,1,0,0},
				{0,0,1,4,4,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,4,4,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,4,1,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,2,2,2,2,0,0,0},
				{0,0,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,7,0},
				{0,0,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,0},
				{0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,7,7,7,7,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};

			int[,] StashMainWalls = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,2,2,2,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,0,1,1,1,0,0},
				{0,0,1,3,3,3,3,3,3,3,0,3,3,3,3,3,3,3,3,3,3,3,0,3,1,0,0},
				{0,0,1,0,0,3,3,3,3,3,3,0,3,3,3,3,3,3,3,3,3,3,0,3,1,0,0},
				{0,0,1,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,0,0},
				{0,0,0,3,0,3,3,3,3,3,0,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0},
				{0,0,1,3,3,3,3,3,3,0,3,3,3,3,3,3,3,3,3,3,3,0,0,3,1,0,0},
				{0,0,1,0,3,3,3,3,3,3,0,3,3,3,3,3,3,3,3,3,3,0,0,3,1,0,0},
				{0,0,0,3,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};

			int[,] StashMainLoot = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,5,0,5,0,0,0,7,0,0,5,0,0,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			};

			int[,] StashMainLoot1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,4,0,5,0,0,0,0,9,0,0,5,0,0,6,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			};

			int[,] StashRoom1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,1,0,1,1,1,1,2,2,2,2,1,1,1,1,1,1,0,1,0,0,0,0},
				{0,0,0,1,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,1,0,0,0,0},
				{0,0,1,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,1,0,0,0,0},
				{0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,1,0,0,0,0},
				{0,0,0,1,0,0,2,2,2,0,0,0,0,2,2,2,2,2,2,0,0,0,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0},
				{0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,0,1,2,2,2,2,1,1,1,1,1,0,1,0,0,0,0},
			};

			int[,] Stash1Walls = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,2,2,2,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,0,1,1,1,0,0},
				{0,0,1,3,3,3,3,3,3,3,0,3,3,3,3,3,3,3,3,3,3,3,0,3,3,0,0},
				{0,0,1,0,0,3,3,0,0,3,3,3,3,3,3,0,0,0,3,3,3,3,0,3,3,0,0},
				{0,0,1,0,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,3,3,3,3,3,1,0,0},
				{0,0,0,3,0,3,3,3,3,3,0,3,3,3,0,3,3,3,3,3,3,3,3,3,0,0,0},
				{0,0,1,3,0,3,3,3,3,0,3,3,3,3,0,3,3,3,3,3,3,0,0,3,3,0,0},
				{0,0,1,0,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,3,3,0,0,3,3,0,0},
				{0,0,0,3,0,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,0,0,3,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};

			int[,] Stash1Loot = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,5,0,0,8,0,0,0,5,0,5,0,5,0,8,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			};

			bool placed = false;
			while (!placed) {
				int hideoutX = (Main.spawnTileX + Main.rand.Next(-800, 800)); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.spawnTileY + Main.rand.Next(120, 400);

				// place the hideout
				if (WorldGen.genRand.Next(2) == 0) {
					PlaceGemStash(hideoutX, hideoutY, StashRoomMain, StashMainWalls, StashMainLoot);
				}
				else {
					PlaceGemStash(hideoutX, hideoutY, StashRoomMain1, StashMainWalls, StashMainLoot1);
				}

				if (WorldGen.genRand.Next(2) == 0) {
					PlaceGemStash(hideoutX + (Main.rand.Next(-5, 5)), hideoutY - 8, StashRoom1, Stash1Walls, Stash1Loot);
				}

				placed = true;
			}
		}

		private static void PlaceGemStash(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++) {
				for (int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						switch (WallsArray[y, x]) {
							case 0:
								break;

							case 1:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;

							case 2:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;

							case 3:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}

			for (int y = 0; y < BlocksArray.GetLength(0); y++) {
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						switch (BlocksArray[y, x]) {
							case 0:
								break;

							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								break;

							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								break;

							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}

			for (int y = 0; y < BlocksArray.GetLength(0); y++) {
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);

						switch (BlocksArray[y, x]) {
							case 0:
								break;

							case 1:
								WorldGen.PlaceTile(k, l, 30);
								tile.active(true);
								break;

							case 2:
								WorldGen.PlaceTile(k, l, 19);
								tile.active(true);
								break;

							case 3:
								WorldGen.PlaceTile(k, l, 63);
								tile.active(true);
								break;

							case 4:
								WorldGen.PlaceTile(k, l, 51);
								tile.active(true);
								break;

							case 7:
								WorldGen.PlaceTile(k, l, 64);
								tile.active(true);
								break;
						}
					}
				}
			}

			for (int y = 0; y < WallsArray.GetLength(0); y++) {
				for (int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);

						switch (WallsArray[y, x]) {
							case 0:
								break;

							case 3:
								WorldGen.PlaceWall(k, l, 27);
								break;
						}
					}
				}
			}

			for (int y = 0; y < LootArray.GetLength(0); y++) {
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);

						switch (LootArray[y, x]) {
							case 0:
								break;

							case 4:
								WorldGen.PlaceObject(k, l, TileID.FishingCrate);  // Crate
								break;

							case 5:
								WorldGen.PlaceTile(k, l, TileID.Pots);  // Pot
								tile.active(true);
								break;

							case 6:
								int objects;
								if (WorldGen.genRand.NextBool(3)) {
									objects = TileID.Statues;
								}
								else if (WorldGen.genRand.NextBool(2)) {
									objects = TileID.Anvils;
								}
								else if (WorldGen.genRand.NextBool(4)) {
									objects = TileID.Pianos;
								}
								else if (WorldGen.genRand.NextBool(4)) {
									objects = TileID.WorkBenches;
								}
								else {
									objects = TileID.Pots;
								}

								WorldGen.PlaceObject(k, l, (ushort)objects);  // Misc
								break;

							case 7:
								WorldGen.PlaceObject(k, l - 1, ModContent.TileType<GemsPickaxeSapphire>());  // Special Pick		
								break;

							case 8:
								if (WorldGen.genRand.NextBool(3)) {
									objects = TileID.Statues;
								}
								else if (WorldGen.genRand.NextBool(2)) {
									objects = TileID.Anvils;
								}
								else if (WorldGen.genRand.NextBool(4)) {
									objects = TileID.Pianos;
								}
								else if (WorldGen.genRand.NextBool(4)) {
									objects = TileID.WorkBenches;
								}
								else {
									objects = TileID.Pots;
								}

								WorldGen.PlaceObject(k, l, (ushort)objects);  // Another Misc Obj
								break;

							case 9:
								WorldGen.PlaceObject(k, l - 1, ModContent.TileType<GemsPickaxeRuby>());  // Special Pick		
								break;
						}
					}
				}
			}
		}
		#endregion Gem Stash

		#region Bone Island
		private static void GenerateBoneIsland(int islands, int section)
		{
			bool placed = false;
			while (!placed) {
				// Select a place in the first 6th of the world
				int sectionSize = Main.maxTilesX / 3 * 2 / islands;
				int towerX = Main.rand.Next((sectionSize * section) + 50, (sectionSize * (section + 1)) - 50);
				int towerY = WorldGen.genRand.Next(Main.maxTilesY / 9, Main.maxTilesY / 8);

				Tile tile = Main.tile[towerX, towerY];
				if (tile.active()) {
					continue;
				}

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();

				if (WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("BoneIsland").PlaceForce(towerX, towerY, out containers);
				}
				else {
					StructureLoader.GetStructure("BoneIsland1").PlaceForce(towerX, towerY, out containers);
				}

				placed = true;
			}
		}
		#endregion Bone Island

		#region Pagoda
		private static void GeneratePagoda()
		{
			bool placed = false;
			MyWorld.pagodaX = 0;

			while (!placed) {
				if (MyWorld.asteroidSide == 0) {
					MyWorld.pagodaX = Main.maxTilesX - Main.rand.Next(200, 350);
				}
				else {
					MyWorld.pagodaX = Main.rand.Next(200, 350);
				}

				MyWorld.pagodaY = (int)(Main.worldSurface / 5.0);
				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				StructureLoader.GetStructure("Pagoda").PlaceForce(MyWorld.pagodaX, MyWorld.pagodaY, out containers);

				//foreach incase we decide to add a second chest.
				foreach (Point chestLocation in containers) {
					for (int x = 0; x < 2; x++) {
						for (int y = 0; y < 2; y++) {
							Main.tile[chestLocation.X + x, chestLocation.Y + y].active(false);
							Main.tile[chestLocation.X + x, chestLocation.Y + y].type = 0;
						}
					}
					WorldGen.PlaceChest(chestLocation.X, chestLocation.Y + 1, 21, true, 28);
				}

				placed = true;
			}
		}
		#endregion Pagoda

		#region Ziggurat
		private static void GenerateZiggurat()
		{
			int[,] ZigguratShape = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,3,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,3,3,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,3,3,3,3,3,3,3,3,3,3,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
			};

			int[,] ZigguratWalls = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,3,3,3,3,3,3,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,3,3,3,3,3,3,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,3,3,3,3,3,3,3,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			};

			int[,] ZigguratLoot = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,3,3,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,5,3,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,6,3,3,3,3,3,3,3,3,6,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,3,3,3,3,4,3,3,3,3,3,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,6,3,3,3,3,3,3,3,3,3,3,6,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,3,3,7,3,7,3,3,8,3,3,3,7,3,7,3,7,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
			};

			bool placed = false;
			while (!placed) {
				// Select a place in the first 6th of the world
				int hideoutX = Main.rand.Next(Main.maxTilesX / 6, Main.maxTilesX / 6 * 5); // from 50 since there's a unaccessible area at the world's borders
																						   // 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool()) {
					hideoutX = Main.maxTilesX - hideoutX;
				}

				int hideoutY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(hideoutX, hideoutY) && hideoutY <= Main.worldSurface) {
					hideoutY++;
				}

				// If we went under the world's surface, try again
				if (hideoutY > Main.worldSurface) {
					continue;
				}

				Tile tile = Main.tile[hideoutX, hideoutY];
				// If the type of the tile we are placing the hideout on doesn't match what we want, try again
				if (tile.type != TileID.Sand && tile.type != TileID.Ebonsand && tile.type != TileID.Crimsand && tile.type != TileID.Sandstone) {
					continue;
				}

				// place the hideout
				PlaceZiggurat(hideoutX, hideoutY - 1, ZigguratShape, ZigguratWalls, ZigguratLoot);
				placed = true;
			}
		}

		private static void PlaceZiggurat(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for (int y = 0; y < BlocksArray.GetLength(0); y++) {
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 2:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 3:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}

			for (int y = 0; y < WallsArray.GetLength(0); y++) {
				for (int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 2:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 3:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}

			for (int y = 0; y < BlocksArray.GetLength(0); y++) {
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, 151);
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceTile(k, l, 152);
								tile.active(true);
								break;
						}
					}
				}
			}

			for (int y = 0; y < WallsArray.GetLength(0); y++) {
				for (int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceWall(k, l, 34);
								break;
							case 2:
								WorldGen.PlaceWall(k, l, 35);
								break;
							case 3:
								WorldGen.PlaceWall(k, l, 34);
								break;
						}
					}
				}
			}

			for (int y = 0; y < LootArray.GetLength(0); y++) {
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 0:
								break;
							case 4:
								WorldGen.PlaceObject(k, l, ModContent.TileType<Tiles.Ambient.ScarabIdol>());
								break;
							case 5:
								WorldGen.PlaceChest(k, l, (ushort)ModContent.TileType<GoldScarabChest>(), false, 0);
								break;
							case 6:
								WorldGen.PlaceObject(k, l, 91);
								break;
							case 7:
								WorldGen.PlaceTile(k, l, 28);
								break;
							case 8:
								WorldGen.PlaceTile(k, l, 102);
								break;
						}
					}
				}
			}
		}
		#endregion Ziggurat

		#region GENPASS: ASTEROIDS
		public static void AsteroidsPass(GenerationProgress progress)
		{
			bool success = false;
			int attempts = 0;
			while (!success) {
				attempts++;
				if (attempts > 1000) {
					success = true;
					continue;
				}
				progress.Message = "Creating an asteroid belt";
				{
					int width = 350;
					int height = 75;
					int x = 0;
					int y = 0;
					if (Main.maxTilesX == 4200) {
						width = 200;
						height = 40;
					}
					if (Main.maxTilesX == 6400) {
						width = 275;
						height = 60;
					}
					if (Main.maxTilesX == 8400) {
						width = 350;
						height = 75;
					}

					if (Main.rand.Next(2) == 0) //change to check for dungeon later, idk how rn.
					{
						MyWorld.asteroidSide = 0;
						x = width + 80;
					}
					else {
						MyWorld.asteroidSide = 1;
						x = Main.maxTilesX - (width + 80);
					}

					y = height + 60;
					PlaceAsteroids(x, y);

				}
				success = true;
			}
		}
		#endregion GENPASS: ASTEROIDS

		#region Asteroid Methods
		private static void PlaceAsteroids(int i, int j)
		{
			bool success = false;
			int attempts = 0;
			while (!success) {
				attempts++;
				if (attempts > 1000) {
					success = true;
					continue;
				}
				int basex = i;
				int basey = j;
				int x = basex;
				int y = basey;

				int numberOfAsteroids = 110;
				int numJunkPiles = 1;
				int numberOfBigs = 4;
				int numberOfOres = 310;
				int width = 350;
				int height = 75;

				if (Main.maxTilesX == 4200) {
					numberOfAsteroids = 33;
					numberOfBigs = 1;
					numberOfOres = 140;
					numJunkPiles = 15;
					width = 200;
					height = 40;
				}

				if (Main.maxTilesX == 6400) {
					numberOfAsteroids = 50;
					numberOfBigs = 2;
					numberOfOres = 220;
					width = 275;
					numJunkPiles = 21;
					height = 60;
				}

				if (Main.maxTilesX == 8400) {
					numberOfAsteroids = 79;
					numberOfBigs = 4;
					numberOfOres = 300;
					width = 350;
					numJunkPiles = 32;
					height = 75;
				}

				int radius = (int)Math.Sqrt((width * width) + (height * height));

				for (int b = 0; b < numberOfAsteroids; b++) //small asteroids
				{
					float distance = (int)(((float)(Main.rand.Next(1000)) / 1000) * (float)Main.rand.Next(radius));
					int angle = Main.rand.Next(360);
					float xsize = (float)(Main.rand.Next(100, 120)) / 100;
					float ysize = (float)(Main.rand.Next(100, 120)) / 100;
					int size = Main.rand.Next(6, 7);
					x = basex + (int)(Main.rand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + Main.rand.Next(-100, 100);
					y = basey + (int)(Main.rand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + Main.rand.Next(-10, 15);
					PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<Asteroid>(), 50, true, ModContent.WallType<AsteroidWall>());
				}

				for (int b = 0; b < numJunkPiles; b++) //junkPiles
				{
					float distance = (int)(((float)(Main.rand.Next(1000)) / 1000) * (float)Main.rand.Next(radius));
					int angle = Main.rand.Next(360);
					float xsize = (float)(Main.rand.Next(100, 120)) / 100;
					float ysize = (float)(Main.rand.Next(100, 120)) / 100;
					int size = Main.rand.Next(3, 4);
					x = basex + (int)(Main.rand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + Main.rand.Next(-100, 100);
					y = basey + (int)(Main.rand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + Main.rand.Next(-10, 15);
					PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<SpaceJunkTile>(), 50);
				}

				for (int b = 0; b < numberOfBigs; b++) //big asteroids
				{
					x = basex + (int)(Main.rand.Next(0 - width, width) / 1.5f);
					y = basey + Main.rand.Next(0 - height, height);
					float xsize = (float)(Main.rand.Next(75, 133)) / 100;
					float ysize = (float)(Main.rand.Next(75, 133)) / 100;
					int size = Main.rand.Next(11, 17);
					PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<BigAsteroid>(), 10, true, ModContent.WallType<AsteroidWall>());
				}

				for (int b = 0; b < numberOfOres; b++) //ores
				{
					float distance = (int)(((float)(Main.rand.Next(1000)) / 1000) * (float)Main.rand.Next(radius));
					int angle = Main.rand.Next(360);
					int size = Main.rand.Next(2, 5);
					x = basex + (int)(Main.rand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + Main.rand.Next(-100, 100);
					y = basey + (int)(Main.rand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + Main.rand.Next(-10, 15);
					ushort ore = OreRoller((ushort)ModContent.TileType<Glowstone>(), (ushort)ModContent.TileType<Glowstone>());
					WorldGen.TileRunner(x, y, Main.rand.Next(2, 10), 2, ore, false, 0f, 0f, false, true);
				}

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				StructureLoader.GetStructure("StarAltar").PlaceForce(basex + (int)(Main.rand.Next(0 - width, width) / 1.5f), basey + Main.rand.Next(0 - height, height), out containers);

				//chest spawning
				int CmaxTries = 20000;
				int Ctries = 0;
				int Csuccesses = 0;

				while (Ctries < CmaxTries && Csuccesses < 4) {
					x = i + WorldGen.genRand.Next(0 - width, width);
					y = j + WorldGen.genRand.Next(0 - height, height);
					if (WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<Tiles.Furniture.SpaceJunk.AsteroidChest>(), false, 0) != -1) {
						Csuccesses++;
					}

					Ctries++;
				}

				success = true;
			}

		}

		private static void PlaceBlob(int x, int y, float xsize, float ysize, int size, int type, int roundness, bool placewall = false, int walltype = 0)
		{
			int distance = size;

			for (int i = 0; i < 360; i++) {
				if (360 - i <= Math.Abs(size - distance) / Math.Sqrt(size) * 50) {
					if (size > distance) {
						distance++;
					}
					else {
						distance--;
					}
				}
				else {
					int increase = Main.rand.Next(roundness);
					if (increase == 0 && distance > 3) {
						distance--;
					}
					if (increase == 1) {
						distance++;
					}
				}

				int offsetX = (int)(Math.Sin(i * (Math.PI / 180)) * distance * xsize);
				int offsetY = (int)(Math.Cos(i * (Math.PI / 180)) * distance * ysize);
				WorldExtras.PlaceLine(x, y, x + offsetX, y + offsetY, type, placewall, walltype);
			}
		}

		private static ushort OreRoller(ushort glowstone, ushort none)
		{
			ushort iron = WorldExtras.GetOreCounterpart(WorldGen.IronTierOre);
			ushort silver = WorldExtras.GetOreCounterpart(WorldGen.SilverTierOre);
			ushort gold = WorldExtras.GetOreCounterpart(WorldGen.GoldTierOre);

			int OreRoll = Main.rand.Next(1120);
			if (OreRoll < 250) {
				return WorldExtras.GetOreCounterpart(iron);
			}
			else if (OreRoll < 400) {
				return WorldExtras.GetOreCounterpart(silver);
			}
			else if (OreRoll < 600) {
				return WorldExtras.GetOreCounterpart(gold);
			}
			else if (OreRoll < 650) {
				return 37; //meteorite
			}
			else {
				return glowstone;
			}
		}
		#endregion Asteroids

		#region GENPASS: PILES/AMBIENT
		public static void PilesPass(GenerationProgress progress)
		{
			progress.Message = "Spirit Mod: Adding Ambient Objects...";

			if (WorldGen.CopperTierOre == TileID.Copper) {
				for (int i = 0; i < Main.maxTilesX * 22.5; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<CopperPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<CopperPile>(), 0, 0, -1, -1);
					}
				}
			}
			else if (WorldGen.CopperTierOre == TileID.Tin) {
				for (int i = 0; i < Main.maxTilesX * 22.5; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<TinPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<TinPile>(), 0, 0, -1, -1);
					}
				}
			}

			if (WorldGen.IronTierOre == TileID.Iron) {
				for (int i = 0; i < Main.maxTilesX * 15; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<IronPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<IronPile>(), 0, 0, -1, -1);
					}
				}
			}
			else if (WorldGen.IronTierOre == TileID.Lead) {
				for (int i = 0; i < Main.maxTilesX * 15; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<LeadPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<LeadPile>(), 0, 0, -1, -1);
					}
				}
			}

			if (WorldGen.SilverTierOre == TileID.Silver) {
				for (int i = 0; i < Main.maxTilesX * 11.75f; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<SilverPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<SilverPile>(), 0, 0, -1, -1);
					}
				}
			}
			else if (WorldGen.SilverTierOre == TileID.Tungsten) {
				for (int i = 0; i < Main.maxTilesX * 11.75f; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<TungstenPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<TungstenPile>(), 0, 0, -1, -1);
					}
				}
			}

			if (WorldGen.GoldTierOre == TileID.Gold) {
				for (int i = 0; i < Main.maxTilesX * 7.5f; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<GoldPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<GoldPile>(), 0, 0, -1, -1);
					}
				}
			}
			else if (WorldGen.GoldTierOre == TileID.Platinum) {
				for (int i = 0; i < Main.maxTilesX * 7.5f; i++) {
					int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					Tile tile = Main.tile[num3, num4];
					if (tile.type == TileID.Stone && tile.active()) {
						WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<PlatinumPile>());
						NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<PlatinumPile>(), 0, 0, -1, -1);
					}
				}
			}

			for (int C = 0; C < Main.maxTilesX * 10; C++) {
				int X = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
				int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 200);
				if (Main.tile[X, Y].type == TileID.Stone) {
					WorldGen.PlaceObject(X, Y, ModContent.TileType<ExplosiveBarrelTile>());
					NetMessage.SendObjectPlacment(-1, X, Y, ModContent.TileType<ExplosiveBarrelTile>(), 0, 0, -1, -1);
				}
			}

			for (int C = 0; C < Main.maxTilesX * 45; C++) {
				int[] sculptures = new int[] { ModContent.TileType<IceWheezerPassive>(), ModContent.TileType<IceFlinxPassive>(), ModContent.TileType<IceBatPassive>(), ModContent.TileType<IceVikingPassive>(), ModContent.TileType<IceWheezerHostile>(), ModContent.TileType<IceFlinxHostile>(), ModContent.TileType<IceBatHostile>(), ModContent.TileType<IceVikingHostile>() };
				{
					int X = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if ((Main.tile[X, Y].type == TileID.IceBlock || Main.tile[X, Y].type == TileID.SnowBlock) && Main.tile[X, Y + 1].type != 162) {
						WorldGen.PlaceObject(X, Y, (ushort)sculptures[Main.rand.Next(8)]);
						NetMessage.SendObjectPlacment(-1, X, Y, (ushort)sculptures[Main.rand.Next(4)], 0, 0, -1, -1);
					}
				}
			}

			for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 78) * 15E-05); k++) {
				int EEXX = WorldGen.genRand.Next(0, Main.maxTilesX);
				int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
				if (Main.tile[EEXX, WHHYY] != null) {
					if (Main.tile[EEXX, WHHYY].active()) {
						if (Main.tile[EEXX, WHHYY].type == 368) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)ModContent.TileType<GraniteOre>());
						}
						if (Main.tile[EEXX, WHHYY].type == 367) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(4, 9), (ushort)ModContent.TileType<MarbleOre>());
						}
					}
				}
			}
			
			for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 16.2f) * 6E-03); k++) {
				{
					int X = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
					int Y = WorldGen.genRand.Next(0, Main.maxTilesY);
					if ((Main.tile[X, Y].type == ModContent.TileType<Asteroid>() || Main.tile[X, Y].type == ModContent.TileType<BigAsteroid>())) {
						WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<BlueShardBig>());
						NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<BlueShardBig>(), 0, 0, -1, -1);
					}
				}
			}
			if (WorldGen.genRand.NextBool(2)) {
				for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 7.2f) * 6E-03); k++) {
					{
						int X = WorldGen.genRand.Next(100, Main.maxTilesX - 20);

						int Y = WorldGen.genRand.Next((int)Main.worldSurface - 100, (int)Main.worldSurface + 30);
						if ((Main.tile[X, Y].type == TileID.SnowBlock || Main.tile[X, Y].type == TileID.IceBlock)) {
							if (Main.rand.Next(3) == 0) {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<SnowBush3>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<SnowBush3>(), 0, 0, -1, -1);
							}
							else if (Main.rand.Next(2) == 0) {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<SnowBush2>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<SnowBush2>(), 0, 0, -1, -1);
							}
							else {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<SnowBush1>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<SnowBush1>(), 0, 0, -1, -1);
							}
						}
					}
				}
			}
			else {
				for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 7.2f) * 6E-03); k++) {
					{
						int X = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
						int Y = WorldGen.genRand.Next((int)Main.worldSurface - 100, (int)Main.worldSurface + 30);
						if ((Main.tile[X, Y].type == TileID.SnowBlock || Main.tile[X, Y].type == TileID.IceBlock)) {
							if (Main.rand.Next(2) == 0) {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<IceCube3>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<IceCube3>(), 0, 0, -1, -1);
							}
							else if (Main.rand.Next(2) == 0) {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<IceCube2>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<IceCube2>(), 0, 0, -1, -1);
							}
							else {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<IceCube1>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<IceCube1>(), 0, 0, -1, -1);
							}
						}
					}
				}
			}
		}
		#endregion GENPASS: Piles/Ambient
	}
}
