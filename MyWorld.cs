using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using System.Reflection;
using Terraria.Utilities;
using static Terraria.ModLoader.ModContent;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpiritMod
{
	public class MyWorld : ModWorld
	{
		private static bool dayTimeLast;
		public static bool dayTimeSwitched;

		public static int auroraType = 1;
		public static int auroraChance = 4;

		public static bool aurora = false;
        public static bool stardustWeather = false;
        public static bool spaceJunkWeather = false;

        public static float asteroidLight = 0;

        public static bool BlueMoon = false;
		public static int SpiritTiles = 0;
		public static int AsteroidTiles = 0;
		public static int ReachTiles = 0;
		public static bool Magicite = false;
		public static bool Thermite = false;
		public static bool Cryolite = false;
		public static bool spiritBiome = false;
		public static bool gmOre = false;
		public static bool starMessage = false;
		public static bool essenceMessage = false;
		public static bool flierMessage = false;

        public static bool gennedTower = false;
        public static bool gennedBandits = false;

        public static bool downedScarabeus = false;
		public static bool downedAncientFlier = false;
		public static bool downedRaider = false;
		public static bool downedAtlas = false;
		public static bool downedInfernon = false;
		public static bool downedSpiritCore = false;
		public static bool downedReachBoss = false;
		public static bool downedDusking = false;
		public static bool downedIlluminantMaster = false;
		public static bool downedOverseer = false;

		public static Dictionary<string, bool> droppedGlyphs = new Dictionary<string, bool>();

		bool night = false;
		public bool txt = false;

		private int WillGenn = 0;
		private int Meme;

		public override void TileCountsAvailable(int[] tileCounts)
		{
			SpiritTiles = tileCounts[mod.TileType("SpiritDirt")]+ tileCounts[mod.TileType("SpiritStone")]
			+tileCounts[mod.TileType("Spiritsand")] +tileCounts[mod.TileType("SpiritIce")] + tileCounts[mod.TileType("SpiritGrass")];
			//now you don't gotta have 6 separate things for tilecount
			ReachTiles = tileCounts[mod.TileType("ReachGrassTile")];
			AsteroidTiles = tileCounts[mod.TileType("Asteroid")] + tileCounts[mod.TileType("BigAsteroid")];
		}
		
		public override TagCompound Save()
		{
			TagCompound data = new TagCompound();
			var downed = new List<string>();
			if (downedScarabeus)
				downed.Add("scarabeus");
			if (downedAncientFlier)
				downed.Add("ancientFlier");
			if (downedRaider)
				downed.Add("starplateRaider");
			if (downedInfernon)
				downed.Add("infernon");
			if (downedReachBoss)
				downed.Add("vinewrathBane");
			if (downedSpiritCore)
				downed.Add("etherealUmbra");
			if (downedDusking)
				downed.Add("dusking");
			if (downedIlluminantMaster)
				downed.Add("illuminantMaster");
			if (downedAtlas)
				downed.Add("atlas");
			if (downedOverseer)
				downed.Add("overseer");
			data.Add("downed", downed);

			TagCompound droppedGlyphTag = new TagCompound();
			foreach (KeyValuePair<string, bool> entry in droppedGlyphs)
			{
				droppedGlyphTag.Add(entry.Key, entry.Value);
			}
			data.Add("droppedGlyphs", droppedGlyphTag);

			data.Add("blueMoon", BlueMoon);

            data.Add("gennedBandits", gennedBandits);
            data.Add("gennedTower", gennedTower);

            SpiritMod.AdventurerQuests.WorldSave(data);
            return data;
		}

		public override void Load(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedScarabeus = downed.Contains("scarabeus");
			downedAncientFlier = downed.Contains("ancientFlier");
			downedRaider = downed.Contains("starplateRaider");
			downedInfernon = downed.Contains("infernon");
			downedReachBoss = downed.Contains("vinewrathBane");
			downedDusking = downed.Contains("dusking");
			downedSpiritCore = downed.Contains("etherealUmbra");
			downedIlluminantMaster = downed.Contains("illuminantMaster");
			downedAtlas = downed.Contains("atlas");
			downedOverseer = downed.Contains("overseer");

            SpiritMod.AdventurerQuests.WorldLoad(tag);
            TagCompound droppedGlyphTag = tag.GetCompound("droppedGlyphs");
			droppedGlyphs.Clear();
			foreach (KeyValuePair<string, object> entry in droppedGlyphTag)
			{
				droppedGlyphs.Add(entry.Key, entry.Value is byte ? (byte)entry.Value != 0 : entry.Value as bool? ?? false);
			}

			BlueMoon = tag.GetBool("blueMoon");

            gennedBandits = tag.GetBool("gennedBandits");
            gennedTower = tag.GetBool("gennedTower");

        }

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				BitsByte flags1 = reader.ReadByte();
				downedScarabeus = flags[0];
				downedAncientFlier = flags[1];
				downedRaider = flags[2];
				downedInfernon = flags[3];
				downedDusking = flags[4];
				downedIlluminantMaster = flags[5];
				downedAtlas = flags[6];
				downedOverseer = flags[7];
				downedReachBoss = flags1[0];
				downedSpiritCore = flags1[1];
			}
			else
			{
				ErrorLogger.Log("Spirit Mod: Unknown loadVersion: " + loadVersion);
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte bosses1 = new BitsByte(downedScarabeus, downedAncientFlier, downedRaider, downedInfernon, downedDusking, downedIlluminantMaster, downedAtlas, downedOverseer);
			BitsByte bosses2 = new BitsByte(downedReachBoss, downedSpiritCore);
			writer.Write(bosses1);
			writer.Write(bosses2);
			BitsByte environment = new BitsByte(BlueMoon);
			writer.Write(environment);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte bosses1 = reader.ReadByte();
			BitsByte bosses2 = reader.ReadByte();
			downedScarabeus = bosses1[0];
			downedAncientFlier = bosses1[1];
			downedRaider = bosses1[2];
			downedInfernon = bosses1[3];
			downedDusking = bosses1[4];
			downedIlluminantMaster = bosses1[5];
			downedAtlas = bosses1[6];
			downedOverseer = bosses1[7];
			downedReachBoss = bosses2[0];
			downedSpiritCore = bosses2[1];
			BitsByte environment = reader.ReadByte();
			BlueMoon = environment[0];
		}

		public override void Initialize()
		{
			BlueMoon = false;
			dayTimeLast = Main.dayTime;
			dayTimeSwitched = false;

			if (NPC.downedQueenBee)
				flierMessage = true;
			else
				flierMessage = false;
			
			if (NPC.downedBoss2 == true)
				gmOre = true;
			else
				gmOre = false;
			
			if (NPC.downedBoss1 == true)
				Magicite = true;
			else
				Magicite = false;

			if (NPC.downedMechBoss3 == true || NPC.downedMechBoss2 == true || NPC.downedMechBoss1 == true)
				spiritBiome = true;
			else
				spiritBiome = false;
			
			if (NPC.downedBoss3)
				starMessage = true;
			else
				starMessage = false;

			if (NPC.downedPlantBoss)
				Thermite = true;
			else
				Thermite = false;
			
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
				essenceMessage = true;
			else
				essenceMessage = false;

			downedScarabeus = false;
			downedAncientFlier = false;
			downedRaider = false;
			downedInfernon = false;
			downedReachBoss = false;
			downedDusking = false;
			downedAtlas = false;
			downedSpiritCore = false;
			downedIlluminantMaster = false;
			downedOverseer = false;
		}
		#region Asteroid
		public static ushort OreRoller(ushort glowstone, ushort none)
		{
			ushort iron = MyWorld.GetNonOre(WorldGen.IronTierOre);
			ushort silver = MyWorld.GetNonOre(WorldGen.SilverTierOre);
			ushort gold = MyWorld.GetNonOre(WorldGen.GoldTierOre);
			
			int OreRoll = Main.rand.Next(1120);
			if (OreRoll < 300)
			{
				return GetNonOre(iron);
			}
			else if (OreRoll < 500)
			{
				return GetNonOre(silver);
			}
			else if (OreRoll < 800)
			{
				return GetNonOre(gold);
			}
			else if (OreRoll < 850)
			{
				return 37; //meteorite
			}
			else if (OreRoll < 1100)
			{
				return glowstone;
			}
			else
			{
				return none;
			}
			return 0;
		}
		public static ushort GetNonOre(ushort ore)
		{
			switch (ore)
			{
				case 7: //copper ==> tin
					return 166;
					break;
				case 166: //tin ==> copper
					return 7;
					break;
				case 6: //iron ==> lead
					return 167;
					break;
				case 167: //lead ==> iron
					return 6;
					break;
				case 9: //silver ==> tungsten
					return 168;
					break;
				case 168: //tungsten ==> silver
					return 9;
					break;
				case 8: //gold ==> platinum
					return 169;
					break;
				case 169: //platinum ==> gold
					return 8;
					break;
			}
			return 0;
		}
		public static void PlaceBlob(int x, int y, float xsize, float ysize, int size, int type, int roundness, bool placewall = false, int walltype = 0)
		{
			int distance = size;
			for (int i = 0; i < 360; i++)
			{
				if ((360 - i) <= ((Math.Abs(size - distance)) / Math.Sqrt(size)) * 50)
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
					int increase = Main.rand.Next(roundness);
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
				drawLine(x, y, x + offsetX, y + offsetY, type, placewall,walltype);
			}
			
		}     
		public static void drawLine(int xpoint1, int ypoint1, int xpoint2, int ypoint2, int type, bool placewall, int walltype)
		{
			int xdist = xpoint2 - xpoint1;
			int ydist = ypoint2 - ypoint1;
			float distance = (float)Math.Sqrt((Math.Abs(xpoint2 - xpoint1)^2) + (Math.Abs(ypoint2 - ypoint1)^2));
			float xDistRelative = (float)xdist / distance;
			float yDistRelative = (float)ydist / distance;
			for (float i = 0; i < distance; i+= (float)0.1)
			{
				int tilePlaceX = xpoint1 + (int)(xDistRelative * i);
				int tilePlaceY = ypoint1 + (int)(yDistRelative * i);
				Tile tile = Main.tile[tilePlaceX, tilePlaceY];
				tile.active(true);
				tile.type = (ushort)type;
				if (i < distance - 1 && placewall)
				{
					tile.wall = (ushort)walltype;
				}
			}
		}
		
		public void PlaceAsteroids(int i, int j)
		{
            bool success = false;
            int attempts = 0;
            while (!success)
            {
                attempts++;
                if (attempts > 1000)
                {
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
                if (Main.maxTilesX == 4200)
                {
                    numberOfAsteroids = 33;
                    numberOfBigs = 1;
                    numberOfOres = 140;
                    numJunkPiles = 15;
                    width = 200;
                    height = 40;
                }
                if (Main.maxTilesX == 6400)
                {
                    numberOfAsteroids = 50;
                    numberOfBigs = 2;
                    numberOfOres = 220;
                    width = 275;
                    numJunkPiles = 21;
                    height = 60;
                }
                if (Main.maxTilesX == 8400)
                {
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
                    PlaceBlob(x, y, xsize, ysize, size, mod.TileType("Asteroid"), 50);
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
                    PlaceBlob(x, y, xsize, ysize, size, mod.TileType("SpaceJunkTile"), 50);
                }
                for (int b = 0; b < numberOfBigs; b++) //big asteroids
                {
                    x = basex + (int)(Main.rand.Next(0 - width, width) / 1.5f);
                    y = basey + Main.rand.Next(0 - height, height);
                    float xsize = (float)(Main.rand.Next(75, 133)) / 100;
                    float ysize = (float)(Main.rand.Next(75, 133)) / 100;
                    int size = Main.rand.Next(11, 17);
                    PlaceBlob(x, y, xsize, ysize, size, mod.TileType("BigAsteroid"), 10, true, mod.WallType("AsteroidWall"));
                }
                for (int b = 0; b < numberOfOres; b++) //ores
                {
                    float distance = (int)(((float)(Main.rand.Next(1000)) / 1000) * (float)Main.rand.Next(radius));
                    int angle = Main.rand.Next(360);
                    int size = Main.rand.Next(2, 5);
                    x = basex + (int)(Main.rand.Next(width) * Math.Sin(angle * (Math.PI / 180))) + Main.rand.Next(-100, 100);
                    y = basey + (int)(Main.rand.Next(height) * Math.Cos(angle * (Math.PI / 180))) + Main.rand.Next(-10, 15);
                    ushort ore = OreRoller((ushort)mod.TileType("Glowstone"), (ushort)mod.TileType("Asteroid"));
                    WorldGen.TileRunner(x, y, Main.rand.Next(2, 10), 2, ore, false, 0f, 0f, false, true);
                }
                success = true;
            }
			
		}
		#endregion
		#region MageTower
		private void PlaceTower(int i, int j, int[,] ShrineArray, int[,] WallsArray, int[,] LootArray) {
			
			for (int y = 0; y < WallsArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x]) {
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, 66); // Stone Slab
								break;	
							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, 144); // Stone Slab
								break;	
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceTile(k, l, 124); // Platforms
								tile.active(true);
								break;
							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, 106); // Platforms
								break;		
							case 8:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, 147); // Stone Slab
								break;	
						}
					}
				}
			}
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x]) {
							case 0:
								break; // no changes
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 7:
								Framing.GetTileSafely(k, l).ClearTile();
								break;		
							case 8:
								WorldGen.PlaceTile(k,l,0);
								tile.active(true);
								break;			
						}
					}
				}
			}
			
			for (int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x]) {
							case 1:
								WorldGen.PlaceTile(k, l, 273); // Stone Slab
								tile.active(true);
								break;	
							case 2:
								WorldGen.PlaceTile(k, l, 19); // Platforms
								tile.active(true);
								break;
							case 3:
								WorldGen.PlaceTile(k, l, 30); // Wood
								tile.active(true);
								break;
							case 6:
								WorldGen.PlaceTile(k, l, 312); // Roofing
								tile.active(true);
								break;					
						}
					}
				}
			}
			for (int y = 0; y < LootArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 1:
								WorldGen.PlaceTile(k, l, 28);  // Pot
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceObject(k, l, mod.TileType("GoblinStatueTile"));
								break;
							case 4:
								WorldGen.PlaceObject(k, l - 1, mod.TileType("ShadowflameStone"));
								break;	
							case 5:
								WorldGen.PlaceObject(k, l, 50); // Book
								break;		
							case 6:
								WorldGen.PlaceObject(k, l, 376); // Crate
								break;	
							case 7:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("GoblinChest"), false, 0); // Gold Chest
								break;	
							case 8:
								WorldGen.PlaceObject(k, l, 13); // Crate
								break;
							case 9:
								WorldGen.PlaceObject(k, l - 1, mod.TileType("GoblinStandardTile")); // Crate
								break;
						}
					}
				}
			}
		}
		public void GenerateTower()
		{
			int[,] TowerShape = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,6,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0},				
				{0,0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0},	
				{0,0,0,0,0,6,6,6,6,0,0,0,0,0,0,0,0,0,0,6,6,6,6,0,0,0,0,0},						
				{0,0,0,0,6,6,6,1,0,0,0,0,0,0,0,0,0,0,0,0,1,6,6,6,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,1,2,2,2,2,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,2,2,2,2,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,3,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},						
				{0,0,0,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,3,3,3,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,2,1,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,1,0,0,2,2,2,1,1,2,2,2,2,2,1,3,3,3,3,3,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,2,0,1,3,3,3,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,2,0,0,1,3,3,0,0,0,0,0},		
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,2,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,3,3,3,3,3,1,2,2,2,2,0,0,0,0,2,2,2,2,1,0,0,0,0,0,0,0},						
				{0,0,0,3,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,3,3,3,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},	
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},	
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,7,7,7,7,0,0},	

			};
			int[,] TowerWallsShape = new int[,]
			{	
			
			
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,8,8,1,1,0,0,8,1,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,8,8,8,8,8,8,8,8,8,4,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,4,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,1,1,1,8,8,4,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,1,1,1,8,8,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,0,8,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,8,8,8,8,8,0,0,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,1,1,8,8,8,8,8,0,8,8,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,4,8,1,8,8,8,8,2,2,8,8,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,1,8,0,8,0,2,2,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,1,8,8,8,8,8,8,8,8,4,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,4,1,1,0,0,0,0,0,0},
				{0,0,0,5,5,5,5,5,4,8,8,8,8,8,8,0,0,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,4,8,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,4,8,2,2,8,8,8,1,1,1,1,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,2,2,8,8,8,1,1,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,0,0,8,8,8,1,8,8,4,5,5,5,5,5,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,0,0,8,8,8,1,8,8,4,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,0,8,8,8,1,8,8,4,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,1,1,8,8,8,8,8,2,2,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,1,1,8,8,8,8,8,2,2,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,1,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0},
				{0,0,0,5,5,5,5,5,4,8,2,2,8,8,8,8,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,4,2,8,2,8,8,8,8,1,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,8,8,8,8,0,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,1,1,1,8,8,8,8,0,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,0,4,1,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,1,4,1,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,1,1,8,8,8,8,8,8,8,1,1,8,4,1,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,1,1,1,1,1,1,8,8,8,1,1,1,8,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,4,0,1,1,4,8,8,8,8,8,8,8,8,8,8,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,4,0,1,1,4,8,8,8,8,8,8,8,8,8,8,8,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,8,8,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0},
	
			};
			int[,] TowerLoot = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,6,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0,0},				
				{0,0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,0,0,0,0,0,0},	
				{0,0,0,0,0,6,6,6,6,0,0,0,0,0,0,0,0,0,0,6,6,6,6,0,0,0,0,0},						
				{0,0,0,0,6,6,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,6,6,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,5,5,8,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,5,5,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},						
				{0,0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,1,0,0,1,0,0,0,0,9,0,0,0},		
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,3,3,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,0,0,0,0,0},		
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,6,0,6,0,0,5,5,5,0,0,0,0,0,5,5,8,0,0,0,0,0,0,0,0,0},	
				{0,0,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},						
				{0,0,0,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},		
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,1,0,0,0,4,0,0,1,0,1,0,1,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},	
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			};
			bool placed = false;
			while (!placed)
			{
				// Select a place in the first 6th of the world
				int towerX = WorldGen.genRand.Next(50, Main.maxTilesX / 6); // from 50 since there's a unaccessible area at the world's borders
				// 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool())
				{
					towerX = Main.maxTilesX - towerX;
				}
				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
				{
					towerY++;
				}
				// If we went under the world's surface, try again
				if (towerY > Main.worldSurface)
				{
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if (tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone && tile.type != TileID.Mud)
				{
					continue;
				}
				// place the tower
				PlaceTower(towerX, towerY - 37, TowerShape, TowerWallsShape, TowerLoot);
                int num = NPC.NewNPC((towerX + 12) * 16, (towerY - 24) * 16, mod.NPCType("BoundRogue"), 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num].homeTileX = -1;
                Main.npc[num].homeTileY = -1;
                Main.npc[num].direction = 1;
                Main.npc[num].homeless = true;
                placed = true;
			}
		}
		#endregion
		#region ReachOutpost
		private void PlaceHideout(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray) 
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
							case 4:
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
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
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile"));
								tile.active(true);
								break;	
							case 2:
                                if (WorldGen.genRand.Next(2) == 0)
                                {
                                    WorldGen.PlaceTile(k, l, mod.TileType("BarkTileTile"));
                                }
								tile.active(true);
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceWall(k, l, 63);
								break;	
							case 2:
								WorldGen.PlaceWall(k, l, mod.WallType("BarkWall"));
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 0:
								break;
							case 3:
								if (Main.rand.Next(2) == 0)
								WorldGen.PlaceObject(k, l - 1, mod.TileType("SkullStick"));
								else
								WorldGen.PlaceObject(k, l - 1, mod.TileType("SkullStickFlip"));
								break;	
							case 4:
								WorldGen.PlaceObject(k, l, 215);
								break;	
							case 5:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("ReachHideoutWoodChest"), false, 0); // Gold Chest
								break;	
							case 6:
								WorldGen.PlaceObject(k, l, 28);  // Pot
								break;	
						}
					}
				}
			}
		}
		public void GenerateHideout()
		{
			
			int[,] HideoutShape = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,2,2,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,1,2,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,1,2,2,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1,2,0,0,0,0,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,01,0,0,0,0,0,0,0,0,0,0,0},
			};
			int[,] HideoutWalls = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,2,2,2,1,1,2,2,1,1,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,1,2,2,1,1,2,2,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,0,2,2,1,1,0,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,2,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,2,2,2,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,1,1,1,2,0,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
			};
			int[,] HideoutLoot = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,2,2,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,1,2,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,0,1,2,2,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,4,0,2,0,0,0,0,0,3,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,3,0,1,1,0,6,0,6,0,0,5,0,0,0,1,1,1,2,0,6,0,6,1,1,1,0,0,0,0,0},
				{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,1,1,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
			};
			bool placed = false;
			while (!placed)
			{
                int towerX = Main.rand.Next(Main.maxTilesX / 6, Main.maxTilesX); ; // from 50 since there's a unaccessible area at the world's borders
                                                                                   // 50% of choosing the last 6th of the world
                if (WorldGen.genRand.NextBool())
                {
                    towerX = Main.maxTilesX - towerX;
                }
                int towerY = 0;
                // We go down until we hit a solid tile or go under the world's surface
                while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
                {
                    towerY++;
                }
                // If we went under the world's surface, try again
                if (towerY > Main.worldSurface)
                {
                    continue;
                }
                Tile tile = Main.tile[towerX, towerY];
                // If the type of the tile we are placing the tower on doesn't match what we want, try again
                if (tile.type != mod.TileType("ReachGrassTile"))
                {
                    continue;
                }
                // place the hideout
                PlaceHideout(towerX, towerY + 1, HideoutShape, HideoutWalls, HideoutLoot);
				placed = true;
			}
		}
		#endregion
		#region Ziggurat
		private void PlaceZiggurat(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray) 
		{
			for (int y = 0; y < BlocksArray.GetLength(0); y++) 
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < BlocksArray.GetLength(0); y++) 
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < LootArray.GetLength(0); y++) 
			{
				for (int x = 0; x < LootArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 0:
								break;
							case 4:
								WorldGen.PlaceObject(k, l, mod.TileType("ScarabIdol"));
								break;	
							case 5:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("GoldScarabChest"), false, 0); 
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
		public void GenerateZiggurat()
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
				int hideoutX = Main.rand.Next(Main.maxTilesX /6, Main.maxTilesX/6*5); // from 50 since there's a unaccessible area at the world's borders
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
				if (tile.type != TileID.Sand && tile.type != TileID.Ebonsand && tile.type != TileID.Crimsand && tile.type != TileID.Sandstone)
				{
					continue;
				}
				// place the hideout
				PlaceZiggurat(hideoutX, hideoutY - 1, ZigguratShape, ZigguratWalls, ZigguratLoot);
				placed = true;
			}
		}
		#endregion
		#region GemStash
		private void PlaceGemStash(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray) 
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < BlocksArray.GetLength(0); y++) 
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
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
			for (int y = 0; y < BlocksArray.GetLength(0); y++) 
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
			for (int y = 0; y < LootArray.GetLength(0); y++) 
			{
				for (int x = 0; x < LootArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 0:
								break;
							case 4:
								WorldGen.PlaceObject(k, l, 376);  // Crate
								break;
							case 5:
								if (Main.rand.NextFloat(1.32f) == 0);
								{
									WorldGen.PlaceTile(k, l, 28);  // Pot
								}
								tile.active(true);
								break;
							case 6:
								int objects;
								if (Main.rand.Next (3) == 0)
								{
									objects = 105;
								}
								else if (Main.rand.Next(2) == 0)
								{
									objects = 16;
								}
								else if (Main.rand.Next(4) == 0)
								{
									objects = 87;
								}
								else if (Main.rand.Next(4) == 0)
								{
									objects = 18;
								}
								else 
								{
									objects = 28;
								}
								WorldGen.PlaceObject(k, l, (ushort)objects);  // Misc
								break;
							case 7:
								WorldGen.PlaceObject(k, l-1, mod.TileType("GemsPickaxeSapphire"));  // Special Pick		
								break;
							case 8:
								if (Main.rand.Next (3) == 0)
								{
									objects = 105;
								}
								else if (Main.rand.Next(2) == 0)
								{
									objects = 16;
								}
								else if (Main.rand.Next(4) == 0)
								{
									objects = 87;
								}
								else if (Main.rand.Next(4) == 0)
								{
									objects = 18;
								}
								else 
								{
									objects = 28;
								}
								WorldGen.PlaceObject(k, l, (ushort)objects);  // Another Misc Obj
								break;
							case 9:
								WorldGen.PlaceObject(k, l-1, mod.TileType("GemsPickaxeRuby"));  // Special Pick		
								break;							
						}
					}
				}
			}	
		}	
		public void GenerateGemStash()
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
			while (!placed)
			{
				int hideoutX = (Main.spawnTileX + Main.rand.Next(-800, 800)); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.spawnTileY + Main.rand.Next(120, 400);
				// place the hideout
				if (WorldGen.genRand.Next(2) == 0)
				{
					PlaceGemStash(hideoutX, hideoutY, StashRoomMain, StashMainWalls, StashMainLoot);
				}
				else
				{
					PlaceGemStash(hideoutX, hideoutY, StashRoomMain1, StashMainWalls, StashMainLoot1);
				}
				if (WorldGen.genRand.Next(2) == 0)
				{
					PlaceGemStash(hideoutX + (Main.rand.Next(-5, 5)), hideoutY - 8, StashRoom1, Stash1Walls, Stash1Loot);
				}
				placed = true;
			}
		}	
		#endregion	
		#region CrateStash
		private void PlaceCrateStash(int i, int j, int[,] BlocksArray, int[,] SecondaryArray, int[,] TertiaryArray) 
		{
			for (int y = 0; y < BlocksArray.GetLength(0); y++) 
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
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
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 5:
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
			for (int y = 0; y < BlocksArray.GetLength(0); y++) 
			{
				for (int x = 0; x < BlocksArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, 1);
								tile.active(true);
								break;	
							case 2:
								Main.tile[k,l].liquid = 255;
								break;	
							case 3:
								WorldGen.PlaceTile(k, l, 30);
								tile.active(true);
								break;	
							case 4:
								WorldGen.PlaceTile(k, l, 19);
								tile.active(true);
								break;	
							case 9:
								if (Main.rand.Next(3) == 0)
								{
								WorldGen.PlaceTile(k, l, 1);
								tile.active(true);
								}	
								break;					
						}
					}
				}
			}
			for (int y = 0; y < SecondaryArray.GetLength(0); y++) 
			{
				for (int x = 0; x < SecondaryArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (SecondaryArray[y, x]) {
							case 0:
								break;	
							case 5:
								if (Main.rand.Next (30) == 0)
								{
									WorldGen.PlaceObject(k, l, 376, true, 2);
								}
								else if (Main.rand.Next(18) == 0)
								{
									WorldGen.PlaceObject(k, l, 376, true, 1);
								}
								else
								{
									WorldGen.PlaceObject(k, l, 376);
								}
								break;	
							case 7:
								{
									WorldGen.PlaceObject(k, l, 33, true, 0);
								}
								break;											
						}
					}
				 }
			}
			for (int y = 0; y < TertiaryArray.GetLength(0); y++) 
			{
				for (int x = 0; x < TertiaryArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (TertiaryArray[y, x]) {
							case 0:
								break;
							case 5:
								WorldGen.PlaceObject(k, l, 376);
								break;			
						}
					}
				 }
			}	
		}	
		public void GenerateCrateStash()
		{
			
			int[,] CrateStashMain = new int[,]
			{
				{0,0,0,8,8,8,8,0,8,8,8,8,8,0,8,8,8,8,8,8,0,0},
				{0,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8},
				{8,8,4,4,4,4,4,4,4,4,4,4,4,4,4,8,8,8,8,8,8,8},
				{0,4,8,8,8,8,8,8,8,8,8,8,8,8,8,4,8,8,8,8,8,8},
				{0,1,9,2,2,2,2,2,2,2,2,2,2,2,2,1,3,3,3,3,3,4},
				{0,1,9,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0}, 
				{0,1,1,1,9,2,2,2,2,2,2,2,2,9,1,1,1,0,0,0,0,0},
				{0,0,1,1,2,2,2,2,2,2,2,2,9,1,1,1,0,0,0,0,0,0},
				{0,0,0,1,1,1,9,9,9,2,9,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
			};
			int[,] CrateStashExtra = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0},
				{0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,5,0,5,7},
				{0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,3,3,3,3,3,0},
				{0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0,0,0,0}, 
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
			};
			int[,] CrateStashThree = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0},
				{0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0},
				{0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0},
				{0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,3,3,3,3,3,0},
				{0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0,0,0,0}, 
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,1,1,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0},
			};
			bool placed = false;
			while (!placed)
			{
				int hideoutX = Main.rand.Next(50, Main.maxTilesX); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.spawnTileY + Main.rand.Next(120, 700);
				Tile tile = Main.tile[hideoutX, hideoutY];
				if (!tile.active() || tile.type != TileID.Stone)
				{
					continue;
				}
				PlaceCrateStash(hideoutX, hideoutY + 2, CrateStashMain, CrateStashExtra, CrateStashThree);
				
				placed = true;
			}
		}
		#endregion
		#region Sepulchre
		private void PlaceSepulchre(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray) 
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
                            case 4:
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, mod.TileType("SepulchreBrick"));
								tile.active(true);
								break;	
							case 2:
                                WorldGen.PlaceTile(k, l, 19, true, false, -1, 12);
                                tile.active(true);
								break;
							case 3:
								WorldGen.PlaceTile(k, l, 51);
								tile.active(true);
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x]) {
							case 0:
								break;
							case 2:
								WorldGen.PlaceWall(k, l, mod.WallType("SepulchreWallTile"));
								break;			
						}
					}
				}
			}
			for (int y = 0; y < LootArray.GetLength(0); y++) { 
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 3:
								if (Main.rand.Next(2) == 0)
                                    WorldGen.PlaceObject(k, l, 50, true, Main.rand.Next(0, 5)); //Books
                                break;	
							case 4:
								WorldGen.PlaceObject(k, l, mod.TileType("SepulchreBannerTile"), true);
								break;	
							case 5:
							int pots;
							if (Main.rand.Next(3) == 0)
							{
								pots = mod.TileType("SepulchrePot1"); 
							}
							else if (Main.rand.Next(2) == 0)
							{
                                pots = mod.TileType("SepulchrePot1"); 
							}
							else
							{
								pots = mod.TileType("SepulchrePot2");
							}
							WorldGen.PlaceTile(k, l, (ushort)pots);
							break;	
							case 6:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("SepulchreChestTile"), false, 0); // Gold Chest
								break;	
							case 7:
								WorldGen.PlaceTile(k, l, 4, true, false, -1, 8);
								break;
							case 8:
                                WorldGen.PlaceObject(k, l, 13, true, Main.rand.Next(0, 2)); //Bottles
                                break;
							case 9:
								WorldGen.PlaceObject(k, l, 187, true, Main.rand.Next(21, 28)); // Crate
								break;
						}
					}
				}
			}	
		}
		public void GenerateSepulchre()
		{	
			int[,] SepulchreRoom1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,1,1,1,1,1,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0,1,1,1,1,0,0},
				{0,0,1,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,0,0},
				{0,0,1,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
				{0,0,1,2,2,2,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,2,2,2,1,0,0},
				{0,0,1,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0},
				{0,0,1,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
				{0,0,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,1,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};
			int[,] SepulchreWalls1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
				{0,0,1,3,3,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,1,0,0},
				{0,0,1,3,3,2,3,2,2,2,2,2,2,2,2,3,3,2,2,2,2,2,2,2,3,2,2,1,0,0},
				{0,0,1,2,2,2,3,3,2,2,2,2,2,2,2,3,3,2,2,2,2,2,2,2,3,2,2,1,0,0},
				{0,0,1,2,2,2,3,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,3,3,2,2,2,2,2,2,2,2,2,2,1,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,3,3,3,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};
			int[,] SepulchreLoot1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
				{0,0,1,4,0,0,0,0,0,0,0,4,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
				{0,0,1,3,3,3,8,8,3,0,0,0,0,0,3,3,5,0,8,3,3,0,0,0,3,3,0,1,0,0},
				{0,0,1,2,2,2,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,2,2,2,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,7,0,1,0,0},
				{0,0,1,0,5,0,0,5,0,5,0,9,0,0,5,0,5,0,5,0,0,0,0,6,0,0,0,1,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};
			int[,] SepulchreRoom2 = new int[,]
			{
				{0,0,0,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
				{0,0,1,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0,0},
				{0,0,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0},
				{0,0,1,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,1,0,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
				{0,0,1,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0,0},				
				{0,0,1,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0}
			};
			int[,] SepulchreWalls2 = new int[,]
			{
				{0,0,0,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,2,2,1,0,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,2,2,1,0,0,0},
				{0,0,1,2,2,3,3,2,2,2,2,3,3,3,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0},
				{0,0,1,2,2,3,3,2,2,2,2,2,3,3,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,2,2,2,3,2,2,2,2,2,2,3,2,2,2,2,3,3,3,3,2,2,2,1,0,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},				
				{0,0,1,2,2,3,3,2,3,3,3,3,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0}
			};
			int[,] SepulchreLoot2 = new int[,]
			{
				{0,0,0,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,0},
				{0,0,1,3,3,0,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,3,3,1,0,0,0},
				{0,0,1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0,0},
				{0,0,1,3,3,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0},
				{0,0,1,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,0,3,3,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,1,0,0,0},
				{0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0},
				{0,0,1,3,3,0,0,0,0,0,0,0,7,0,0,7,0,0,0,0,0,0,0,3,3,1,0,0,0},				
				{0,0,1,3,3,3,0,0,5,0,5,0,0,6,0,0,5,0,9,0,5,0,3,3,3,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0}
			};
            int[,] SepulchreRoom3 = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,0,0,0,0,1,1,1,1,0,1,1,1,1,1,0,0,0,0,0,1,1,1,0,0},
                {0,0,1,3,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,3,1,0,0},
                {0,0,1,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0},
                {0,0,1,3,3,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,0,0},
                {0,0,1,3,0,0,0,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,1,0,0},
                {0,0,1,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,3,1,0,0},
                {0,0,1,3,3,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0},
                {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,1,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
            };
            int[,] SepulchreWalls3 = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,1,0,0},
                {0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,1,0,0},
                {0,0,1,3,3,3,2,2,2,3,2,2,2,3,3,2,2,2,2,3,2,2,2,2,3,2,2,1,0,0},
                {0,0,1,3,3,2,2,2,2,2,2,2,2,3,3,3,3,2,2,3,2,2,2,2,3,2,2,1,0,0},
                {0,0,1,2,2,2,2,2,2,2,2,3,3,2,2,3,3,2,2,2,2,2,2,2,3,2,2,1,0,0},
                {0,0,1,2,2,2,2,3,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
                {0,0,1,3,3,2,2,2,2,2,2,2,2,2,2,3,3,2,2,2,2,2,2,2,2,2,2,1,0,0},
                {0,0,1,2,3,3,2,2,2,2,2,2,2,3,3,3,2,2,2,2,2,2,2,2,2,2,2,1,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0,0},
            };
            int[,] SepulchreLoot3 = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
                {0,0,1,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,4,0,0,0,1,0,0},
                {0,0,1,0,0,0,0,0,8,3,3,3,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,1,3,3,3,8,8,3,8,8,3,8,8,3,3,5,0,8,3,3,0,0,0,3,3,0,1,0,0},
                {0,0,1,2,2,2,2,2,2,0,7,0,0,0,2,2,2,2,2,2,2,0,7,0,2,2,2,1,0,0},
                {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0},
                {0,0,1,0,0,5,0,5,0,5,0,9,0,0,5,0,0,0,5,0,0,0,0,5,0,0,5,1,0,0},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
            };
            int hideoutX = Main.rand.Next(0, Main.maxTilesX); // from 50 since there's a unaccessible area at the world's borders
			int hideoutY = Main.spawnTileY + Main.rand.Next(200, Main.maxTilesY);
			if (Main.rand.Next(2) == 0)
			{
                PlaceSepulchre(hideoutX, hideoutY + 9, SepulchreRoom3, SepulchreWalls3, SepulchreLoot3);

                PlaceSepulchre(hideoutX + Main.rand.Next(-8, 8), hideoutY, SepulchreRoom1, SepulchreWalls1, SepulchreLoot1);
            }
			else
			{

                PlaceSepulchre(hideoutX, hideoutY - 10, SepulchreRoom3, SepulchreWalls3, SepulchreLoot3);

                PlaceSepulchre(hideoutX, hideoutY + 1, SepulchreRoom2, SepulchreWalls2, SepulchreLoot2);
			}
		}
		#endregion 
		#region BanditHideout
		private void PlaceBanditHideout(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray) 
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++) 
			{
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				 {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
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
							case 4:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 5:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;			
						}
					}
				}
			}
			for (int y = 0; y < LootArray.GetLength(0); y++) { 
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 4:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 5:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 6:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 7:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;		
							case 8:					
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
									
								break;
							case 9:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 10:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 11:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 12:
								WorldGen.PlaceObject(k, l, 15);
								break;
							case 13:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 14:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 15:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 16:
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
					if (WorldGen.InWorld(k, l, 30)){
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
							case 4:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 5:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
								break;	
							case 6:
								WorldGen.KillWall(k, l);
								Framing.GetTileSafely(k, l).ClearTile();
                                break;
                            case 9:
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x]) {
							case 0:
								break;
							case 4:
								WorldGen.PlaceWall(k, l, 4);
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
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, 30);
								tile.active(true);
								break;	
							case 2:
								WorldGen.PlaceTile(k, l, 38);
								tile.active(true);
								break;	
							case 3:
								WorldGen.PlaceTile(k, l, 124);
								tile.active(true);
								break;	
							case 4:
								WorldGen.PlaceTile(k, l, 213);
								tile.active(true);
								break;	
							case 5:
								WorldGen.PlaceTile(k, l, 19, true, false, -1, 12);
								tile.active(true);
								break;	
							case 6:
								WorldGen.PlaceWall(k, l, 106);
								break;	
							case 7:
								WorldGen.PlaceTile(k, l, 19, true, false, -1, 0);
								tile.active(true);
								break;	
						
						}
					}
				}
			}
			for (int y = 0; y < LootArray.GetLength(0); y++) { 
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 4:
								WorldGen.PlaceObject(k, l, 17, true, 0);
								break;	
							case 5:
								WorldGen.PlaceTile(k, l, 28);
								break;
							case 6:
								WorldGen.PlaceTile(k, l, 10, true, false, -1, 13);
								break;	
							case 7:
								WorldGen.PlaceObject(k, l, 240, true, Main.rand.Next(44, 45)); // Crate
								break;	
							case 8:					
								WorldGen.PlaceObject(k, l, 94); 
								break;
							case 9:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("BanditChest"), false, 0); // Gold Chest
								break;
							case 10:
								WorldGen.PlaceObject(k, l, 42, true, 6);
								break;
							case 11:
								WorldGen.PlaceObject(k, l, 215);
								break;
							case 12:
								WorldGen.PlaceObject(k, l, 15);
								break;
							case 13:
								WorldGen.PlaceObject(k, l, 187, true, 28); // Crate
								break;	
							case 14:
								WorldGen.PlaceObject(k, l, 187, true, 26); // Crate
								break;	
							case 15:
								WorldGen.PlaceObject(k, l, 187, true, 27); // Crate
								break;	
							case 16:
								WorldGen.PlaceObject(k, l, 187, true, 23); // Crate
								break;		
							case 17:
								WorldGen.PlaceObject(k, l, 376); // Crate
								break;										
						}
					}
				}
			}
		}	
		public void GenerateBanditHideout()
		{	
			int[,] BanditTiles = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,2,2,2,2,2,2,2,2,5,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,0,0,0,0,0,0,0,2,2,5,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,3,0,0,0,0,0,0,0,0,3,2,2,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,3,0,0,0,0,0,0,0,0,3,0,2,2,5,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,0,3,0,0,0,0,0,0,0,0,3,0,0,2,2,5,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,0,0,0,3,0,0,0,0,0,0,0,0,3,0,0,0,2,2,5,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,1,1,1,1,1,5,5,5,5,5,5,5,5,1,1,1,1,1,2,2,5},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,6,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,6,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,4,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,6,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,6,4,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,6,6,3,6,6,6,6,6,6,6,6,6,6,6,6,6,6,3,6,6,4,9},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,6,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,6,4,9},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,0,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,4,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,4,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,9,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,9,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,9,9},
				{0,0,0,0,0,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,9,9},
				{0,0,0,6,6,6,6,6,6,6,6,6,9,9,9,9,9,9,9,9,9,6,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,6,9,9,9},
				{0,0,0,2,1,1,1,1,1,1,1,2,7,7,7,7,7,7,7,7,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
			};
			int[,] BanditWalls = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,2,2,2,2,2,2,2,2,5,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,2,2,5,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,2,2,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,4,4,2,2,5,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,5,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,2,2,5,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,2,2,1,1,1,1,1,4,4,4,4,4,4,4,4,1,1,1,1,1,2,2,5},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0},
			};
			int[,] BanditLoot = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,7,0,0,0,0,7,0,0,2,2,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,17,0,5,0,0,5,0,5,0,5,0,5,0,0,2,2,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,2,2,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,10,0,0,0,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,8,12,0,0,4,0,0,9,0,0,3,0,0,6,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,11,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,16,0,0,15,0,0,11,0,0,0,14,0,0,0,0,13,0,0},
				{0,0,0,2,1,1,1,1,1,1,1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			};
		
			bool placed = false;
			while (!placed)
			{
                int towerX = WorldGen.genRand.Next(50, Main.maxTilesX / 4); 
                if (WorldGen.genRand.NextBool())
                {
                    towerX = Main.maxTilesX - towerX;
                }
                int towerY = 0;
                // We go down until we hit a solid tile or go under the world's surface
                while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
                {
                    towerY++;
                }
                // If we went under the world's surface, try again
                if (towerY > Main.worldSurface)
                {
                    continue;
                }
                Tile tile = Main.tile[towerX, towerY];
                // If the type of the tile we are placing the tower on doesn't match what we want, try again
                if (tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone && tile.type != TileID.SnowBlock)
                {
                    continue;
                }
                PlaceBanditHideout(towerX, towerY - 22, BanditTiles, BanditWalls, BanditLoot);
				int num = NPC.NewNPC((towerX + 31) * 16, (towerY - 20) * 16, mod.NPCType("BoundRogue"), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num].homeTileX = -1;
				Main.npc[num].homeTileY = -1;
				Main.npc[num].direction = 1;
				Main.npc[num].homeless = true;

				placed = true;
			}
		}
        #endregion
        #region ReachMicros
        private void PlaceAltar(int i, int j, int[,] BlocksArray, int[,] LootArray) {
			
			for (int y = 0; y < BlocksArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break; // no changes
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile"));
								tile.active(true);
								break;
							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}
			
			for (int y = 0; y < LootArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 2:
								WorldGen.PlaceObject(k, l-4, mod.TileType("SkeletonTree"));
								break;
							case 3:
								if (Main.rand.Next(2) == 0)
								WorldGen.PlaceObject(k, l - 1, mod.TileType("SkullStick"));
								else
								WorldGen.PlaceObject(k, l - 1, mod.TileType("SkullStickFlip"));
								break;	
							case 4:
								WorldGen.PlaceObject(k, l - 1, mod.TileType("BoneAltar")); // Campfire
								break;
						}
					}
				}
			}
		}
		public void GenerateAltar()
		{
			int[,] TileShape = new int[,]
			{
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
			};
			int[,] DecorShape = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,3,0,0,2,0,0,0,0,0,0,0,4,0,3,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
			};
			bool placed = false;
			while (!placed)
			{
				// Select a place in the first 6th of the world
				int towerX = Main.rand.Next(Main.maxTilesX /6, Main.maxTilesX); ; // from 50 since there's a unaccessible area at the world's borders
				// 50% of choosing the last 6th of the world
				if (WorldGen.genRand.NextBool())
				{
					towerX = Main.maxTilesX - towerX;
				}
				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
				{
					towerY++;
				}
				// If we went under the world's surface, try again
				if (towerY > Main.worldSurface)
				{
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if (tile.type != mod.TileType("ReachGrassTile"))
				{
					continue;
				}
				// place the tower
				PlaceAltar(towerX, towerY, TileShape, DecorShape);
				int num = NPC.NewNPC((towerX) * 16, (towerY - 10) * 16, mod.NPCType("ReachmanPassive"), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num].homeTileX = -1;
				Main.npc[num].homeTileY = -1;
				Main.npc[num].direction = 1;
				Main.npc[num].homeless = true;
				int num1 = NPC.NewNPC((towerX + 10) * 16, (towerY - 10) * 16, mod.NPCType("ReachmanPassive"), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num1].homeTileX = -1;
				Main.npc[num1].homeTileY = -1;
				Main.npc[num1].direction = -1;
				Main.npc[num1].homeless = true;
                int num2 = NPC.NewNPC((towerX + 5) * 16, (towerY - 10) * 16, mod.NPCType("BoundAdventurer"), 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num2].homeTileX = -1;
                Main.npc[num2].homeTileY = -1;
                Main.npc[num2].direction = -1;
                Main.npc[num2].homeless = true;
                placed = true;
			}
		}
        private void PlaceUndergroundAltar(int i, int j, int[,] BlocksArray, int[,] LootArray)
        {

            for (int y = 0; y < BlocksArray.GetLength(0); y++)
            { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
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
                                break; // no changes
                            case 1:
                                Framing.GetTileSafely(k, l).ClearTile();
                                WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile"));
                                tile.active(true);
                                break;
                            case 2:
                                Framing.GetTileSafely(k, l).ClearTile();
                                break;
                        }
                    }
                }
            }

            for (int y = 0; y < LootArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
                for (int x = 0; x < LootArray.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (LootArray[y, x])
                        {
                            case 2:
                                WorldGen.PlaceObject(k, l - 4, mod.TileType("SkeletonTree"));
                                break;
                            case 3:
                                if (Main.rand.Next(2) == 0)
                                    WorldGen.PlaceObject(k, l - 1, mod.TileType("SkullStick"));
                                else
                                    WorldGen.PlaceObject(k, l - 1, mod.TileType("SkullStickFlip"));
                                break;
                            case 4:
                                WorldGen.PlaceObject(k, l - 1, mod.TileType("BoneAltar")); // Campfire
                                break;
                        }
                    }
                }
            }
        }
        public void GenerateUndergroundAltar()
        {
            int[,] TileShape = new int[,]
            {
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0},
                {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            };
            int[,] DecorShape = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,3,0,0,2,0,0,0,0,0,0,0,4,0,3,0,0},
                {0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            };
            bool placed = false;
            while (!placed)
            {
                int towerX = (int)Main.rand.Next(50, Main.maxTilesX); ; // from 50 since there's a unaccessible area at the world's borders
                int towerY = (int)Main.rand.Next((int)Main.worldSurface + 20, Main.maxTilesY);
                Tile tile = Main.tile[towerX, towerY];
                // If the type of the tile we are placing the tower on doesn't match what we want, try again
                if (tile.type != mod.TileType("ReachGrassTile"))
                {
                    continue;
                }
                {
                    PlaceUndergroundAltar(towerX, towerY + Main.rand.Next(-70, 0), TileShape, DecorShape);
                }
                placed = true;
            }
        }
        private void PlaceGlowRoom(int i, int j, int[,] BlocksArray, int[,] LootArray) {
			
			for (int y = 0; y < BlocksArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break; // no changes
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceTile(k, l, mod.TileType("BlastStone"));
								tile.active(true);
								WorldGen.KillWall(k, l);
								break;
							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
                                if (WorldGen.genRand.Next(2) == 0)
                                {
                                    WorldGen.KillWall(k, l);
                                    WorldGen.PlaceWall(k, l, 207);
                                }
                                break;
						}
					}
				}
			}
			for (int y = 0; y < BlocksArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x]) {
							case 0:
								break; // no changes
							case 1:

								break;
							case 2:
								break;
						}
					}
				}
			}
			for (int y = 0; y < LootArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (LootArray[y, x]) {
							case 0:
								break; // no changes
							case 3:
								if (Main.rand.Next (2) == 0)
								{
								WorldGen.PlaceTile(k, l, ModContent.TileType<Tiles.Ambient.ReachMicros.GlowGrass>());
								}
								else
								{
								WorldGen.PlaceTile(k, l, ModContent.TileType<Tiles.Ambient.ReachMicros.GlowGrass2>());
								}
								break;
							case 4:
								WorldGen.PlaceChest(k, l, (ushort)mod.TileType("ReachChest"), false, 0); // Gold Chest
								break;
							case 5:
								if (Main.rand.Next (2) == 0)
								{
								WorldGen.PlaceTile(k, l, ModContent.TileType<Tiles.Ambient.ReachMicros.GlowGrass>());
								}
								else
								{
								WorldGen.PlaceTile(k, l, ModContent.TileType<Tiles.Ambient.ReachMicros.GlowGrass2>());	
								}
								
								break;
							case 6:
								WorldGen.PlaceTile(k, l, ModContent.TileType<Tiles.Ambient.ReachMicros.GlowVine>(), true);
								break;
						}
					}
				}
			}
		}
		public void GenerateGlowRoom()
		{	
			int[,] GlowRoom1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0,0},
				{0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},		
			};
			int[,] GlowRoom2 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,2,2,2,2,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},		
			};
			int[,] GlowLoot1 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,2,2,2,2,2,2,1,0,0,0,0,0,0},
				{0,0,0,0,1,1,2,2,6,2,6,2,2,2,2,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,5,2,3,2,2,4,2,2,5,2,2,3,2,1,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},		
			};
			int[,] GlowLoot2 = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,1,1,2,6,2,2,2,6,2,2,2,2,1,1,0,0,0,0,0},
				{0,0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0,0},
				{0,0,0,0,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,0,0,0},
				{0,0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,0,0,0},
				{0,0,0,1,2,2,5,2,3,2,2,2,4,2,2,2,5,2,3,1,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},			
			};
			bool placed = false;
			while (!placed)
			{
				// Select a place in the first 6th of the world
				int towerX = (int)Main.rand.Next(50, Main.maxTilesX); ; // from 50 since there's a unaccessible area at the world's borders
				int towerY = (int)Main.rand.Next((int)Main.worldSurface + 10, Main.maxTilesY);
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if (tile.type != mod.TileType("ReachGrassTile"))
				{
					continue;
				}
				if (WorldGen.genRand.Next(2) == 0)
				{
					PlaceGlowRoom(towerX, towerY, GlowRoom1, GlowLoot1);
				}
				else
				{
					PlaceGlowRoom(towerX, towerY, GlowRoom2, GlowLoot2);
				}
				placed = true;
			}
		}
        private void PlaceGlowGrave(int i, int j, int[,] BlocksArray)
        {
            for (int y = 0; y < BlocksArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
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
                                tile.ClearTile();
                                WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile"));
                                tile.active(true);
                                break;
                            case 3:
                                tile.ClearTile();
                                WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile"));
                                tile.active(true);
                                break;
                            case 5:
                                tile.ClearTile();
                                break;
                        }
                    }
                }
            }
            for (int y = 0; y < BlocksArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
                for (int x = 0; x < BlocksArray.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (BlocksArray[y, x])
                        {
                            case 1:
                                WorldGen.PlaceTile(k, l, mod.TileType("ReachGrassTile"));
                                tile.active(true);
                                break;
                            case 2:
                                Framing.GetTileSafely(k, l).ClearTile();
                                WorldGen.PlaceObject(k, l, 85, true, Main.rand.Next(0, 5));
                                int num256 = Sign.ReadSign(k, l, true);
                                if (num256 >= 0)
                                {
                                    switch (Main.rand.Next(6))
                                    {
                                        case 0:
                                            Sign.TextSign(num256, "Servants to the Briar for all eternity, the Wraiths are ancient constructs of vine and blood held together by the crude spells of the savages. Bound to the will of the savages, they can only be freed should the last of their mortal body be destroyed.");
                                            break;
                                        case 1:
                                            Sign.TextSign(num256, "The text is too faded to read.");
                                            break;
                                        case 2:
                                            Sign.TextSign(num256, "An untamable land engulfed by nature, the Briar is surrounded by an aura of mystery. The maw of the Briar devours all who dare defile it, leaving them to sink unto madness as their cries are drowned out by the everlasting downpour.");
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
                            case 4:
                                tile.ClearTile();
                                if (Main.rand.Next(3) == 0)
                                {
                                    WorldGen.PlaceTile(k, l, mod.TileType("GlowGrass"));
                                }
                                else if (Main.rand.Next(3) == 0)
                                {
                                    WorldGen.PlaceTile(k, l, mod.TileType("GlowGrass2"));
                                }
                                else
                                {
                                    WorldGen.PlaceTile(k, l, mod.TileType("GlowGrass3"));
                                }
                                break;

                        }
                    }
                }
            }
        }

        public void GenerateGlowGrave()
        {
            int[,] GraveShape1 = new int[,]
            {
                {0,0,3,3,1,1,1,1,1,1,1,3,3,0,0},
                {0,0,3,3,1,5,5,5,5,5,1,3,3,0,0},
                {0,3,3,1,1,5,5,5,5,5,1,1,3,3,0},
                {0,3,1,1,5,5,5,5,5,5,5,5,1,3,0},
                {0,3,1,1,5,5,5,5,5,5,5,5,1,3,0},
                {0,3,1,5,5,5,5,5,5,5,5,5,1,3,0},
                {0,3,1,5,5,5,5,5,5,5,5,5,1,3,0},
                {0,3,1,1,5,4,5,2,5,5,5,4,1,3,0},
                {0,3,3,1,1,1,1,1,1,1,1,1,3,3,0},
                {0,0,3,3,3,3,3,3,3,3,3,3,3,0,0},
            };
            bool placed = false;
            while (!placed)
            {
                int hideoutX = Main.rand.Next(50, Main.maxTilesX); // from 50 since there's a unaccessible area at the world's borders
                int hideoutY = Main.spawnTileY + Main.rand.Next(120, 700);
                Tile tile = Main.tile[hideoutX, hideoutY];
                if (!tile.active() || tile.type != mod.TileType("ReachGrassTile"))
                {
                    continue;
                }

                {
                    PlaceGlowGrave(hideoutX, hideoutY, GraveShape1);
                }
                placed = true;
            }
        }
        #endregion
        #region EvilMicros
        private void PlaceBoneSpike(int i, int j, int[,] BlocksArray)
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
                                tile.ClearTile();
                                tile.type = TileID.FleshBlock;
                                tile.active(true);
                                break;
                            case 2:
                                tile.ClearTile();
                                tile.type = TileID.BoneBlock;
                                tile.active(true);
                                break;
                            case 3:
                                tile.ClearTile();
                                tile.type = TileID.BoneBlock;
                                tile.slope(2);
                                tile.active(true);
                                break;
                            case 4:
                                tile.ClearTile();
                                tile.type = TileID.BoneBlock;
                                tile.slope(1);
                                tile.active(true);
                                break;
                            case 5:
                                tile.ClearTile();
                                tile.type = TileID.BoneBlock;
                                tile.slope(4);
                                tile.active(true);
                                break;
                            case 6:
                                tile.ClearTile();
                                tile.type = TileID.Dirt;
                                tile.active(true);
                                if (!Main.tile[k, l - 1].active())
                                {
                                    WorldGen.PlaceTile(k, l, TileID.FleshGrass);
                                }
                                break;
                            case 7:
                                tile.ClearTile();
                                tile.type = TileID.BoneBlock;
                                tile.slope(7);
                                tile.active(true);
                                break;
                            case 8:
                                tile.ClearTile();
                                tile.type = TileID.BoneBlock;
                                tile.slope(5);
                                tile.active(true);
                                break;
                            case 9:
                                tile.ClearTile();
                                WorldGen.PlaceWall(k, l, 77);
                                break;
                            case 10:
                                tile.ClearTile();
                                WorldGen.PlaceWall(k, l, 77);
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
                            case 9:
                                tile.ClearTile();
                                WorldGen.PlaceChest(k, l, (ushort)mod.TileType("FleshSpikeChest_Tile"), false, 0);
                                break;
                        }
                    }
                }
            }
        }
        public void GenerateBoneSpike()
        {

            int[,] SpikeShape1 = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,3,2,0,0,0,0,0,2,4,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,2,2,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,3,2,2,0,0,0,0,0,0,2,2,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,2,2,4,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,3,2,2,7,0,0,0,0,0,0,2,2,2,4,0,0,0,0,0,0,0},
                {0,0,0,0,0,3,2,2,2,0,0,0,1,1,0,0,0,2,2,2,0,0,0,0,0,0,0},
                {0,0,0,0,0,2,2,2,2,0,1,1,1,1,1,1,0,2,2,2,2,0,0,0,0,0,0},
                {6,6,1,1,2,2,2,2,2,1,1,1,10,10,1,1,0,0,2,2,2,4,0,0,0,0,0},
                {6,1,1,1,2,2,2,2,1,1,1,1,9,10,1,1,1,1,2,2,2,2,0,0,0,0,0},
                {6,6,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,1,1,6,6},
                {6,6,6,1,1,1,1,1,1,6,1,1,1,1,1,1,6,6,1,1,1,1,1,6,6,6,6},
                {6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,0},
            };
            int[,] SpikeShape2 = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,3,2,0,0,0,0,0,4,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,3,2,2,0,0,0,0,0,2,4,0,0,0,0,0,0},
                {0,0,0,0,0,0,3,2,2,0,0,0,0,0,0,0,2,4,0,0,0,0,0},
                {0,0,0,0,0,0,2,2,2,0,1,1,1,1,0,0,2,2,4,0,0,0,0},
                {0,0,0,0,0,0,2,2,2,2,1,10,10,1,1,0,2,2,2,4,0,1,1},
                {0,0,0,0,0,3,2,2,2,2,1,9,10,1,1,1,2,2,2,2,1,1,1},
                {6,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,2,1,1,1,6},
                {6,6,1,1,1,2,1,1,1,6,6,6,6,6,6,6,6,1,1,1,6,6,0},
                {0,6,6,6,1,1,1,1,6,6,6,6,6,6,6,6,6,6,6,6,6,0,0},
                {0,0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,0,0},
                {0,0,6,6,6,6,0,0,0,0,6,6,6,6,6,6,6,6,0,0,0,0,0},

            };
            int[,] SpikeShape3 = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,3,0,0,0,0,4,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,3,2,0,0,0,0,2,4,0,0,0,0,0,0},
                {0,0,0,0,3,2,2,7,0,0,0,0,0,2,4,0,0,0,0,0},
                {0,1,1,3,2,2,2,0,0,0,0,0,0,2,2,4,0,0,0,0},
                {6,1,1,2,2,2,2,1,1,6,0,0,0,2,2,2,4,0,1,1},
                {6,6,1,1,2,2,1,1,6,6,6,1,1,2,2,2,2,1,1,1},
                {0,6,6,1,1,1,1,6,6,6,6,1,1,1,1,2,1,1,1,6},
                {0,6,6,6,6,6,6,6,6,6,6,6,6,6,6,1,1,6,6,0},
            };

            bool placed = false;
            int counter = 0;
            while (!placed)
            {
                counter++;
                if (counter >= 300)
                {
                    placed = true;
                }
                // Select a place in the first 6th of the world
                int towerX = WorldGen.genRand.Next(300, Main.maxTilesX); // from 50 since there's a unaccessible area at the world's borders
                                                                         // 50% of choosing the last 6th of the world
                if (WorldGen.genRand.NextBool())
                {
                    towerX = Main.maxTilesX - towerX;
                }
                int towerY = 0;
                // We go down until we hit a solid tile or go under the world's surface
                while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
                {
                    towerY += 2;
                }
                // If we went under the world's surface, try again
                if (towerY > Main.worldSurface)
                {
                    continue;
                }
                Tile tile = Main.tile[towerX, towerY];

                if (tile.type != TileID.FleshGrass)
                {
                    continue;
                }
                // place the tower
                if (Main.rand.Next(2) == 0)
                {
                    PlaceBoneSpike(towerX, towerY - 4, SpikeShape1);
                }
                else if (Main.rand.Next(3) == 0)
                {
                    PlaceBoneSpike(towerX, towerY - 1, SpikeShape3);
                }
                else
                {
                    PlaceBoneSpike(towerX, towerY - 6, SpikeShape2);
                }
                placed = true;
            }
        }
        private void PlaceCorruptHole(int i, int j, int[,] BlocksArray)
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
                                tile.ClearTile();
                                tile.type = TileID.Ebonstone;
                                tile.active(true);
                                break;
                            case 2:
                                tile.ClearTile();
                                tile.type = TileID.Ebonsand;
                                tile.active(true);
                                break;
                            case 3:
                                tile.ClearTile();
                                tile.type = TileID.CorruptSandstone;
                                tile.active(true);
                                break;
                            case 4:
                                tile.ClearTile();
                                WorldGen.PlaceWall(k, l, 3);
                                break;
                            case 5:
                                tile.ClearTile();
                                WorldGen.PlaceWall(k, l, 3);
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
                            case 5:
                                WorldGen.PlaceChest(k, l, (ushort)mod.TileType("EbonChest_Tile"), false, 0);
                                break;
                        }
                    }
                }
            }
        }
        public void GenerateCorruptHole()
        {
            int[,] HoleShape1 = new int[,]
            {
                {0,0,0,0,0,3,0,0,0,0,0,0,3,0,0,0,0},
                {0,0,0,3,1,2,1,1,3,3,1,1,2,1,3,0,0},
                {0,3,1,2,2,1,1,1,4,4,1,1,1,2,2,1,3},
                {0,2,2,2,2,2,1,4,4,4,4,1,2,2,2,2,2},
                {2,2,2,2,1,1,1,4,5,4,4,1,1,1,1,2,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            };
            int[,] HoleShape2 = new int[,]
            {
                {0,0,0,0,0,0,0,3,0,0,0,0,0,3,0,0,0,0,0,0},
                {0,0,0,0,3,0,1,2,2,2,2,2,2,2,0,3,0,0,0,0},
                {0,0,3,1,2,2,2,2,1,3,3,1,2,2,2,2,0,3,0,0},
                {3,0,2,2,1,1,1,1,1,4,4,1,1,1,1,1,2,2,0,0},
                {2,2,2,2,2,2,2,1,4,4,4,4,1,2,2,2,2,2,2,3},
                {2,2,1,1,1,2,2,1,4,5,4,4,1,2,2,1,1,1,2,2},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},
            };
            int[,] HoleShape3 = new int[,]
            {
                {0,0,0,0,0,3,0,0,0,0,0,0,3,0,0,0,0},
                {0,0,0,3,1,2,1,1,3,3,1,1,2,1,3,0,0},
                {0,3,1,2,2,1,1,1,4,4,1,1,1,2,2,1,3},
                {0,2,2,2,2,2,1,4,4,4,4,1,2,2,2,2,2},
                {2,2,2,2,1,1,1,4,4,4,4,1,1,1,1,2,2},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
            };
            bool placed = false;
            int counter = 0;
            while (!placed)
            {
                counter++;
                if (counter >= 300)
                {
                    placed = true;
                }
                // Select a place in the first 6th of the world
                int towerX = WorldGen.genRand.Next(300, Main.maxTilesX); // from 50 since there's a unaccessible area at the world's borders
                                                                         // 50% of choosing the last 6th of the world
                if (WorldGen.genRand.NextBool())
                {
                    towerX = Main.maxTilesX - towerX;
                }
                int towerY = 0;
                // We go down until we hit a solid tile or go under the world's surface
                while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
                {
                    towerY += 2;
                }
                // If we went under the world's surface, try again
                if (towerY > Main.worldSurface)
                {
                    continue;
                }
                Tile tile = Main.tile[towerX, towerY];

                if (tile.type != TileID.CorruptGrass)
                {
                    continue;
                }
                // place the tower
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        PlaceCorruptHole(towerX, towerY + 2, HoleShape3);
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        PlaceCorruptHole(towerX, towerY + 2, HoleShape2);
                    }
                    else
                    {
                        PlaceCorruptHole(towerX, towerY + 2, HoleShape1);
                    }
                }
                placed = true;
            }
        }
        #endregion
        #region SurfaceMicros
        private void PlaceCampsite(int i, int j, int[,] BlocksArray)
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
                                WorldGen.PlaceTile(k, l, mod.TileType("TentOpposite"));
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
        public void GenerateCampsite()
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
            while (!placed)
            {
                // Select a place in the first 6th of the world
                int towerX = Main.spawnTileX + Main.rand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
                                                                         // 50% of choosing the last 6th of the world
                if (WorldGen.genRand.NextBool())
                {
                    towerX = Main.maxTilesX - towerX;
                }
                int towerY = 0;
                // We go down until we hit a solid tile or go under the world's surface
                while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
                {
                    towerY++;
                }
                // If we went under the world's surface, try again
                if (towerY > Main.worldSurface)
                {
                    continue;
                }
                Tile tile = Main.tile[towerX, towerY];
                // If the type of the tile we are placing the tower on doesn't match what we want, try again
                if (tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone)
                {
                    continue;
                }
                // place the tower
                PlaceCampsite(towerX, towerY + 1, CampShape1);
                placed = true;
            }
        }
        private void PlaceGrave(int i, int j, int[,] ShrineArray, int[,] WallArray)
        {

            for (int y = 0; y < WallArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
                for (int x = 0; x < WallArray.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (WallArray[y, x])
                        {
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
                                if (Main.rand.Next(2) == 0)
                                {
                                    WorldGen.PlaceWall(k, l, 106);
                                }
                                else
                                {
                                    WorldGen.PlaceWall(k, l, 63);
                                }
                                break;
                            case 4:
                                Framing.GetTileSafely(k, l).ClearTile();
                                WorldGen.KillWall(k, l);
                                {
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
            for (int y = 0; y < ShrineArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
                for (int x = 0; x < ShrineArray.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (ShrineArray[y, x])
                        {
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
            for (int y = 0; y < ShrineArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
                for (int x = 0; x < ShrineArray.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (ShrineArray[y, x])
                        {
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
            for (int y = 0; y < ShrineArray.GetLength(0); y++)
            { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
                for (int x = 0; x < ShrineArray.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (ShrineArray[y, x])
                        {
                            case 2:
                                int num256 = Sign.ReadSign(k, l, true);
                                if (num256 >= 0)
                                {
                                    switch (Main.rand.Next(6))
                                    {
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

        public void GenerateGrave()
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
            while (!placed)
            {
                // Select a place in the first 6th of the world
                int towerX = Main.spawnTileX + Main.rand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
                                                                                                // 50% of choosing the last 6th of the world
                if (WorldGen.genRand.NextBool())
                {
                    towerX = Main.maxTilesX - towerX;
                }
                int towerY = 0;
                // We go down until we hit a solid tile or go under the world's surface
                while (!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface)
                {
                    towerY++;
                }
                // If we went under the world's surface, try again
                if (towerY > Main.worldSurface)
                {
                    continue;
                }
                Tile tile = Main.tile[towerX, towerY];
                // If the type of the tile we are placing the tower on doesn't match what we want, try again
                if (tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone)
                {
                    continue;
                }
                // place the tower
                PlaceGrave(towerX, towerY + 2, GraveShape, GraveWalls);
                placed = true;
            }
        }
        #endregion
        private void Vines(GenerationProgress progress)
        {
            progress.Message = "Spreading vines in the Briar...";
            Tile tile;
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 1.2f) * 6E-05); k++)
            {
                bool placeSuccessful = false;
                int tileToPlace;
                if (WorldGen.genRand.Next(16) == 0)
                {
                    tileToPlace = TileType<Tiles.Ambient.ReachMicros.Vine2>();
                }
                else
                {
                    tileToPlace = TileType<Tiles.Ambient.ReachMicros.Vine1>();
                }
                while (!placeSuccessful)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)Main.worldSurface - 200, Main.maxTilesY);
                    tile = Main.tile[x, y];
                    if (tile.type == mod.TileType("ReachGrassTile"))
                    {
                        {
                            WorldGen.PlaceTile(x, y, tileToPlace);
                        }
                    }
                    placeSuccessful = tile.active() && tile.type == tileToPlace;
                }
            }
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int GuideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Sunflowers"));
			if (GuideIndex == -1)
			{
				// Guide pass removed by some other mod.
				return;
			}
			tasks.Insert(GuideIndex + 1, new PassLegacy("ReachHideout", 
				delegate (GenerationProgress progress)
			{
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        success = true;
                        continue;
                    }
                    {
                        if (Main.rand.Next(2) == 0)
                        {
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                GenerateGrave();
                            }
                        }
                        else
                        {
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                GenerateCampsite();
                            }
                        }
                        int num67 = 1;
                        if (Main.maxTilesX == 4200)
                        {
                            num67 = Main.rand.Next(9, 15);
                        }
                        else if (Main.maxTilesX == 6400)
                        {
                            num67 = Main.rand.Next(13, 18);
                        }
                        else if (Main.maxTilesX == 8400)
                        {
                            num67 = Main.rand.Next(17, 24);
                        }
                        for (int j = 0; j < num67; j++)
                        {
                            GenerateSepulchre();
                        }
                        GenerateAltar();
                        int num32 = 1;
                        if (Main.maxTilesX == 4200)
                        {
                            num32 = Main.rand.Next(2, 5);
                        }
                        else if (Main.maxTilesX == 6400)
                        {
                            num32 = Main.rand.Next(5, 9);
                        }
                        else if (Main.maxTilesX == 8400)
                        {
                            num32 = Main.rand.Next(7, 12);
                        }
                        for (int k = 0; k < num32; k++)
                        {
                            GenerateGlowRoom();
                        }
                        for (int k = 0; k < (int)(num32 * 1.7); k++)
                        {
                            GenerateGlowGrave();
                        }

                        GenerateHideout();                       
                        GenerateUndergroundAltar();
                        if (WorldGen.crimson)
                        {
                            GenerateBoneSpike();
                        }
                        else
                        {
                            GenerateCorruptHole();
                        }
                        if (Main.rand.Next(2) == 0)
                        {
                            GenerateBanditHideout();
                            gennedBandits = true;
                        }
                        else
                        {
                            GenerateTower();
                            gennedTower = true;
                        }
                        GenerateZiggurat();
                        int[,] BoneIslandShape = new int[,]
                        {
                            {0,0,0,0,0,0,3,0,0,3,0,0,4,0,0,0,0,5,0,0,0,4,0,4,0,0,0,3,0,0,0,0},
                            {0,0,2,2,2,2,2,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,2,2,0,0},
                            {0,0,0,0,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,0,0,0},
                            {0,0,0,0,0,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,0,0,0,0,0},
                            {0,0,0,0,0,2,2,2,2,2,2,1,7,7,1,1,1,1,1,7,7,1,2,2,2,2,2,0,0,0,0,0},
                            {0,0,0,0,0,0,0,2,2,2,2,2,2,7,1,1,1,1,1,2,2,2,2,2,2,0,2,0,0,0,0,0},
                            {0,0,0,0,0,0,0,2,0,0,2,2,2,7,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,2,0,0,2,2,2,2,2,2,2,2,2,2,0,0,2,2,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,2,2,0,2,2,2,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,2,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        };
                        int[,] BoneIslandWalls = new int[,]
                        {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,0,0,0,0},
                            {0,0,0,0,0,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,0,0,0,0,0},
                            {0,0,0,0,0,0,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,2,2,2,0,2,0,0,0,0,0},
                            {0,0,0,0,0,0,2,0,2,2,2,2,2,2,1,1,1,1,1,2,2,2,2,2,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0,2,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                        };
                        bool placed = false;
                        while (!placed)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                // Select a place in the first 6th of the world
                                int towerX = WorldGen.genRand.Next(Main.maxTilesX / 3, Main.maxTilesX / 3 * 2); // from 50 since there's a unaccessible area at the world's borders
                                                                                             // 50% of choosing the last 6th of the world
                                if (WorldGen.genRand.NextBool())
                                {
                                    towerX = Main.maxTilesX - towerX;
                                }
                                int towerY = WorldGen.genRand.Next(Main.maxTilesY / 9, Main.maxTilesY / 8);
                                Tile tile = Main.tile[towerX, towerY];
                                if (tile.active())
                                {
                                    continue;
                                }
                                {
                                    PlaceBoneIsland(towerX, towerY, BoneIslandShape);
                                    PlaceBoneIslandWalls(towerX, towerY, BoneIslandWalls);
                                }
                                placed = true;
                            }
                        }
                        success = true;
                    }

                }
            }));
            int TrapsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Traps"));
            if (TrapsIndex != -1)
            {
                tasks.Insert(TrapsIndex + 1, new PassLegacy("Briar Vines", Vines));
            }
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (ShiniesIndex == -1)
            {
                // Shinies pass removed by some other mod.
                return;
            }
			tasks.Insert(TrapsIndex + 1, new PassLegacy("Asteroids", delegate (GenerationProgress progress)
            {
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        success = true;
                        continue;
                    }
                    progress.Message = "Creating an asteroid belt";
                    {
                        int width = 350;
                        int height = 75;
                        int x = 0;
                        int y = 0;
                        if (Main.maxTilesX == 4200)
                        {
                            width = 200;
                            height = 40;
                        }
                        if (Main.maxTilesX == 6400)
                        {
                            width = 275;
                            height = 60;
                        }
                        if (Main.maxTilesX == 8400)
                        {
                            width = 350;
                            height = 75;
                        }

                        if (Main.rand.Next(3) == 0) //change to check for dungeon later, idk how rn.
                        {
                            x = width + 80;
                        }
                        else
                        {
                            x = Main.maxTilesX - (width + 80);
                        }

                        y = height + 60;
                        PlaceAsteroids(x, y);

                    }
                    success = true;
                }
            }));
            tasks.Insert(ShiniesIndex + 1, new PassLegacy("Piles", delegate (GenerationProgress progress)
            {

                progress.Message = "Piling Up Ores...";
                {
                    if (WorldGen.CopperTierOre == TileID.Copper)
                    {
                        for (int i = 0; i < Main.maxTilesX * 19.5; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("CopperPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("CopperPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    else if (WorldGen.CopperTierOre == TileID.Tin)
                    {
                        for (int i = 0; i < Main.maxTilesX * 19.5; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("TinPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("TinPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    if (WorldGen.IronTierOre == TileID.Iron)
                    {
                        for (int i = 0; i < Main.maxTilesX * 12; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("IronPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("IronPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    else if (WorldGen.IronTierOre == TileID.Lead)
                    {
                        for (int i = 0; i < Main.maxTilesX * 12; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("LeadPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("LeadPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    if (WorldGen.SilverTierOre == TileID.Silver)
                    {
                        for (int i = 0; i < Main.maxTilesX * 9.75f; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("SilverPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("SilverPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    else if (WorldGen.SilverTierOre == TileID.Tungsten)
                    {
                        for (int i = 0; i < Main.maxTilesX * 9.75f; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("TungstenPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("TungstenPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    if (WorldGen.GoldTierOre == TileID.Gold)
                    {
                        for (int i = 0; i < Main.maxTilesX * 6f; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("GoldPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("GoldPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                    else if (WorldGen.GoldTierOre == TileID.Platinum)
                    {
                        for (int i = 0; i < Main.maxTilesX * 6f; i++)
                        {
                            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
                            int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
                            Tile tile = Main.tile[num3, num4];
                            if (tile.type == TileID.Stone && tile.active())
                            {
                                WorldGen.PlaceObject(num3, num4 - 1, mod.TileType("PlatinumPile"));
                                NetMessage.SendObjectPlacment(-1, num3, num4 - 1, mod.TileType("PlatinumPile"), 0, 0, -1, -1);
                            }
                        }
                    }
                }
            }));
        }		
		public override void PostWorldGen()
		{
            Tile tile;
            tile = Main.tile[1, 1];
            for (int trees = 0; trees < 18000; trees++)
			{
				int E = Main.maxTilesX;
                int F = (int)Main.worldSurface;
                tile = Framing.GetTileSafely(E, F);
				if (tile.type == mod.TileType("ReachGrassTile"))
				{
					WorldGen.GrowTree(E, F);
				}
			}
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * .2f) * 3E-05); k++)
            {
                bool placeSuccessful = false;
                int tileToPlace;
                if (WorldGen.genRand.Next(3) == 0)
                {
                    tileToPlace = TileType<Tiles.Ambient.SkullStick>();
                }
                else if (WorldGen.genRand.Next(3) == 0)
                {
                    tileToPlace = TileType<Tiles.Ambient.SkullStickFlip>();
                }
                else if (WorldGen.genRand.Next(4) == 0)
                {
                    tileToPlace = TileType<Tiles.Ambient.SkullStick3>();
                }
                else
                {
                    tileToPlace = TileType<Tiles.Ambient.SkullStick3Flip>();
                }
                while (!placeSuccessful)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)Main.worldSurface - 100, Main.maxTilesY);
                    tile = Main.tile[x, y];
                    if (tile.type == mod.TileType("ReachGrassTile"))
                    {
                        {
                            WorldGen.PlaceTile(x, y, tileToPlace);
                        }
                    }
                    placeSuccessful = tile.active() && tile.type == tileToPlace;
                }
            }
            int num584 = 1;
            if (Main.maxTilesX == 4200)
            {
                num584 = Main.rand.Next(7, 10);
            }
            else if (Main.maxTilesX == 6400)
            {
                num584 = Main.rand.Next(12, 16);
            }
            else if (Main.maxTilesX == 8400)
            {
                num584 = Main.rand.Next(15, 21);
            }
            for (int k = 0; k < num584; k++)
			{
				GenerateCrateStash();
			}		
			for (int k = 0; k < Main.rand.Next(5, 7); k++)
			{
				GenerateGemStash();
			}
			for (int i = 1; i < Main.rand.Next(4, 6); i++)
            {
                int itemsToPlaceInGlassChestsSecondaryChoice = 0;
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (chest != null && Main.tile[chest.x, chest.y].frameX == 13 * 36 && Main.rand.Next(3) == 0)
                    {
                        chest.item[6].SetDefaults(mod.ItemType("ChaosPearl"), false);
                        chest.item[6].stack = WorldGen.genRand.Next(20, 30);
                    }
                    if (chest != null && Main.tile[chest.x, chest.y].frameX == 2 * 36 && Main.rand.Next(10) == 0)
                    {
                        if (WorldGen.crimson)
                        {
                            chest.item[5].SetDefaults(mod.ItemType("Tenderizer"), false);
                        }
                        else
                        {
                            chest.item[5].SetDefaults(mod.ItemType("Slugger"), false);
                        }
                    }

                }
            }
            int[] commonItems1 = new int[] { 20, 22, 703, 704 };
            int[] ammo1 = new int[] { 40, 42 };
            int[] potions = new int[] { 290, 292, 298, 299, 303, 304 };
            int[] recall = new int[] { 2350 };
            int[] potionscorrupt = new int[] { 2349 };
            int[] potionscrim = new int[] { 2347, 2323 };
            int[] other1 = new int[] { 3093, 168 };
            int[] other2 = new int[] { 31, 8 };
            int[] moddedMaterials = new int[] { mod.ItemType("BismiteCrystal"), mod.ItemType("OldLeather") };
            int[] crimsonMain = new int[] { mod.ItemType("Spineshot"), mod.ItemType("FleshStick") };
            int[] corruptMain = new int[] { mod.ItemType("CorruptSpearVariant"), mod.ItemType("Ebonwand") };
            for (int i = 1; i < Main.rand.Next(4, 6); i++)
            {
                int[] itemsToPlacePrimary = new int[] { mod.ItemType("SepulchreStaff"), mod.ItemType("SepulchrePendant") };
                int[] ammoToPlace = new int[] { mod.ItemType("SepulchreArrow"), mod.ItemType("SepulchreBullet") };
                int itemsToPlaceInGlassChestsSecondaryChoice = 0;
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("SepulchreChestTile"))
                    {
                        chest.item[0].SetDefaults(itemsToPlacePrimary[Main.rand.Next(2)], false);
                        chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                        chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                        chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                        chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                        chest.item[3].SetDefaults(potions[Main.rand.Next(6)], false);
                        chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                        chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                        chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                        chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[7].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
                        chest.item[7].stack = WorldGen.genRand.Next(2, 6);
                        chest.item[8].SetDefaults(72, false);
                        chest.item[8].stack = WorldGen.genRand.Next(12, 30);
                    }
                    if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("FleshSpikeChest_Tile"))
                    {
                        chest.item[0].SetDefaults(crimsonMain[Main.rand.Next(2)], false);
                        chest.item[0].stack = 1;
                        chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                        chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                        chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                        chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                        chest.item[3].SetDefaults(potionscrim[Main.rand.Next(2)], false);
                        chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                        chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                        chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                        chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[7].SetDefaults(287, false);
                        chest.item[7].stack = WorldGen.genRand.Next(60, 99);
                        chest.item[8].SetDefaults(72, false);
                        chest.item[8].stack = WorldGen.genRand.Next(10, 29);
                    }
                    if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("EbonChest_Tile"))
                    {
                        chest.item[0].SetDefaults(corruptMain[Main.rand.Next(2)], false);
                        chest.item[0].stack = 1;
                        chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                        chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                        chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                        chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                        chest.item[3].SetDefaults(potionscorrupt[Main.rand.Next(1)], false);
                        chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                        chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                        chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                        chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[7].SetDefaults(287, false);
                        chest.item[7].stack = WorldGen.genRand.Next(60, 99);
                        chest.item[8].SetDefaults(72, false);
                        chest.item[8].stack = WorldGen.genRand.Next(10, 29);
                    }
                }
            }
            for (int i = 1; i < Main.rand.Next(4, 6); i++)
            {
                int[] itemsToPlacePrimary = new int[] { mod.ItemType("CleftHorn"), mod.ItemType("CactusStaff") };
                int itemsToPlaceInGlassChestsSecondaryChoice = 0;
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("GoldScarabChest"))
                    {
                        chest.item[0].SetDefaults(itemsToPlacePrimary[Main.rand.Next(2)], false);
                        chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                        chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                        chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                        chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                        chest.item[3].SetDefaults(potions[Main.rand.Next(6)], false);
                        chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                        chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                        chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                        chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[7].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
                        chest.item[7].stack = WorldGen.genRand.Next(2, 6);
                        chest.item[8].SetDefaults(72, false);
                        chest.item[8].stack = WorldGen.genRand.Next(12, 30);
                        break;
                    }
                }
            }
            for (int i = 1; i < Main.rand.Next(4, 6); i++)
            {
                int[] itemsToPlacePrimary = new int[] { mod.ItemType("Glyph"), ItemID.MagicMirror, ItemID.WandofSparking };
                int itemsToPlaceInGlassChestsSecondaryChoice = 0;
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("GoblinChest"))
                    {
                        chest.item[0].SetDefaults(itemsToPlacePrimary[Main.rand.Next(2)], false);
                        chest.item[0].stack = WorldGen.genRand.Next(1, 1);
                        chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                        chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                        chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                        chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                        chest.item[3].SetDefaults(potions[Main.rand.Next(6)], false);
                        chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                        chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                        chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                        chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[7].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
                        chest.item[7].stack = WorldGen.genRand.Next(2,6);
                        chest.item[8].SetDefaults(72, false);
                        chest.item[8].stack = WorldGen.genRand.Next(12, 30);
                        break;
                    }
                }
                int[] itemsToPlacePrimary1 = new int[] { mod.ItemType("Glyph"), ItemID.Radar, ItemID.Blowpipe, ItemID.WandofSparking };
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("ReachHideoutWoodChest"))
                    {
                        chest.item[0].SetDefaults(itemsToPlacePrimary1[Main.rand.Next(2)], false);
                        chest.item[0].stack = WorldGen.genRand.Next(1, 1);
                        chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                        chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                        chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                        chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                        chest.item[3].SetDefaults(potions[Main.rand.Next(6)], false);
                        chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                        chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                        chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                        chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                        chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                        chest.item[7].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
                        chest.item[7].stack = WorldGen.genRand.Next(2, 6);
                        chest.item[8].SetDefaults(72, false);
                        chest.item[8].stack = WorldGen.genRand.Next(12, 30);
                        break;
                    }
                }
            }
            {
				for (int i = 1; i < Main.rand.Next(4, 6); i++)
				{
					int itemsToPlaceInGlassChestsSecondaryChoice = 0;
					for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
					{
						Chest chest = Main.chest[chestIndex];
						if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("ReachChest"))
						{
							for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
							{
                                chest.item[1].SetDefaults(commonItems1[Main.rand.Next(4)], false);
                                chest.item[1].stack = WorldGen.genRand.Next(3, 10);
                                chest.item[2].SetDefaults(ammo1[Main.rand.Next(2)], false);
                                chest.item[2].stack = WorldGen.genRand.Next(20, 50);
                                chest.item[3].SetDefaults(potions[Main.rand.Next(6)], false);
                                chest.item[3].stack = WorldGen.genRand.Next(2, 3);
                                chest.item[4].SetDefaults(recall[Main.rand.Next(1)], false);
                                chest.item[4].stack = WorldGen.genRand.Next(2, 3);
                                chest.item[5].SetDefaults(other1[Main.rand.Next(2)], false);
                                chest.item[5].stack = WorldGen.genRand.Next(1, 4);
                                chest.item[6].SetDefaults(other2[Main.rand.Next(2)], false);
                                chest.item[6].stack = WorldGen.genRand.Next(1, 4);
                                chest.item[7].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
                                chest.item[7].stack = WorldGen.genRand.Next(2, 6);
                                chest.item[8].SetDefaults(72, false);
                                chest.item[8].stack = WorldGen.genRand.Next(12, 30);
                                if (Main.rand.Next(4) == 0)
                                {
                                    chest.item[9].SetDefaults(mod.ItemType("GladeWreath"), false);
                                }

                            }		
						}
					}
				}
			}
			int[] itemsToPlaceInGlassChests = new int[] { mod.ItemType("ReachChestMagic"), mod.ItemType("BriarRattle"), mod.ItemType("ReachStaffChest"), mod.ItemType("ReachBoomerang"), mod.ItemType("ReachBrooch") };
			int itemsToPlaceInGlassChestsChoice = 0;
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("ReachChest"))
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						itemsToPlaceInGlassChestsChoice = Main.rand.Next(itemsToPlaceInGlassChests.Length);
						chest.item[0].SetDefaults(itemsToPlaceInGlassChests[itemsToPlaceInGlassChestsChoice]);
						
					}
				}
			}
            for (int trees = 0; trees < 18000; trees++)
			{
				int E = Main.maxTilesX;
				int F = (int)Main.worldSurface;
				tile = Framing.GetTileSafely(E, F);
				if (tile.type == mod.TileType("ReachGrassTile"))
				{
					WorldGen.GrowTree(E, F);
				}
			} 
		}
		private void PlaceBoneIsland(int i, int j, int[,] BoneIslandArray) {
			
			for (int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BoneIslandArray[y, x]) {
							case 0:
								break; // no changes
							case 1:
								WorldGen.PlaceTile(k, l, 0); // Dirt
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceTile(k, l, 189);
								break;
							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 6:
								WorldGen.PlaceTile(k, l, TileID.Grass); // Dirt
								tile.active(true);
								break;
							case 7:
								WorldGen.PlaceTile(k, l, 194); // Dirt
								tile.active(true);
								break;
						}
					}
				}
			}
			for (int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BoneIslandArray[y, x]) {
							case 3:
								WorldGen.PlaceTile(k, l, (ushort)mod.TileType("SkullPile1")); // Gold Chest
								break;
							case 4:
								WorldGen.PlaceTile(k, l, (ushort)mod.TileType("SkullPile2")); // Gold Chest
								break;		
							case 5:
								WorldGen.PlaceTile(k, l - 1, (ushort)mod.TileType("AvianEgg")); // Gold Chest
								break;					
						}
					}
				}
			}
		}
	private void PlaceBoneIslandWalls(int i, int j, int[,] BoneIslandArray) {
			
			for (int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BoneIslandArray[y, x]) {
							case 0:
								break; // no changes
							case 1:
								WorldGen.PlaceTile(k, l, 0); // Dirt
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceTile(k, l, 189);
								break;
							case 3:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								break;
						}
					}
				}
			}
			for (int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30)){
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BoneIslandArray[y, x]) {
							case 1:
								WorldGen.PlaceWall(k, l, 73); 
								break;		
							case 2:
								WorldGen.PlaceWall(k, l, 73); 
								break;					
						}
					}
				}
			}
		}
		public override void PostUpdate()
		{
            Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.ZoneSpirit)
			{
				if (!aurora)
				{
					aurora = true;
				}
				auroraType = 5;
			}
			if (Main.dayTime != dayTimeLast)
				dayTimeSwitched = true;
			else
				dayTimeSwitched = false;
			dayTimeLast = Main.dayTime;

			if (dayTimeSwitched)
			{
                if (Main.rand.Next(4) == 0 && !spaceJunkWeather)
                {
                    stardustWeather = true;
                }
                else
                {
                    stardustWeather = false;
                }
                if (Main.rand.Next(4) == 0 && !stardustWeather)
                {
                    spaceJunkWeather = true;
                }
                else
                {
                    spaceJunkWeather = false;
                }
                if (!Main.dayTime && Main.hardMode)
				{
					if (!Main.fastForwardTime && !Main.bloodMoon && WorldGen.spawnHardBoss == 0 &&
						NPC.downedMechBossAny && Main.rand.Next(27) == 0)
					{
						Main.NewText("A Blue Moon is rising...", 0, 90, 220);
						BlueMoon = true;
					}
				}
				else
				{
					BlueMoon = false;
				}
			   if (NPC.downedBoss3)
			    {
				    auroraChance = 6;
			    }
			    if (Main.hardMode)
			    {
				    auroraChance = 15;
			    }
                {
                    if (!Main.dayTime && Main.rand.Next(auroraChance) == 0)
                    {
                        auroraType = Main.rand.Next(new int[] { 1, 2, 3, 5 });
                        aurora = true;
                    }
                    else
                    {
                        aurora = false;
                    }
                }
                if (Main.bloodMoon)
                {
                    MyWorld.auroraType = 6;
                }
                if (Main.pumpkinMoon)
                {
                    MyWorld.auroraType = 7;
                }
                if (Main.snowMoon)
                {
                    auroraType = 8;
                }

            }

                if (NPC.downedBoss1)
			{
				if (!Magicite)
				{
					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 23) * 15E-05); k++)
					{
                        int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int y = WorldGen.genRand.Next(0, Main.maxTilesY);
						if (Main.tile[x, y] != null)
						{
							if (Main.tile[x, y].active())
							{
								if (Main.tile[x, y].type == mod.TileType("ReachGrassTile"))
								{
                                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(4, 7), mod.TileType("FloranOreTile"), false, 0f, 0f, false, true);


                                }
                            }
						}
					}
					Main.NewText("The Briar's vines tremble...", 100, 220, 100);
					Main.NewText("Ancient enemies have awakened underground!", 204, 153, 0);
					Magicite = true;
				}
			}
			if (NPC.downedBoss2)
			{
				if (!gmOre)
				{
                    for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 3.2f) * 15E-05); k++)
                    {
                        int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                        int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
                        if (Main.tile[EEXX, WHHYY] != null)
                        {
                            if (Main.tile[EEXX, WHHYY].active())
                            {
                                if (Main.tile[EEXX, WHHYY].type == 161)
                                {
                                    WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)mod.TileType("CryoliteOreTile"));
                                }
                                else if (Main.tile[EEXX, WHHYY].type == 163)
                                {
                                    WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)mod.TileType("CryoliteOreTile"));
                                }
                                else if (Main.tile[EEXX, WHHYY].type == 164)
                                {
                                    WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)mod.TileType("CryoliteOreTile"));
                                }
                                else if (Main.tile[EEXX, WHHYY].type == 200)
                                {
                                    WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)mod.TileType("CryoliteOreTile"));
                                }
                            }
                        }
                    }
                    for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 86) * 15E-05); k++)
					{
						int EEXX = WorldGen.genRand.Next(0, Main.maxTilesX);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
						if (Main.tile[EEXX, WHHYY] != null)
						{
							if (Main.tile[EEXX, WHHYY].active())
							{
								if (Main.tile[EEXX, WHHYY].type == 368)
								{
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)mod.TileType("GraniteOre"));
								}
							}
						}
					}
					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 108) * 15E-05); k++)
					{
						int EEXX = WorldGen.genRand.Next(0, Main.maxTilesX);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
						if (Main.tile[EEXX, WHHYY] != null)
						{
							if (Main.tile[EEXX, WHHYY].active())
							{
								if (Main.tile[EEXX, WHHYY].type == 367)
								{
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(4, 9), (ushort)mod.TileType("MarbleOre"));
								}
							}
						}
					}
					{
						Main.NewText("Energy seeps into marble and granite caverns...", 100, 220, 100);
                        Main.NewText("The icy caverns are shimmering", 70, 170, 255);
                        gmOre = true;
					}
				}
			}
            if (NPC.downedQueenBee)
			{
				if (!flierMessage)
				{
					Main.NewText("The skies grow restless...", 204, 153, 0);
					flierMessage = true;
				}
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				if (!essenceMessage)
				{
					Main.NewText("The Essences are bursting!", 66, 170, 100);

					essenceMessage = true;
				}
			}
			if (NPC.downedPlantBoss)
			{
				if (!Thermite)
				{
					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 1.13f) * 15E-05); k++)
					{
						int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 500);
						if (Main.tile[EEXX, WHHYY] != null)
						{
							if (Main.tile[EEXX, WHHYY].active())
							{
								if (Main.tile[EEXX, WHHYY].type == 1)
								{
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(4, 6), (ushort)mod.TileType("ThermiteOre"));
								}
							}
						}
					}
					Main.NewText("The Caverns have been flooded with lava!", 220, 40, 40);
					Main.NewText("The Spirits are ready to bless you once more...", 80, 80, 220);
					Thermite = true;
				}
			}
			if (NPC.downedBoss3)
			{
				if (!starMessage)
				{
					starMessage = true;
					if (!txt)
					{
						Main.NewText("The stars are brightening...", 66, 170, 244);
						txt = true;
					}
				}
			}


			if (NPC.downedMechBoss3 == true || NPC.downedMechBoss2 == true || NPC.downedMechBoss1 == true)
			{
				if (!spiritBiome)
				{
					spiritBiome = true;
					Main.NewText("The Spirits spread through the Land...", Color.Orange.R, Color.Orange.G, Color.Orange.B);
					Random rand = new Random();
					int XTILE;
					if (Terraria.Main.dungeonX > Main.maxTilesX / 2) //rightside dungeon
					{
						XTILE = WorldGen.genRand.Next((Main.maxTilesX / 2) + 300, Main.maxTilesX - 500);
					}
					else //leftside dungeon
					{
						XTILE = WorldGen.genRand.Next(75, (Main.maxTilesX / 2) - 600);
					}
					int xAxis = XTILE;
					int xAxisMid = xAxis + 70;
					int xAxisEdge = xAxis + 380;
					int yAxis = 0;
					for (int y = 0; y < Main.maxTilesY; y++)
					{
						yAxis++;
						xAxis = XTILE;
						for (int i = 0; i < 450; i++)
						{
							xAxis++;
							if (Main.tile[xAxis, yAxis] != null)
							{
								if (Main.tile[xAxis, yAxis].active())
								{
									int[] TileArray = { 0 };
									if (TileArray.Contains(Main.tile[xAxis, yAxis].type))
									{
										if (Main.tile[xAxis, yAxis + 1] == null)
										{
											if (Main.rand.Next(0, 50) == 1)
											{
												WillGenn = 0;
												if (xAxis < xAxisMid - 1)
												{
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if (xAxis > xAxisEdge + 1)
												{
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if (WillGenn < 10)
												{
													Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritDirt");
												}
											}
										}
										else
										{
											WillGenn = 0;
											if (xAxis < xAxisMid - 1)
											{
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if (xAxis > xAxisEdge + 1)
											{
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if (WillGenn < 10)
											{
												Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritDirt");
											}
										}
									}
									int[] TileArray84 = { 2, 23, 109, 199 };
									if (TileArray84.Contains(Main.tile[xAxis, yAxis].type))
									{
										if (Main.tile[xAxis, yAxis + 1] == null)
										{
											if (rand.Next(0, 50) == 1)
											{
												WillGenn = 0;
												if (xAxis < xAxisMid - 1)
												{
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if (xAxis > xAxisEdge + 1)
												{
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if (WillGenn < 18)
												{
													Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritGrass");
												}
											}
										}
										else
										{
											WillGenn = 0;
											if (xAxis < xAxisMid - 1)
											{
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if (xAxis > xAxisEdge + 1)
											{
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if (WillGenn < 18)
											{
												Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritGrass");
											}
										}
									}
									int[] TileArray1 = { 161, 163, 164, 200 };
									if (TileArray1.Contains(Main.tile[xAxis, yAxis].type))
									{
										if (Main.tile[xAxis, yAxis + 1] == null)
										{
											if (rand.Next(0, 50) == 1)
											{
												WillGenn = 0;
												if (xAxis < xAxisMid - 1)
												{
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if (xAxis > xAxisEdge + 1)
												{
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if (WillGenn < 18)
												{
													Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritIce");
												}
											}
										}
										else
										{
											WillGenn = 0;
											if (xAxis < xAxisMid - 1)
											{
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if (xAxis > xAxisEdge + 1)
											{
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if (WillGenn < 18)
											{
												Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritIce");
											}
										}
									}
									int[] TileArray2 = { 1, 25, 117, 203 };
									if (TileArray2.Contains(Main.tile[xAxis, yAxis].type))
									{
										if (Main.tile[xAxis, yAxis + 1] == null)
										{
											if (rand.Next(0, 50) == 1)
											{
												WillGenn = 0;
												if (xAxis < xAxisMid - 1)
												{
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if (xAxis > xAxisEdge + 1)
												{
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if (WillGenn < 18)
												{
													Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritStone");
												}
											}
										}
										else
										{
											WillGenn = 0;
											if (xAxis < xAxisMid - 1)
											{
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if (xAxis > xAxisEdge + 1)
											{
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if (WillGenn < 18)
											{
												Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("SpiritStone");
											}
										}
									}

									int[] TileArray89 = { 3, 24, 110, 113, 115, 201, 205, 52, 62, 32, 165 };
									if (TileArray89.Contains(Main.tile[xAxis, yAxis].type))
									{
										if (Main.tile[xAxis, yAxis + 1] == null)
										{
											if (rand.Next(0, 50) == 1)
											{
												WillGenn = 0;
												if (xAxis < xAxisMid - 1)
												{
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if (xAxis > xAxisEdge + 1)
												{
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if (WillGenn < 18)
												{
													Main.tile[xAxis, yAxis].active(false);
												}
											}
										}
										else
										{
											WillGenn = 0;
											if (xAxis < xAxisMid - 1)
											{
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if (xAxis > xAxisEdge + 1)
											{
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if (WillGenn < 18)
											{
												Main.tile[xAxis, yAxis].active(false);
											}
										}
									}


									int[] TileArray3 = { 53, 116, 112, 234 };
									if (TileArray3.Contains(Main.tile[xAxis, yAxis].type))
									{
										if (Main.tile[xAxis, yAxis + 1] == null)
										{
											if (rand.Next(0, 50) == 1)
											{
												WillGenn = 0;
												if (xAxis < xAxisMid - 1)
												{
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if (xAxis > xAxisEdge + 1)
												{
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if (WillGenn < 18)
												{
													Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("Spiritsand");
												}
											}
										}
										else
										{
											WillGenn = 0;
											if (xAxis < xAxisMid - 1)
											{
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if (xAxis > xAxisEdge + 1)
											{
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if (WillGenn < 18)
											{
												Main.tile[xAxis, yAxis].type = (ushort)mod.TileType("Spiritsand");
											}
										}
									}
								}
								if (Main.tile[xAxis, yAxis].type == mod.TileType("SpiritStone") && yAxis > WorldGen.rockLayer + 100 && Main.rand.Next(1500) == 6)
								{
									WorldGen.TileRunner(xAxis, yAxis, (double)WorldGen.genRand.Next(5, 7), 1, mod.TileType("SpiritOreTile"), false, 0f, 0f, true, true);
								}
							}
						}
					}
					int chests = 0;
					for (int r = 0; r < 380000; r++)
					{
						int success = WorldGen.PlaceChest(xAxis - Main.rand.Next(450), Main.rand.Next(100, 275), (ushort)mod.TileType("SpiritChestLocked"), false, 2);
						if (success > -1)
						{
							string[] lootTable = { "GhastKnife", "GhastStaff", "GhastStaffMage", "GhastSword", "GhastBeam", };
							Main.chest[success].item[0].SetDefaults(mod.ItemType(lootTable[chests]), false);
							int[] lootTable2 = { 499, 1508, mod.ItemType("SpiritBar"), Items.Glyphs.Glyph._type };
							Main.chest[success].item[1].SetDefaults(lootTable2[Main.rand.Next(4)], false);
							Main.chest[success].item[1].stack = WorldGen.genRand.Next(3, 8);
							Main.chest[success].item[2].SetDefaults(lootTable2[Main.rand.Next(4)], false);
							Main.chest[success].item[2].stack = WorldGen.genRand.Next(3, 8);
							Main.chest[success].item[3].SetDefaults(lootTable2[Main.rand.Next(4)], false);
							Main.chest[success].item[3].stack = WorldGen.genRand.Next(3, 8);
							Main.chest[success].item[4].SetDefaults(lootTable2[Main.rand.Next(4)], false);
							Main.chest[success].item[4].stack = WorldGen.genRand.Next(1, 2);
							chests++;
							if (chests >= 5)
							{
								break;
							}
						}
					}
				}
			}
		}
	}
}