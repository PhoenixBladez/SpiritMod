using Microsoft.Xna.Framework;
using SpiritMod.Noise;
using SpiritMod.Sepulchre;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Furniture;
using SpiritMod.Tiles.Walls.Natural;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace SpiritMod
{
	public class Sepulchure : ModWorld
	{
		private int wall => ModContent.WallType<SepulchreWallTile>();
		private int tile => ModContent.TileType<SepulchreBrick>();
		private int tiletwo => ModContent.TileType<SepulchreBrickTwo>();
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int index = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle Chests"));
			if (index == -1) {
				return;
			}
			//plz scale for me cause im so out of the loop rn

			tasks.Insert(++index, new PassLegacy("Sepulchure",
				delegate (GenerationProgress progress) {
					progress.Message = "Sepulchuring";

					for(int i = 0; i< Main.maxTilesX/200; i++) {
						CreateSepulchre(new Vector2(Main.rand.Next(400,Main.maxTilesX - 400), Main.rand.Next((int)Main.maxTilesY - 400, (int)Main.maxTilesY - 200)));
					}
				}, 300f));
		}
		public void CreateSepulchre(Vector2 position)
		{
			PerlinNoiseTwo noiseType = new PerlinNoiseTwo(WorldGen._genRandSeed);
			int i = (int)position.X;
			int j = (int)position.Y;
			/*for (int x = i - 50; x < i + 50; x++) //dirt square
             {
                 for (int y = j - 90; y < j + 50; y++)
                 {
                    Main.tile[x,y].active(true);
                 }
             }*/
			for (int x = i - 25; x < i + 25; x++) //Main structure, DO NOT USE LOOPTHROUGHTILES HERE
			{
				for (int y = j - 20; y < j + 20; y++) {
					if (noiseType.Noise2D((x * 600) / (float)4200, (y * 600) / (float)1200) > 0.85f) //regular rooms
					{
						CreateRoom(x, y, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(5, 10));
					}
					if (noiseType.Noise2D((x * 600) / (float)4200, (y * 600) / (float)1200) > 0.95f || (x == i && y == j)) //towers
					{
						int towerheight = 30;
						int width = WorldGen.genRand.Next(5, 8);
						CreateRoom(x, y - towerheight, width, towerheight - 8);
						CreateHalfCircle(x + (int)(width / 2) + 2, y - (int)(towerheight * 1.5f), width + 4);
					}
				}
			}
			WallCleanup(i, j);
			CreateChests(i, j);
			PolishSepulchre(i, j);
		}
		public delegate void AtTile(int x, int y);
		public void WallCleanup(int i, int j)
		{
			List<AtTile> delegateList = new List<AtTile>();
			delegateList.Add(delegate (int x, int y) //clear away 2 wide walls
			{
				DeleteWallVertical(x, y, 6);
				DeleteWallHorizontal(x, y, 6);
			});
			delegateList.Add(delegate (int x, int y) //kill orphan tiles
			{
				DeleteOrphan(x, y);
			});
			delegateList.Add(delegate (int x, int y) //clear away 2 wide walls
			{
				DeleteWallVertical(x, y, 4);
				DeleteWallHorizontal(x, y, 4);
			});
			foreach (AtTile atTile in delegateList) {
				LoopThroughTiles(i, j, atTile);
			}
		}
		public void CreateChests(int i, int j)
		{
			int chests = 0;
			int tries = 0;
			while (chests == 0) {
				for (int x = i - 50; x < i + 50; x++) {
					for (int y = j - 90; y < j + 50; y++) {
						if ((Main.tile[x, y + 1].type == tile || Main.tile[x, y + 1].type == tiletwo) && 
							(Main.tile[x + 1, y + 1].type == tile || Main.tile[x + 1, y + 1].type == tiletwo) &&
							(Main.tile[x - 1, y + 1].type == tile || Main.tile[x - 1, y + 1].type == tiletwo)
							&& chests == 0 && Main.rand.Next(100) == 0 && Main.tile[x, y].wall == wall) {
							WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<SepulchreChestTile>(), false, 0);
							if (Main.tile[x, y - 1].type == (ushort)ModContent.TileType<SepulchreChestTile>()) {
								chests++;
							}
						}
					}
				}
				tries++;
				if (tries > 6000) {
					Main.NewText("Error: No space to place a chest found", 255, 255, 255);
					break;
				}
			}
		}

		public void PolishSepulchre(int i, int j)
		{
			bool placedEvilTome = false;
			PerlinNoiseTwo noiseType2 = new PerlinNoiseTwo(WorldGen._genRandSeed);
			List<AtTile> delegateList = new List<AtTile>();
			delegateList.Add(delegate (int x, int y) //platforms
			{
				if (Main.rand.Next(50) == 0 && Main.tile[x - 1, y].type == tile && Main.tile[x - 1, y].active()) {
					CreateShelf(x, y, 50, 12, false, ref placedEvilTome);
				}
			});
			delegateList.Add(delegate (int x, int y) //cursed armor
			{
				if ((Main.tile[x, y + 1].type == tile || Main.tile[x, y + 1].type == tiletwo) && Main.rand.Next(17) == 1 && Main.tile[x, y].wall == wall) {
					WorldGen.PlaceObject(x, y, ModContent.TileType<CursedArmor>());
				}
			});
			delegateList.Add(delegate (int x, int y) //pots
			{
				if ((Main.tile[x, y + 1].type == tile || Main.tile[x, y + 1].type == tiletwo) && Main.rand.Next(4) == 1 && Main.tile[x, y].wall == wall) {
					int potType = 0;
					switch (Main.rand.Next(3)) {
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
			});
			delegateList.Add(delegate (int x, int y) //cracks in walls
			{
				if (noiseType2.Noise2D((float)(x * 200) / (float)1200, (float)(y * 200) / (float)1200) > 0.75f && Main.tile[x, y].wall == wall) {
					Main.tile[x, y].wall = (ushort)0;
				}
			});
			delegateList.Add(delegate (int x, int y) //chandeliers
			{
				if ((Main.tile[x, y - 1].type == tile || Main.tile[x, y - 1].type == tiletwo) && Main.rand.Next(12) == 1 && Main.tile[x, y].wall == wall) {
					WorldGen.PlaceObject(x, y, ModContent.TileType<SepulchreChandelier>());
				}
			});
			delegateList.Add(delegate (int x, int y) //Windows
			{
				if (WorldGen.genRand.Next(30) == 0 && (Main.tile[x - 1, y].type == tile || Main.tile[x - 1, y].type == tiletwo) && Main.tile[x - 1, y].active()) {
					int windowType = 0;
					switch (Main.rand.Next(2)) {
						case 0:
							windowType = ModContent.TileType<SepulchreWindowOne>();
							break;
						default:
							windowType = ModContent.TileType<SepulchreWindowTwo>();
							break;
					}
					CreateWindowRow(x, y, 50, windowType);
				}
			});
			delegateList.Add(delegate (int x, int y) //shelves
			{
				if (Main.rand.Next(30) == 0 && (Main.tile[x - 1, y].type == tile || Main.tile[x - 1, y].type == tiletwo) && Main.tile[x - 1, y].active()) {
					CreateShelf(x, y, Main.rand.Next(4, 8), 12, true, ref placedEvilTome);
				}
				if (Main.rand.Next(30) == 0 && (Main.tile[x - 1, y].type == tile || Main.tile[x - 1, y].type == tiletwo) && Main.tile[x + 1, y].active()) {
					CreateShelfBackwards(x, y, Main.rand.Next(4, 8), 12, true, ref placedEvilTome);
				}
			});
			delegateList.Add(delegate (int x, int y) //cobwebs
			{
				if (noiseType2.Noise2D((float)(x * 100) / (float)1200, (float)(y * 100) / (float)1200) > 0.70f && Main.tile[x, y].wall == wall && !Main.tile[x, y].active()) {
					Main.tile[x, y].active(true);
					Main.tile[x, y].type = 51;
				}
			});
			delegateList.Add(delegate (int x, int y) //cracks in tiles
			{
				if (Main.rand.Next(25) == 0 && (Main.tile[x, y].type == tile || Main.tile[x, y].type == tiletwo) && Main.tile[x, y].active()) {
					Main.tile[x, y].active(false);
				}
			});
			delegateList.Add(delegate (int x, int y) //torches
			{
				if (Main.rand.Next(200) == 0 && Main.tile[x, y].wall == wall && !Main.tile[x, y].active()) {
					WorldGen.PlaceTile(x, y, 4, true, false, -1, 8);
				}
			});
			delegateList.Add(delegate (int x, int y) //mirrors
			{
				if (Main.rand.Next(200) == 0 && Main.tile[x, y].wall == wall && !Main.tile[x, y].active()) {
					if (!MirrorsNearby(x, y))
						WorldGen.PlaceObject(x, y, ModContent.TileType<SepulchreMirror>());
				}
			});
			foreach (AtTile atTile in delegateList) {
				LoopThroughTiles(i, j, atTile);
			}
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
						tile2.active(true);
						tile2.type = (ushort)tile;
					}
					else {
						if (tile2.type != tile && tile2.type != tiletwo)
							tile2.active(false);
						tile2.wall = (ushort)wall;

					}
				}
			}
		}
		public bool MirrorsNearby(int x, int y)
		{
			for (int i = x - 6; i < x + 6; i++) {
				for (int j = y - 6; j < y + 6; j++) {
					if (Main.tile[i, j].type == ModContent.TileType<SepulchreMirror>() || Main.tile[i, j].type == ModContent.TileType<SepulchreWindowOne>() || Main.tile[i, j].type == ModContent.TileType<SepulchreWindowTwo>()) {
						return true;
					}
				}
			}
			return false;
		}
		public static void PlaceShelfItem(int x, int y, ref bool placedEvilTome)
		{
			if (Main.rand.Next(15) == 1 && !placedEvilTome) {
				WorldGen.PlaceTile(x, y, ModContent.TileType<ScreamingTomeTile>(), true, false, -1);
				placedEvilTome = true;
				return;
			}
			switch (Main.rand.Next(3)) {
				case 0:
					WorldGen.PlaceTile(x, y, 50, true, false, -1, Main.rand.Next(5));
					break;
				case 1:
					WorldGen.PlaceTile(x, y, 13, true, false, -1, Main.rand.Next(3));
					break;
			}
		}
		public void CreateShelf(int i, int j, int length, int platformType, bool placeobjects, ref bool placedEvilTome)
		{
			for (int x = i; x < i + length; x++) {
				if (Main.tile[x, j].active() || Main.tile[x, j - 1].active() || Main.tile[x, j - 2].active() || Main.tile[x, j].wall != wall) {
					return;
				}
				WorldGen.PlaceTile(x, j, 19, true, false, -1, platformType);
				if (placeobjects)
					PlaceShelfItem(x, j - 1, ref placedEvilTome);
			}
		}
		public void CreateShelfBackwards(int i, int j, int length, int platformType, bool placeobjects, ref bool placedEvilTome)
		{
			for (int x = i; x > i - length; x--) {
				if (Main.tile[x, j].active() || Main.tile[x, j - 1].active() || Main.tile[x, j - 2].active() || Main.tile[x, j].wall != wall) {
					return;
				}
				WorldGen.PlaceTile(x, j, 19, true, false, -1, platformType);
				if (placeobjects)
					PlaceShelfItem(x, j - 1, ref placedEvilTome);
			}
		}
		public void CreateWindowRow(int i, int j, int length, int windowType)
		{
			for (int x = i + 2; x < i + length; x += 5) {
				if (Main.tile[x, j].active() || Main.tile[x, j - 1].active() || Main.tile[x, j - 2].active() || Main.tile[x, j].wall != wall) {
					return;
				}
				if (Main.rand.Next(3) != 0)
					WorldGen.PlaceObject(x, j, windowType);
			}
		}
		public void CreateRoom(int i, int j, int width, int height)
		{
			for (int x = i - (width / 2); x <= i + (width * 2); x++) {
				for (int y = j - (height / 2); y <= j + (height * 2); y++) {
					Tile tile2 = Main.tile[x, y];
					if (y == j - (height / 2) || y == j + (height * 2) || x == i - (width / 2) || x == i + (width * 2)) {
						tile2.active(true);
						tile2.type = (ushort)tiletwo;
					}
					else if (y == j - (height / 2) + 1 || y == j + (height * 2) - 1 || x == i - (width / 2) + 1 || x == i + (width * 2) - 1) {
						tile2.active(true);
						tile2.type = (ushort)tiletwo;
					}
					else {
						tile2.active(false);
						tile2.wall = (ushort)wall;
						tile2.liquid = 0;
					}
				}
			}
		}
		public void DeleteWallVertical(int i, int j, int width)
		{
			width /= 2;
			if (!Main.tile[i, j - width].active() && !Main.tile[i, j + width].active() && Main.tile[i, j + width].wall == wall && Main.tile[i, j - width].wall == wall) {
				for (int y = j - width; y <= j + width; y++) {
					Main.tile[i, y].active(false);
					Main.tile[i, y].wall = (ushort)wall;
				}
			}
		}
		public void DeleteWallHorizontal(int i, int j, int width)
		{
			width /= 2;
			if (!Main.tile[i - width, j].active() && !Main.tile[i + width, j].active() && Main.tile[i + width, j].wall == wall && Main.tile[i - width, j].wall == wall) {
				for (int x = i - width; x <= i + width; x++) {
					Main.tile[x, j].active(false);
					Main.tile[x, j].wall = (ushort)wall;
				}
			}
		}
		public void DeleteOrphan(int i, int j)
		{
			if (!Main.tile[i - 1, j].active() && !Main.tile[i + 1, j].active() && !Main.tile[i, j - 1].active() && !Main.tile[i, j + 1].active() && (Main.tile[i, j].type == tile || Main.tile[i, j].type == tiletwo)) {
				Main.tile[i, j].active(false);
			}
		}
	}
}
