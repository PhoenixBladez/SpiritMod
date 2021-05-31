using Microsoft.Xna.Framework;
using SpiritMod.Noise;
using SpiritMod.Sepulchre;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Walls.Natural;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace SpiritMod
{
	public class Sepulchure : ModWorld
	{
		private int Wall => ModContent.WallType<SepulchreWallTile>();
		private int Tile => ModContent.TileType<SepulchreBrick>();
		private int TileTwo => ModContent.TileType<SepulchreBrickTwo>();

		public static void RemoveWaterFromRegion(int width, int height, Vector2 startingPoint)
		{
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					if (Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquidType() == 0 && Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquid > 64) {
						Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].ClearEverything();
						if (Main.netMode == NetmodeID.MultiplayerClient) // sync
						{
							NetMessage.sendWater(i + (int)startingPoint.X, j + (int)startingPoint.Y);
						}
					}
				}
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int index = tasks.FindIndex(genpass => genpass.Name.Equals("Settle Liquids Again"));
			if (index == -1) {
				return;
			}
			//plz scale for me cause im so out of the loop rn

			tasks.Insert(++index, new PassLegacy("Sepulchure",
				delegate (GenerationProgress progress) {
					progress.Message = "Spirit Mod: Generating Dark Sepulchres...";

					for(int i = 0; i< Main.maxTilesX/200; i++) {
						CreateSepulchre(new Vector2(WorldGen.genRand.Next((int)(Main.maxTilesX * 0.2f), (int)(Main.maxTilesX * 0.8f)), WorldGen.genRand.Next(Main.maxTilesY - 500, Main.maxTilesY - 300)));
					}
				}, 300f));
		}

		public void CreateSepulchre(Vector2 position)
		{
			int[] invalidTypes = new int[]
			{
				TileID.BeeHive,
				TileID.BlueDungeonBrick,
				TileID.GreenDungeonBrick,
				TileID.PinkDungeonBrick,
				TileID.LihzahrdBrick,
				TileTwo
			};

			bool fail = false;
			for (int x = (int)position.X - 150; x < (int)position.X + 150; x++)
			{
				for (int y = (int)position.Y - 150; y < (int)position.Y + 150; y++) {
					for (int z = 0; z < invalidTypes.Length; z++)
					{
						if (Framing.GetTileSafely(x, y).type == invalidTypes[z])
						{
							fail = true;
							break;
						}
					}
					if (fail)
						break;
				}
			}

			int tries = 0;
			while (fail) {
				position = new Vector2(WorldGen.genRand.Next((int)(Main.maxTilesX * 0.2f), (int)(Main.maxTilesX * 0.8f)), WorldGen.genRand.Next(Main.maxTilesY - 500, Main.maxTilesY - 300));
				fail = false;
				for (int x = (int)position.X - 150; x < (int)position.X + 150; x++) //Main structure, DO NOT USE LOOPTHROUGHTILES HERE
				{
					for (int y = (int)position.Y - 150; y < (int)position.Y + 150; y++) {
						for (int z = 0; z < invalidTypes.Length; z++)
							if (Framing.GetTileSafely(x, y).type == invalidTypes[z]) {
								fail = true;
								break;
							}

						if (fail)
							break;
					}
				}
				tries++;
				if (tries > 100)
					return;
			}

			var noiseType = new PerlinNoiseTwo(WorldGen._genRandSeed);
			int i = (int)position.X;
			int j = (int)position.Y;

			for (int x = i - 25; x < i + 25; x++) //Main structure, DO NOT USE LOOPTHROUGHTILES HERE
			{
				for (int y = j - 20; y < j + 20; y++) {
					if (noiseType.Noise2D((x * 600) / (float)4200, (y * 600) / (float)1200) > 0.85f) //regular rooms
						CreateRoom(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(5, 10));
					if (noiseType.Noise2D((x * 600) / (float)4200, (y * 600) / (float)1200) > 0.95f || (x == i && y == j)) //towers
					{
						int towerheight = 30;
						int width = WorldGen.genRand.Next(5, 8);
						CreateRoom(x, y - towerheight, width, towerheight - 8);
						CreateHalfCircle(x + (width / 2) + 2, y - (int)(towerheight * 1.5f), width + 4);
					}
				}
			}

			WallCleanup(i, j);
			for (int x = i - 50; x < i + 50; x++)
				for (int y = j - 90; y < j + 50; y++)
					if (Main.rand.Next(25) == 0 && (Main.tile[x, y].type == Tile || Main.tile[x, y].type == TileTwo) && Main.tile[x, y].active())
						Main.tile[x, y].active(false);
			CreateChests(i, j);
			PolishSepulchre(i, j);
			PlaceHauntedTome(i, j);
			RemoveWaterFromRegion(50,40,position - new Vector2(25,20));
		}

		public delegate void AtTile(int x, int y);

		public void WallCleanup(int i, int j)
		{
			var delegateList = new List<AtTile>
			{
				delegate (int x, int y) //clear away 2 wide walls
				{
					DeleteWallVertical(x, y, 6);
					DeleteWallHorizontal(x, y, 6);
				},
				delegate (int x, int y) //kill orphan tiles
				{
					DeleteOrphan(x, y);
				},
				delegate (int x, int y) //clear away 2 wide walls
				{
					DeleteWallVertical(x, y, 4);
					DeleteWallHorizontal(x, y, 4);
				}
			};
			foreach (AtTile atTile in delegateList)
				LoopThroughTiles(i, j, atTile);
		}

		public void CreateChests(int i, int j)
		{
			bool placedchest = false;
			int tries = 0;
			while (!placedchest) {
				for (int x = i - 50; x < i + 50; x++) {
					for (int y = j - 90; y < j + 50; y++) {
						if ((Main.tile[x, y + 1].type == Tile || Main.tile[x, y + 1].type == TileTwo)
							&& !placedchest && Main.rand.Next(100) == 0 && Main.tile[x, y].wall == Wall) {
							Main.tile[x + 1, y + 1].active(true);
							Main.tile[x + 1, y + 1].type = Main.tile[x, y + 1].type;
							WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<SepulchreChestTile>(), false, 0);
							if (Main.tile[x, y - 1].type == (ushort)ModContent.TileType<SepulchreChestTile>()) 
								placedchest = true;

						}
					}
				}
				tries++;
				if (tries > 6000)
					break;
			}
		}

		public void PlaceHauntedTome(int i, int j)
		{
			bool success = false;
			int tries = 0;
			while (!success) {
				for (int x = i - 50; x < i + 50; x++) {
					for (int y = j - 90; y < j + 50; y++) {
						if (Main.tile[x, y + 1].type == TileID.Platforms && !success && Main.rand.Next(100) == 0 && Main.tile[x, y].wall == Wall) {
							WorldGen.KillTile(x, y);
							WorldGen.PlaceTile(x, y, (ushort)ModContent.TileType<ScreamingTomeTile>(), true);
							if (Main.tile[x, y].type == (ushort)ModContent.TileType<ScreamingTomeTile>())
								success = true;
						}
					}
				}
				tries++;
				if (tries > 6000) 
					break;
			}
		}

		public void PolishSepulchre(int i, int j)
		{
			var noiseType2 = new PerlinNoiseTwo(WorldGen._genRandSeed);
			var delegateList = new List<AtTile>
			{
				delegate (int x, int y) //platforms
				{
					if (Main.rand.Next(50) == 0 && Main.tile[x - 1, y].type == Tile && Main.tile[x - 1, y].active())
						CreateShelf(x, y, 50, 12, false);
				},
				delegate (int x, int y) //cursed armor
				{
					if ((Main.tile[x, y + 1].type == Tile || Main.tile[x, y + 1].type == TileTwo) && Main.rand.Next(17) == 1 && Main.tile[x, y].wall == Wall)
						WorldGen.PlaceObject(x, y, ModContent.TileType<CursedArmor>());
				},
				delegate (int x, int y) //pots
				{
					if ((Main.tile[x, y + 1].type == Tile || Main.tile[x, y + 1].type == TileTwo) && Main.rand.Next(4) == 1 && Main.tile[x, y].wall == Wall)
					{
						int potType = 0;
						switch (Main.rand.Next(3))
						{
							case 0:
								potType = ModContent.TileType<SepulchrePot1>();
								break;
							case 1:
								potType = ModContent.TileType<SepulchrePot2>();
								break;
							case 2:
								potType = ModContent.TileType<SepulchrePot2>();
								break;
						}
						WorldGen.PlaceObject(x, y, potType);
					}
				},
				delegate (int x, int y) //cracks in walls
				{
					if (noiseType2.Noise2D((x * 200) / (float)1200, (y * 200) / (float)1200) > 0.75f && Main.tile[x, y].wall == Wall)
						Main.tile[x, y].wall = 0;
				},
				delegate (int x, int y) //chandeliers
				{
					if ((Main.tile[x, y - 1].type == Tile || Main.tile[x, y - 1].type == TileTwo) && Main.rand.Next(12) == 1 && Main.tile[x, y].wall == Wall)
						WorldGen.PlaceObject(x, y, ModContent.TileType<SepulchreChandelier>());
				},
				delegate (int x, int y) //Windows
				{
					if (WorldGen.genRand.Next(30) == 0 && (Main.tile[x - 1, y].type == Tile || Main.tile[x - 1, y].type == TileTwo) && Main.tile[x - 1, y].active())
					{
						int windowType = 0;
						switch (Main.rand.Next(2))
						{
							case 0:
								windowType = ModContent.TileType<SepulchreWindowOne>();
								break;
							default:
								windowType = ModContent.TileType<SepulchreWindowTwo>();
								break;
						}
						CreateWindowRow(x, y, 50, windowType);
					}
				},
				delegate (int x, int y) //shelves
				{
					if (Main.rand.Next(20) == 0 && (Main.tile[x - 1, y].type == Tile || Main.tile[x - 1, y].type == TileTwo) && Main.tile[x - 1, y].active())
						CreateShelf(x, y, Main.rand.Next(4, 8), 12, true);
					if (Main.rand.Next(20) == 0 && (Main.tile[x - 1, y].type == Tile || Main.tile[x - 1, y].type == TileTwo) && Main.tile[x + 1, y].active())
						CreateShelfBackwards(x, y, Main.rand.Next(4, 8), 12, true);
				},
				delegate (int x, int y) //cobwebs
				{
					if (noiseType2.Noise2D((x * 100) / (float)1200, (y * 100) / (float)1200) > 0.70f && Main.tile[x, y].wall == Wall && !Main.tile[x, y].active())
					{
						Main.tile[x, y].active(true);
						Main.tile[x, y].type = 51;
					}
				},
				delegate (int x, int y) //torches
				{
					if (Main.rand.Next(200) == 0 && Main.tile[x, y].wall == Wall && !Main.tile[x, y].active())
						WorldGen.PlaceTile(x, y, 4, true, false, -1, 8);
				},
				delegate (int x, int y) //mirrors
				{
					if (Main.rand.Next(200) == 0 && Main.tile[x, y].wall == Wall && !Main.tile[x, y].active())
						if (!MirrorsNearby(x, y))
							WorldGen.PlaceObject(x, y, ModContent.TileType<SepulchreMirror>());
				}
			};

			foreach (AtTile atTile in delegateList)
				LoopThroughTiles(i, j, atTile);
		}

		public static void LoopThroughTiles(int i, int j, AtTile atTile)
		{
			for (int x = i - 50; x < i + 50; x++) {
				for (int y = j - 90; y < j + 50; y++) {
					atTile.Invoke(x, y);
				}
			}
		}

		public void CreateHalfCircle(int x, int y, int diameter)
		{
			for (float i = -3.14f; i < 0f; i += 0.01f) {
				for (float j = 0; j <= diameter; j += 0.5f) {
					Vector2 pos = i.ToRotationVector2();
					pos *= j;
					pos.Y += Math.Abs(pos.X);
					Tile tile2 = Main.tile[(int)pos.X + x, (int)pos.Y + y];
					if (Math.Abs(j - diameter) <= 0.5f) {
						tile2.ClearTile();
						WorldGen.PlaceTile((int)pos.X + x, (int)pos.Y + y, Tile, true);
					}
					else {
						if (tile2.type != Tile && tile2.type != TileTwo)
							tile2.ClearEverything();

						WorldGen.PlaceWall((int)pos.X + x, (int)pos.Y + y, Wall, true);
					}
				}
			}
		}

		public bool MirrorsNearby(int x, int y)
		{
			for (int i = x - 6; i < x + 6; i++) {
				for (int j = y - 6; j < y + 6; j++) {
					if (Main.tile[i, j].type == ModContent.TileType<SepulchreMirror>() || Main.tile[i, j].type == ModContent.TileType<SepulchreWindowOne>() || Main.tile[i, j].type == ModContent.TileType<SepulchreWindowTwo>())
						return true;
				}
			}
			return false;
		}

		public static void PlaceShelfItem(int x, int y)
		{
			switch (Main.rand.Next(3)) {
				case 0:
					WorldGen.PlaceTile(x, y, 50, true, false, -1, Main.rand.Next(5));
					break;
				case 1:
					WorldGen.PlaceTile(x, y, 13, true, false, -1, Main.rand.Next(3));
					break;
			}
		}

		public void CreateShelf(int i, int j, int length, int platformType, bool placeobjects)
		{
			for (int x = i; x < i + length; x++) {
				if (Main.tile[x, j].active() || Main.tile[x, j - 1].active() || Main.tile[x, j - 2].active() || Main.tile[x, j].wall != Wall)
					return;
				WorldGen.PlaceTile(x, j, 19, true, false, -1, platformType);
				if (placeobjects)
					PlaceShelfItem(x, j - 1);
			}
		}

		public void CreateShelfBackwards(int i, int j, int length, int platformType, bool placeobjects)
		{
			for (int x = i; x > i - length; x--) {
				if (Main.tile[x, j].active() || Main.tile[x, j - 1].active() || Main.tile[x, j - 2].active() || Main.tile[x, j].wall != Wall)
					return;
				WorldGen.PlaceTile(x, j, 19, true, false, -1, platformType);
				if (placeobjects)
					PlaceShelfItem(x, j - 1);
			}
		}

		public void CreateWindowRow(int i, int j, int length, int windowType)
		{
			for (int x = i + 2; x < i + length; x += 5) {
				if (Main.tile[x, j].active() || Main.tile[x, j - 1].active() || Main.tile[x, j - 2].active() || Main.tile[x, j].wall != Wall)
					return;
				if (Main.rand.Next(3) != 0)
					WorldGen.PlaceObject(x, j, windowType);
			}
		}

		public void CreateRoom(int i, int j, int width, int height)
		{
			for (int x = i - (width / 2); x <= i + (width * 2); x++) {
				for (int y = j - (height / 2); y <= j + (height * 2); y++) {
					Tile tile = Framing.GetTileSafely(x, y);
					if (y == j - (height / 2) || y == j + (height * 2) || x == i - (width / 2) || x == i + (width * 2)) {
						tile.ClearTile();
						WorldGen.PlaceTile(x, y, TileTwo, true);
					}
					else if (y == j - (height / 2) + 1 || y == j + (height * 2) - 1 || x == i - (width / 2) + 1 || x == i + (width * 2) - 1) {
						tile.ClearTile();
						WorldGen.PlaceTile(x, y, TileTwo, true);
					}
					else {
						tile.ClearEverything();
						WorldGen.PlaceWall(x, y, Wall, true);
					}
				}
			}
		}

		public void DeleteWallVertical(int i, int j, int width)
		{
			width /= 2;
			if (!Main.tile[i, j - width].active() && !Main.tile[i, j + width].active() && Main.tile[i, j + width].wall == Wall && Main.tile[i, j - width].wall == Wall) {
				for (int y = j - width; y <= j + width; y++) {
					Framing.GetTileSafely(i, y).ClearEverything();
					WorldGen.PlaceWall(i, y, Wall, true);
				}
			}
		}

		public void DeleteWallHorizontal(int i, int j, int width)
		{
			width /= 2;
			if (!Main.tile[i - width, j].active() && !Main.tile[i + width, j].active() && Main.tile[i + width, j].wall == Wall && Main.tile[i - width, j].wall == Wall) {
				for (int x = i - width; x <= i + width; x++) {
					Framing.GetTileSafely(x, j).ClearEverything();
					WorldGen.PlaceWall(x, j, Wall, true);
				}
			}
		}

		public void DeleteOrphan(int i, int j)
		{
			if (!Main.tile[i - 1, j].active() && !Main.tile[i + 1, j].active() && !Main.tile[i, j - 1].active() && !Main.tile[i, j + 1].active() && (Main.tile[i, j].type == Tile || Main.tile[i, j].type == TileTwo))
				Framing.GetTileSafely(i, j).ClearEverything();
		}
	}
}
