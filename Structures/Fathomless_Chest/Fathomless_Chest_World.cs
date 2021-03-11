using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;


namespace SpiritMod.Structures.Fathomless_Chest
{
	public class Fathomless_Chest_World : ModWorld
	{
		public static bool isThereAChest = false;

		public override void PostUpdate()
		{
			if (!isThereAChest)
			{
				finalShrineGen();
			}
		}
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
							case 0:						
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 2:						
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearEverything();
								break;
							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 6:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 7:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 8:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 9:
								Framing.GetTileSafely(k, l).ClearTile();
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
								tile.slope(0); // I give up, this is retarded
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

		public override void PostWorldGen()
        {
			finalShrineGen();
		}
		
		public void finalShrineGen()
		{
			isThereAChest = true;
			int spawnposXa = Main.rand.Next(Main.maxTilesX);
            int spawnposYa = WorldGen.genRand.Next(Convert.ToInt32(Main.rockLayer), Convert.ToInt32(Main.rockLayer + 500));
            bool safetyCheckLihzard = 
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == TileID.LihzahrdBrick || Main.tile[spawnposXa + 10,spawnposYa + 10].type == TileID.LihzahrdBrick ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 87 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 87;
           

			bool safetyCheckDungeonWalls = 
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 7 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 7 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 8 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 8 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 9 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 9 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 94 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 94 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 98 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 98 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 96 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 96 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 97 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 97 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 99 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 99 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 95 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 95;
			
			bool safetyCheckDungeonTiles = 
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == 41 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 41 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == 43 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 43 ||
			Main.tile[spawnposXa - 10,spawnposYa - 10].type == 44 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 44;
			
            while (safetyCheckLihzard || safetyCheckDungeonWalls || safetyCheckDungeonTiles)
            {
                spawnposXa = Main.rand.Next(Main.maxTilesX);
                spawnposYa = WorldGen.genRand.Next(Convert.ToInt32(Main.rockLayer), Convert.ToInt32(Main.rockLayer + 500));
				
				safetyCheckLihzard = 
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == TileID.LihzahrdBrick || Main.tile[spawnposXa + 10,spawnposYa + 10].type == TileID.LihzahrdBrick ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 87 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 87;
				
				safetyCheckDungeonWalls = 
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 7 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 7 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 8 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 8 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 9 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 9 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 94 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 94 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 98 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 98 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 96 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 96 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 97 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 97 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 99 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 99 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].wall == 95 || Main.tile[spawnposXa + 10,spawnposYa + 10].wall == 95;
				
				safetyCheckDungeonTiles = 
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == 41 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 41 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == 43 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 43 ||
				Main.tile[spawnposXa - 10,spawnposYa - 10].type == 44 || Main.tile[spawnposXa + 10,spawnposYa + 10].type == 44;
            }
            PlaceShrine(spawnposXa, spawnposYa, Structures.Fathomless_Chest.Fathomless_Chest_Arrays.ShrineShape1);
            PlaceShrineMiscs(spawnposXa, spawnposYa, Structures.Fathomless_Chest.Fathomless_Chest_Arrays.Miscs);
		}
		public override TagCompound Save()
		{
			var ttt = new List<string>();
			if (isThereAChest)
			{
				ttt.Add("isThereAChestTTT");
			}

			return new TagCompound {
				{"ttt", ttt}
			};
		}
		
		public override void Load(TagCompound tag)
		{
			var ttt = tag.GetList<string>("ttt");
			isThereAChest = ttt.Contains("isThereAChestTTT");
		}
		
		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				isThereAChest = flags[0];
			}
			else
			{
				ErrorLogger.Log("SpiritMod: Unknown loadVersion: " + loadVersion);
			}
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = isThereAChest;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			isThereAChest = flags[0];
		}
	}
}