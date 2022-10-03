using Microsoft.Xna.Framework;
using SpiritMod.Items.BossLoot.StarplateDrops;
using SpiritMod.Items.Sets.FloranSet;
using SpiritMod.Items.Sets.GraniteSet;
using SpiritMod.Items.Sets.MarbleSet;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.IceSculpture.Hostile;
using SpiritMod.Tiles.Ambient.SpaceCrystals;
using SpiritMod.Tiles.Ambient.SurfaceIce;
using SpiritMod.Tiles.Ambient.Underground;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Furniture;
using SpiritMod.Tiles.Piles;
using SpiritMod.Tiles.Walls.Natural;
using SpiritMod.Utilities;
using SpiritMod.World.Micropasses;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace SpiritMod.World
{
	public static class SpiritGenPasses
	{
		// Please organize fields with the proper comment, i.e.
		// type field = value; //Allows y to do x

		private static List<DecorSpamData> decorSpam = new List<DecorSpamData>(); //For ambient decor spam genpass merging

		// Please put all your methods in regions
		// Genpass methods should have their region name be "GENPASS: CAPITALIZED GENPASS NAME"
		// Non-genpass utility methods that are used by genpasses can be named anything else

		#region GENPASS: SPIRIT MICROS
		public static void MicrosPass(GenerationProgress progress, GameConfiguration config)
		{
			progress.Message = "Spirit Mod: Microstructures: Boulders";
			BoulderMicropass.Run();

			progress.Message = "Spirit Mod: Microstructures: Waterfalls";
			WaterfallMicropass.Run();

			progress.Message = "Spirit Mod: Microstructures: Campsite";
			if (WorldGen.genRand.NextBool(4))
				GenerateCampsite();

			progress.Message = "Spirit Mod: Microstructures: Sunken Surfaces";
			SunkenSurfaceMicropass.Run();

			progress.Message = "Spirit Mod: Microstructures: Stargrass";
			StargrassMicropass.Run();

			progress.Message = "Spirit Mod: Microstructures: Hideouts";
			if (ModContent.GetInstance<SpiritClientConfig>().DoubleHideoutGeneration)
			{
				new BanditHideout().Generate();
				new GoblinTower().Generate();
				MyWorld.gennedBandits = true;
				MyWorld.gennedTower = true;
			}
			else
			{
				if (WorldGen.genRand.NextBool(2))
				{
					new BanditHideout().Generate();
					MyWorld.gennedBandits = true;
				}
				else
				{
					new GoblinTower().Generate();
					MyWorld.gennedTower = true;
				}
			}

			progress.Message = "Spirit Mod: Microstructures: Stashes, Caverns and Dungeons";

			int siz = (int)((Main.maxTilesX / 4200f) * 7);
			int repeats = WorldGen.genRand.Next(siz, siz + 4);

			for (int k = 0; k < repeats - 2; k++)
				GenerateCrateStash();

			for (int k = 0; k < (repeats / 2 + 1); k++)
				GenerateCrateStashJungle();

			for (int k = 0; k < (repeats + 2); k++)
				GenerateBismiteCavern();

			if (WorldGen.genRand.NextBool(2))
				for (int k = 0; k < (repeats / 4); k++)
					GenerateStoneDungeon();

			for (int k = 0; k < WorldGen.genRand.Next(5, 7); k++)
				GenerateGemStash();

			progress.Message = "Spirit Mod: Microstructures: Avian Islands";

			List<int> takenIslands = new List<int>();
			for (int i = 0; i < Main.maxTilesX / 4200f * 2f; i++)
				GenerateBoneIsland(takenIslands); //2 islands in a small world

			progress.Message = "Spirit Mod: Microstructures: Pagoda and Ziggurat";

			GeneratePagoda();
			GenerateZiggurat();
		}
		#endregion Spirit Micros

		private static void BlacklistBriarBlocks(int maxTries, int width, int height, Rectangle randomizationRange, ref Point tileCheckPos, out bool failed)
		{
			int tries = 0;
			failed = false;

			int[] TileBlacklist = GlobalExtensions.TileSet<BriarGrass, FloranOreTile, BlastStone>();
			int[] WallBlacklist = new int[]
			{
				ModContent.WallType<ReachWallNatural>(),
				ModContent.WallType<ReachStoneWall>()
			};

			do
			{
				tileCheckPos.X = WorldGen.genRand.Next(randomizationRange.X, randomizationRange.X + randomizationRange.Width);
				tileCheckPos.Y = WorldGen.genRand.Next(randomizationRange.Y, randomizationRange.Y + randomizationRange.Height);

				int xDist = width; //Increased due to chance at second floor
				int yDist = height; //Increased due to chance at second floor
				int xCenter = tileCheckPos.X;
				int yCenter = tileCheckPos.Y; //Shifted up due to chance at second floor

				bool blackListedTile = false;
				for (int i = -xDist / 2; i <= xDist / 2; i++)
				{
					for (int j = -yDist / 2; j <= yDist / 2; j++)
					{
						Tile tile = Framing.GetTileSafely(xCenter + i, yCenter + j);
						if (TileBlacklist.Contains(tile.TileType) || WallBlacklist.Contains(tile.WallType))
						{
							blackListedTile = true;
							break;
						}
					}
					if (blackListedTile)
						break;
				}

				if (blackListedTile) //Keep going until blacklisted tiles are not hit
				{
					tries++;
					if (tries >= maxTries) //If it tries too many times, break and cancel the generation, as to not softlock forever
					{
						failed = true;
						break;
					}
					continue;
				}

				break;
			} while (true);
		}

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

			while (true)
			{
				// Select a place in the first 6th of the world
				int fireX = Main.spawnTileX + WorldGen.genRand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
																		  // 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool())
					fireX = Main.maxTilesX - fireX;

				int fireY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(fireX, fireY) && fireY <= Main.worldSurface)
					fireY++;

				// If we went under the world's surface, try again
				if (fireY > Main.worldSurface)
					continue;

				Tile tile = Main.tile[fireX, fireY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if (tile.TileType != TileID.Dirt && tile.TileType != TileID.Grass && tile.TileType != TileID.Stone)
					continue;

				// place the tower
				PlaceCampsite(fireX, fireY + 1, CampShape1);
				break;
			}
		}

		private static void PlaceCampsite(int i, int j, int[,] BlocksArray)
		{
			for (int y = 0; y < BlocksArray.GetLength(0); y++)
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						switch (BlocksArray[y, x])
						{
							case 0:
								tile.ClearTile();
								break;
							case 1:
								tile.ClearTile();
								tile.TileType = 0;
								tile.HasTile = true;
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

			for (int y = 0; y < BlocksArray.GetLength(0); y++)
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						switch (BlocksArray[y, x])
						{
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
								tile.HasTile = true;
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
			while (true)
			{
				int hideoutX = WorldGen.genRand.Next(300, Main.maxTilesX - 300); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 450);
				Tile tile = Main.tile[hideoutX, hideoutY];

				if (!tile.HasTile || tile.TileType != TileID.Stone)
					continue;

				StructureHelper.Generator.GenerateStructure("Structures/CrateStashRegular", new Point16(hideoutX, hideoutY), SpiritMod.Instance);
				break;
			}
		}

		private static void GenerateCrateStashJungle()
		{
			while (true)
			{
				int hideoutX = WorldGen.genRand.Next(300, Main.maxTilesX - 300); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 450);
				Tile tile = Framing.GetTileSafely(hideoutX, hideoutY);

				if (!tile.HasTile || tile.TileType != 60)
					continue;

				StructureHelper.Generator.GenerateStructure("Structures/CrateStashJungle", new Point16(hideoutX, hideoutY), SpiritMod.Instance);
				break;
			}
		}
		#endregion Crate Stashes

		#region Bismite Cavern
		private static void GenerateBismiteCavern()
		{
			while (true)
			{
				int hideoutX = WorldGen.genRand.Next(300, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				Tile tile = Framing.GetTileSafely(hideoutX, hideoutY);

				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();

				if (!tile.HasTile || tile.TileType != TileID.Stone)
					continue;

				//if (WorldGen.genRand.NextBool(2)) //STRUCTURES
				//	StructureLoader.GetStructure("BismiteCavern1").PlaceForce(hideoutX, hideoutY, out containers);
				//else if (WorldGen.genRand.NextBool(2))
				//	StructureLoader.GetStructure("BismiteCavern2").PlaceForce(hideoutX, hideoutY, out containers);
				//else
				//	StructureLoader.GetStructure("BismiteCavern3").PlaceForce(hideoutX, hideoutY, out containers);

				break;
			}
		}
		#endregion Bismite Cavern

		#region Stone Dungeon
		private static void GenerateStoneDungeon()
		{
			while (true)
			{
				int hideoutX = WorldGen.genRand.Next(50, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY);
				Tile tile = Framing.GetTileSafely(hideoutX, hideoutY);

				if (!tile.HasTile || tile.TileType != TileID.Stone)
					continue;

				StructureHelper.Generator.GenerateStructure("Structures/StoneDungeon", new Point16(hideoutX, hideoutY), SpiritMod.Instance);
				break;
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

			Point center = new Point();
			int width = (StashRoomMain.GetLength(1) / 2) + 5;
			int height = (StashRoomMain.GetLength(0) / 2) + 4;
			Rectangle positionRange = new Rectangle(Main.spawnTileX - 800, Main.spawnTileY + 120, 1600, 280);
			BlacklistBriarBlocks(80, width, height, positionRange, ref center, out bool failed);


			if (failed) //Dont generate if tried too many times
				return;

			// place the hideout
			if (WorldGen.genRand.NextBool(2))
				PlaceGemStash(center.X, center.Y, StashRoomMain, StashMainWalls, StashMainLoot);
			else
				PlaceGemStash(center.X, center.Y, StashRoomMain1, StashMainWalls, StashMainLoot1);

			if (WorldGen.genRand.NextBool(2))
				PlaceGemStash(center.X + (WorldGen.genRand.Next(-5, 5)), center.Y - 8, StashRoom1, Stash1Walls, Stash1Loot);
		}

		private static void PlaceGemStash(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++)
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						switch (WallsArray[y, x])
						{
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

			for (int y = 0; y < BlocksArray.GetLength(0); y++)
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						switch (BlocksArray[y, x])
						{
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

			for (int y = 0; y < BlocksArray.GetLength(0); y++)
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						switch (BlocksArray[y, x])
						{
							case 0:
								break;

							case 1:
								WorldGen.PlaceTile(k, l, 30);
								tile.HasTile = true;
								break;

							case 2:
								WorldGen.PlaceTile(k, l, 19);
								tile.HasTile = true;
								break;

							case 3:
								WorldGen.PlaceTile(k, l, 63);
								tile.HasTile = true;
								break;

							case 4:
								WorldGen.PlaceTile(k, l, 51);
								tile.HasTile = true;
								break;

							case 7:
								WorldGen.PlaceTile(k, l, 64);
								tile.HasTile = true;
								break;
						}
					}
				}
			}

			for (int y = 0; y < WallsArray.GetLength(0); y++)
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						switch (WallsArray[y, x])
						{
							case 0:
								break;

							case 3:
								WorldGen.PlaceWall(k, l, 27);
								break;
						}
					}
				}
			}

			for (int y = 0; y < LootArray.GetLength(0); y++)
			{
				for (int x = 0; x < LootArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;

					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						switch (LootArray[y, x])
						{
							case 0:
								break;

							case 4:
								WorldGen.PlaceObject(k, l, TileID.FishingCrate);  // Crate
								break;

							case 5:
								WorldGen.PlaceTile(k, l, TileID.Pots);  // Pot
								tile.HasTile = true;
								break;

							case 6:
								int objects;
								if (WorldGen.genRand.NextBool(3))
								{
									objects = TileID.Statues;
								}
								else if (WorldGen.genRand.NextBool(2))
								{
									objects = TileID.Anvils;
								}
								else if (WorldGen.genRand.NextBool(4))
								{
									objects = TileID.Pianos;
								}
								else if (WorldGen.genRand.NextBool(4))
								{
									objects = TileID.WorkBenches;
								}
								else
								{
									objects = TileID.Pots;
								}

								WorldGen.PlaceObject(k, l, (ushort)objects);  // Misc
								break;

							case 7:
								WorldGen.PlaceObject(k, l - 1, ModContent.TileType<GemsPickaxeSapphire>());  // Special Pick		
								break;

							case 8:
								if (WorldGen.genRand.NextBool(3))
								{
									objects = TileID.Statues;
								}
								else if (WorldGen.genRand.NextBool(2))
								{
									objects = TileID.Anvils;
								}
								else if (WorldGen.genRand.NextBool(4))
								{
									objects = TileID.Pianos;
								}
								else if (WorldGen.genRand.NextBool(4))
								{
									objects = TileID.WorkBenches;
								}
								else
								{
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
		private static void GenerateBoneIsland(List<int> takenIslands)
		{
			while (true)
			{
				// Select a place in the first 6th of the world
				Point pos = FindBoneIslandPlacement(takenIslands);

				StructureHelper.Generator.GenerateStructure("Structures/BoneIsland", new Point16(pos.X, pos.Y), SpiritMod.Instance);
				break;
			}
		}

		private static Point FindBoneIslandPlacement(List<int> takenIslands)
		{
			int totalAttempts = -1;

			while (true)
			{
				totalAttempts++;

				if (totalAttempts > 3000)
					break;

				int houseMax = Array.IndexOf(WorldGen.floatingIslandHouseX, WorldGen.floatingIslandHouseX.First(x => x == 0));
				int house = WorldGen.genRand.Next(houseMax);

				while (takenIslands.Contains(house))
					house = WorldGen.genRand.Next(houseMax);

				Point pos = new Point(WorldGen.floatingIslandHouseX[house], WorldGen.floatingIslandHouseY[house]);

				while (true)
				{
					int xOffset = WorldGen.genRand.NextBool() ? WorldGen.genRand.Next(40, 60) : -WorldGen.genRand.Next(60, 80);
					Point realPos = new Point(pos.X + xOffset, pos.Y + WorldGen.genRand.Next(15, 25));
					bool failed = false;

					for (int i = 0; i < 30; ++i)
					{
						if (failed)
							break;

						for (int j = 0; j < 20; ++j)
						{
							Tile tile = Framing.GetTileSafely(realPos.X + i, realPos.Y + j);
							if (tile.HasTile)
							{
								failed = true; //Retry if this space is taken up
								break;
							}
						}
					}

					if (failed)
						continue;

					takenIslands.Add(house);
					return realPos;
				}
			}
			return new Point(0, 0);
		}
		#endregion Bone Island

		#region Pagoda
		private static void GeneratePagoda()
		{
			if (MyWorld.asteroidSide == 0)
				MyWorld.pagodaX = Main.maxTilesX - WorldGen.genRand.Next(200, 350);
			else
				MyWorld.pagodaX = WorldGen.genRand.Next(200, 350);

			MyWorld.pagodaY = (int)(Main.worldSurface / 5.0);
			StructureHelper.Generator.GenerateStructure("Structures/Pagoda", new Point16(MyWorld.pagodaX, MyWorld.pagodaY), SpiritMod.Instance);
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
			while (!placed)
			{
				// Select a place in the first 6th of the world
				int hideoutX = WorldGen.genRand.Next(Main.maxTilesX / 6, Main.maxTilesX / 6 * 5); // from 50 since there's a unaccessible area at the world's borders
																						   // 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool())
				{
					hideoutX = Main.maxTilesX - hideoutX;
				}

				int hideoutY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(hideoutX, hideoutY) && hideoutY <= Main.worldSurface)
				{
					hideoutY++;
				}

				// If we went under the world's surface, try again
				if (hideoutY > Main.worldSurface)
				{
					continue;
				}

				Tile tile = Main.tile[hideoutX, hideoutY];
				// If the type of the tile we are placing the hideout on doesn't match what we want, try again
				if (tile.TileType != TileID.Sand && tile.TileType != TileID.Ebonsand && tile.TileType != TileID.Crimsand && tile.TileType != TileID.Sandstone)
				{
					continue;
				}

				// place the hideout
				PlaceZiggurat(hideoutX, hideoutY - 1, ZigguratShape, ZigguratWalls, ZigguratLoot);
				placed = true;
			}
		}

		private static void PlaceZiggurat(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for (int y = 0; y < BlocksArray.GetLength(0); y++)
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x])
						{
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

			for (int y = 0; y < WallsArray.GetLength(0); y++)
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x])
						{
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

			for (int y = 0; y < BlocksArray.GetLength(0); y++)
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x])
						{
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, 151);
								tile.HasTile = true;
								break;
							case 2:
								WorldGen.PlaceTile(k, l, 152);
								tile.HasTile = true;
								break;
						}
					}
				}
			}

			for (int y = 0; y < WallsArray.GetLength(0); y++)
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x])
						{
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

			for (int y = 0; y < LootArray.GetLength(0); y++)
			{
				for (int x = 0; x < LootArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x])
						{
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
		public static void AsteroidsPass(GenerationProgress progress, GameConfiguration config)
		{
			progress.Message = "Creating an asteroid belt";
			int width = 200 + (int)(((Main.maxTilesX / 4200f) - 1) * 75); //Automatically scales based on world size
			int height = 50 + (int)(((Main.maxTilesX / 4200f) - 1) * 15);
			int x = width + 80;
			MyWorld.asteroidSide = 0;

			if (WorldGen.genRand.NextBool())
			{
				x = Main.maxTilesX - (width + 80);
				MyWorld.asteroidSide = 1;
			}

			int y = height + WorldGen.genRand.Next(36, 50); //If you want to change the top of the asteroid biome, change this
			PlaceAsteroids(x, y, width, height);
		}
		#endregion GENPASS: ASTEROIDS

		#region Asteroid Methods
		private static void PlaceAsteroids(int i, int j, int width, int height)
		{
			int numberOfAsteroids = 33 + (int)(((Main.maxTilesX / 4200f) - 1) * 20); //easy world size scaling woo
			int numJunkPiles = 15 + (int)(((Main.maxTilesX / 4200f) - 1) * 8);
			int numberOfOres = 140 + (int)(((Main.maxTilesX / 4200f) - 1) * 80);
			int numberOfBigs = 1;

			if (Main.maxTilesX == 6400) //didn't want to redo this since it seems important, but I did fix it for XL worlds
				numberOfBigs = 2;
			else if (Main.maxTilesX >= 8400)
				numberOfBigs = 4;

			for (int k = 0; k < numberOfAsteroids; k++) //small asteroids
			{
				int angle = WorldGen.genRand.Next(360);
				float xsize = (float)(WorldGen.genRand.Next(100, 120)) / 100;
				float ysize = (float)(WorldGen.genRand.Next(100, 120)) / 100;
				int size = WorldGen.genRand.Next(6, 7);
				int x = i + (int)(WorldGen.genRand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + WorldGen.genRand.Next(-100, 100);
				int y = j + (int)(WorldGen.genRand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + WorldGen.genRand.Next(-10, 15);
				PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<Asteroid>(), 50, true, ModContent.WallType<AsteroidWall>());
			}

			for (int k = 0; k < numJunkPiles; k++) //junkPiles
			{
				int angle = WorldGen.genRand.Next(360);
				float xsize = (float)(WorldGen.genRand.Next(100, 120)) / 100;
				float ysize = (float)(WorldGen.genRand.Next(100, 120)) / 100;
				int size = WorldGen.genRand.Next(3, 4);
				int x = i + (int)(WorldGen.genRand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + WorldGen.genRand.Next(-100, 100);
				int y = j + (int)(WorldGen.genRand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + WorldGen.genRand.Next(-10, 15);
				PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<SpaceJunkTile>(), 50);
			}

			for (int k = 0; k < numberOfBigs; k++) //big asteroids
			{
				int x = i + (int)(WorldGen.genRand.Next(0 - width, width) / 1.5f);
				int y = j + WorldGen.genRand.Next(0 - height, height);
				float xsize = (float)(WorldGen.genRand.Next(75, 133)) / 100;
				float ysize = (float)(WorldGen.genRand.Next(75, 133)) / 100;
				int size = WorldGen.genRand.Next(11, 17);
				PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<BigAsteroid>(), 10, true, ModContent.WallType<AsteroidWall>());
			}

			for (int k = 0; k < numberOfOres; k++) //ores
			{
				int angle = WorldGen.genRand.Next(360);
				int x = i + (int)(WorldGen.genRand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + WorldGen.genRand.Next(-100, 100);
				int y = j + (int)(WorldGen.genRand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + WorldGen.genRand.Next(-10, 15);
				ushort ore = OreRoller((ushort)ModContent.TileType<Glowstone>());
				WorldGen.TileRunner(x, y, WorldGen.genRand.Next(2, 10), 2, ore, false, 0f, 0f, false, true);
			}

			StructureHelper.Generator.GenerateStructure("Structures/StarAltar", new Point16(i + (int)(WorldGen.genRand.Next(0 - width, width) / 1.5f), j + WorldGen.genRand.Next(-10, height)), SpiritMod.Instance);

			//chest spawning
			const int MaxChestTries = 10000;
			int chestTries = 0;
			int chestSuccesses = 0;

			while (chestTries < MaxChestTries && chestSuccesses < 4)
			{
				int x = i + WorldGen.genRand.Next(0 - width, width);
				int y = j + WorldGen.genRand.Next(0 - height, height);
				if (WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<Tiles.Furniture.SpaceJunk.AsteroidChest>(), false, 0) != -1)
				{
					chestSuccesses++;
					chestTries = 0;
				}
				chestTries++;
			}

			// super lazy basic smoothing pass
			ushort asteroidType = (ushort)ModContent.TileType<Asteroid>();
			ushort junkType = (ushort)ModContent.TileType<SpaceJunkTile>();
			for (int smoothX = i - width; smoothX < i + width; smoothX++)
			{
				for (int smoothY = j - height; smoothY < j + height; smoothY++)
				{
					if (!WorldGen.InWorld(smoothX, smoothY)) 
						continue;

					Tile tile = Framing.GetTileSafely(smoothX, smoothY);
					if (tile.HasTile && (tile.TileType == junkType || tile.TileType == asteroidType) && WorldGen.genRand.NextBool(2))
						Tile.SmoothSlope(smoothX, smoothY);
				}
			}
		}

		private static void PlaceBlob(int x, int y, float xsize, float ysize, int size, int type, int roundness, bool placewall = false, int walltype = 0)
		{
			int distance = size;
			for (int i = 0; i < 360; i++)
			{
				if (360 - i <= Math.Abs(size - distance) / Math.Sqrt(size) * 50)
				{
					if (size > distance)
					{
						distance++;
					}
					else
					{
						distance--;
					}
				}
				else
				{
					int increase = WorldGen.genRand.Next(roundness);
					if (increase == 0 && distance > 3)
					{
						distance--;
					}
					if (increase == 1)
					{
						distance++;
					}
				}

				int offsetX = (int)(Math.Sin(i * (Math.PI / 180)) * distance * xsize);
				int offsetY = (int)(Math.Cos(i * (Math.PI / 180)) * distance * ysize);
				WorldExtras.PlaceLine(x, y, x + offsetX, y + offsetY, type, placewall, walltype);
			}
		}

		private static ushort OreRoller(ushort glowstone)
		{
			ushort iron = WorldExtras.GetOreCounterpart(WorldGen.SavedOreTiers.Iron);
			ushort silver = WorldExtras.GetOreCounterpart(WorldGen.SavedOreTiers.Silver);
			ushort gold = WorldExtras.GetOreCounterpart(WorldGen.SavedOreTiers.Gold);

			int roll = WorldGen.genRand.Next(1120);
			if (roll < 250)
				return WorldExtras.GetOreCounterpart(iron);
			else if (roll < 400)
				return WorldExtras.GetOreCounterpart(silver);
			else if (roll < 600)
				return WorldExtras.GetOreCounterpart(gold);
			else if (roll < 650)
				return TileID.Meteorite;
			else
				return glowstone;
		}
		#endregion Asteroids

		#region GENPASS: PILES/AMBIENT
		public static void PilesPass(GenerationProgress progress, GameConfiguration config)
		{
			progress.Message = "Spirit Mod: Adding Ambient Objects...";

			//Copper/tin piles
			for (int i = 0; i < Main.maxTilesX * 22.5; i++)
			{
				int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
				int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
				Tile tile = Main.tile[num3, num4];
				if (tile.TileType == TileID.Stone && tile.HasTile)
					WorldGen.PlaceObject(num3, num4 - 1, WorldGen.SavedOreTiers.Copper == TileID.Copper ? ModContent.TileType<CopperPile>() : ModContent.TileType<TinPile>());
			}

			//Iron/lead piles
			for (int i = 0; i < Main.maxTilesX * 15; i++)
			{
				int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
				int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
				Tile tile = Main.tile[num3, num4];
				if (tile.TileType == TileID.Stone && tile.HasTile)
					WorldGen.PlaceObject(num3, num4 - 1, WorldGen.SavedOreTiers.Iron == TileID.Iron ? ModContent.TileType<IronPile>() : ModContent.TileType<LeadPile>());
			}

			//Silver/tungsten piles
			for (int i = 0; i < Main.maxTilesX * 11.75f; i++)
			{
				int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
				int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
				Tile tile = Main.tile[num3, num4];
				if (tile.TileType == TileID.Stone && tile.HasTile)
					WorldGen.PlaceObject(num3, num4 - 1, WorldGen.SavedOreTiers.Silver == TileID.Silver ? ModContent.TileType<SilverPile>() : ModContent.TileType<TungstenPile>());
			}

			//Gold/platinum piles
			for (int i = 0; i < Main.maxTilesX * 7.5f; i++)
			{
				int num3 = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
				int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
				Tile tile = Main.tile[num3, num4];
				if (tile.TileType == TileID.Stone && tile.HasTile)
					WorldGen.PlaceObject(num3, num4 - 1, WorldGen.SavedOreTiers.Gold == TileID.Gold ? ModContent.TileType<GoldPile>() : ModContent.TileType<PlatinumPile>());
			}

			for (int C = 0; C < Main.maxTilesX * 10; C++)
			{
				int X = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
				int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 200);
				if (Main.tile[X, Y].TileType == TileID.Stone)
					WorldGen.PlaceObject(X, Y, ModContent.TileType<ExplosiveBarrelTile>());
			}

			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY * 78) * 15E-05); k++)
			{
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);
				int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
				if (Main.tile[x, y] != null && Main.tile[x, y].HasTile)
				{
					if (Main.tile[x, y].TileType == TileID.Granite)
						WorldGen.OreRunner(x, y, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)ModContent.TileType<GraniteOre>());

					if (Main.tile[x, y].TileType == TileID.Marble)
						WorldGen.OreRunner(x, y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(4, 9), (ushort)ModContent.TileType<MarbleOre>());
				}
			}

			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY * 7.2f) * 6E-03); k++)
			{
				int X = WorldGen.genRand.Next(100, Main.maxTilesX - 20);
				int Y = WorldGen.genRand.Next((int)Main.worldSurface - 100, (int)Main.worldSurface + 30);

				bool ice = WorldGen.genRand.NextBool(2);
				int[] types = GlobalExtensions.TileSet<SnowBush1, SnowBush2, SnowBush3, TundraBerries1x2, TundraBerries2x2>();
				if (ice)
					types = GlobalExtensions.TileSet<IceCube1, IceCube2, IceCube3>();

				if (Main.tile[X, Y].TileType == TileID.SnowBlock || Main.tile[X, Y].TileType == TileID.IceBlock)
				{
					int type = WorldGen.genRand.Next(types);
					WorldGen.PlaceObject(X, Y, type);
					NetMessage.SendObjectPlacment(-1, X, Y, type, 0, 0, -1, -1);
				}
			}

			int[] mushSet = GlobalExtensions.TileSet<WhiteMushroom2x2, WhiteMushroom2x3, RedMushroom1x1, RedMushroom2x2, RedMushroom3x2, BrownMushrooms, BrownMushroomLarge>();
			AddDecorSpam("Mushrooms", mushSet, TileObjectData.GetTileData(ModContent.TileType<WhiteMushroom2x2>(), 0).AnchorValidTiles, 700, ((int)Main.worldSurface, Main.maxTilesY - 200));
			AddDecorSpam("OreDeposits", new int[] { ModContent.TileType<OreDeposits>() }, TileSets.Mosses.With(TileID.Stone), 150, ((int)Main.worldSurface, Main.maxTilesY - 200));
			AddDecorSpam("BlueShards", new[] { ModContent.TileType<BlueShardBig>() }, GlobalExtensions.TileSet<Asteroid, BigAsteroid>(), 14, (42, (int)Main.worldSurface));
			int[] sculptures = GlobalExtensions.TileSet<IceWheezerPassive, IceFlinxPassive, IceBatPassive, IceVikingPassive, IceWheezerHostile, IceFlinxHostile, IceBatHostile, IceVikingHostile>();
			AddDecorSpam("IceStatues", sculptures, new int[] { TileID.IceBlock, TileID.SnowBlock }, 120, ((int)WorldGen.rockLayer, Main.maxTilesY));

			PopulateSpam();
		}

		private static void AddDecorSpam(string name, int[] types, int[] ground, int baseReps, (int high, int low) rangeY, bool forced = true)
		{
			DecorSpamData data = new DecorSpamData(name, types, ground, baseReps, rangeY, forced);
			decorSpam.Add(data);
		}

		private static void PopulateSpam()
		{
			int maxReps = 0;
			Dictionary<string, int> repeatsByName = new Dictionary<string, int>();

			foreach (var item in decorSpam)
			{
				if (item.BaseRepeats > maxReps)
					maxReps = item.BaseRepeats;

				repeatsByName.Add(item.Name, 0);
			}

			for (int i = 0; i < maxReps * GlobalExtensions.WorldSize; ++i)
			{
				foreach (var item in decorSpam)
				{
					if (repeatsByName[item.Name] >= item.RealRepeats)
						continue;

				retry:
					int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
					int y = WorldGen.genRand.Next(item.RangeY.high, item.RangeY.low);
					Tile tile = Main.tile[x, y];

					if (tile.HasTile && item.ValidGround.Contains(tile.TileType))
					{
						int type = WorldGen.genRand.Next(item.Types);
						TileObjectData data = TileObjectData.GetTileData(type, 0);

						int originY = data is not null ? data.Origin.Y : 1;
						int height = data is not null ? data.Height : 1;
						int styleRange = data is not null ? data.RandomStyleRange : 1;
						int style = WorldGen.genRand.Next(styleRange);

						bool didPlace = WorldGen.PlaceObject(x, y - height + originY, type, true, style);

						if (didPlace)
						{
							repeatsByName[item.Name]++;
							NetMessage.SendObjectPlacment(-1, x, y - height + originY, type, style, 0, -1, -1);
						}
						else
						{
							if (!item.Forced)
							{
								repeatsByName[item.Name]++;
								continue;
							}

							goto retry; //worldgen code is a cesspool you can deal with a little goto
						}
					}
					else
						goto retry;
				}
			}

			decorSpam.Clear();
		}
		#endregion GENPASS: Piles/Ambient
	}
}