using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Glyphs;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Placeable;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Town;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.IceSculpture.Hostile;
using SpiritMod.Tiles.Ambient.SpaceCrystals;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Furniture;
using SpiritMod.Tiles.Furniture.SpaceJunk;
using SpiritMod.Tiles.Piles;
using SpiritMod.Tiles.Walls.Natural;
using SpiritMod.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod
{
	public class MyWorld : ModWorld
	{
		List<Point> yureis = new List<Point>();
		List<Point> samurais = new List<Point>();
		private static bool dayTimeLast;
		public static bool dayTimeSwitched;

		public static bool aurora = false;
		public static int auroraType = 1;
		public static int auroraTypeFixed;
		public static int auroraChance = 4;

		public static bool luminousOcean = false;
		public static int luminousType = 1;

		public static bool stardustWeather = false;
		public static bool spaceJunkWeather = false;
		public static bool meteorShowerWeather = false;

		public static float asteroidLight = 0;
		public static float spiritLight = 0;

		public static bool BlueMoon = false;
		public static int pagodaX = 0;
		public static int pagodaY = 0;
		public static int SpiritTiles = 0;
		public static int AsteroidTiles = 0;
		public static int MarbleTiles = 0;
		public static int GraniteTiles = 0;
		public static int ReachTiles = 0;
		public static int HiveTiles = 0;
		public static int CorruptHazards = 0;

		public static bool Magicite = false;
		public static bool Thermite = false;
		public static bool Cryolite = false;
		public static bool spiritBiome = false;
		public static bool rockCandy = false;
		public static bool gmOre = false;
		public static bool starMessage = false;
		public static bool essenceMessage = false;
		public static int asteroidSide = 0;
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
		public static bool downedBlueMoon = false;

		//Adventurer variables
		public static bool sepulchreComplete = false;
        public static bool jadeStaffComplete = false;
        public static bool shadowflameComplete = false;
        public static bool vibeShroomComplete = false;
        public static bool winterbornComplete = false;
        public static bool drBonesComplete = false;
        public static bool spawnHornetFish = false;
		public static bool spawnVibeshrooms = false;
		public static int numWinterbornKilled;
		public static int numBeholdersKilled;
		public static int numValkyriesKilled;
		public static int numAntlionsKilled;
		public static int numDrBonesKilled;
		public static int numWheezersKilled;
		public static int numStardancersKilled;

		public static Dictionary<string, bool> droppedGlyphs = new Dictionary<string, bool>();

		//bool night = false;
		public bool txt = false;

		private int WillGenn = 0;
		private int Meme;

		public override void TileCountsAvailable(int[] tileCounts)
		{
			SpiritTiles = tileCounts[ModContent.TileType<SpiritDirt>()] + tileCounts[ModContent.TileType<SpiritStone>()]
			+ tileCounts[ModContent.TileType<Spiritsand>()] + tileCounts[ModContent.TileType<SpiritIce>()] + tileCounts[ModContent.TileType<SpiritGrass>()];
			//now you don't gotta have 6 separate things for tilecount
			ReachTiles = tileCounts[ModContent.TileType<BriarGrass>()];
			AsteroidTiles = tileCounts[ModContent.TileType<Asteroid>()] + tileCounts[ModContent.TileType<BigAsteroid>()] + tileCounts[ModContent.TileType<SpaceJunkTile>()] + tileCounts[ModContent.TileType<Glowstone>()];
			CorruptHazards = tileCounts[ModContent.TileType<Corpsebloom>()] + tileCounts[ModContent.TileType<Corpsebloom1>()] + tileCounts[ModContent.TileType<Corpsebloom2>()];
			MarbleTiles = tileCounts[367];
			GraniteTiles = tileCounts[368];
			HiveTiles = tileCounts[225];
		}


		public override TagCompound Save()
		{
			TagCompound data = new TagCompound();
			var downed = new List<string>();
			if(downedScarabeus)
				downed.Add("scarabeus");
			if(downedAncientFlier)
				downed.Add("ancientFlier");
			if(downedRaider)
				downed.Add("starplateRaider");
			if(downedInfernon)
				downed.Add("infernon");
			if(downedReachBoss)
				downed.Add("vinewrathBane");
			if(downedSpiritCore)
				downed.Add("etherealUmbra");
			if(downedDusking)
				downed.Add("dusking");
			if(downedIlluminantMaster)
				downed.Add("illuminantMaster");
			if(downedAtlas)
				downed.Add("atlas");
			if(downedOverseer)
				downed.Add("overseer");
			if(downedBlueMoon)
				downed.Add("bluemoon");
			data.Add("downed", downed);

			TagCompound droppedGlyphTag = new TagCompound();
			foreach(KeyValuePair<string, bool> entry in droppedGlyphs) {
				droppedGlyphTag.Add(entry.Key, entry.Value);
			}
			data.Add("droppedGlyphs", droppedGlyphTag);

			data.Add("blueMoon", BlueMoon);

			data.Add("gennedBandits", gennedBandits);
			data.Add("gennedTower", gennedTower);

			//Adventurer Bools
			data.Add("sepulchreComplete", sepulchreComplete);
            data.Add("jadeStaffComplete", jadeStaffComplete);
            data.Add("shadowflameComplete", shadowflameComplete);
            data.Add("vibeShroomComplete", vibeShroomComplete);
            data.Add("winterbornComplete", winterbornComplete);
            data.Add("drBonesComplete", drBonesComplete);
            data.Add("spawnHornetFish", spawnHornetFish);
			data.Add("spawnVibeshrooms", spawnVibeshrooms);
			data.Add("numWinterbornKilled", numWinterbornKilled);
			data.Add("numAntlionsKilled", numAntlionsKilled);
			data.Add("numWheezersKilled", numWheezersKilled);

			SpiritMod.AdventurerQuests.WorldSave(data);
            SaveSpecialNPCs(data);
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
			downedBlueMoon = downed.Contains("bluemoon");
            LoadSpecialNPCs(tag);
            SpiritMod.AdventurerQuests.WorldLoad(tag);
			TagCompound droppedGlyphTag = tag.GetCompound("droppedGlyphs");
			droppedGlyphs.Clear();
			foreach(KeyValuePair<string, object> entry in droppedGlyphTag) {
				droppedGlyphs.Add(entry.Key, entry.Value is byte ? (byte)entry.Value != 0 : entry.Value as bool? ?? false);
			}

			BlueMoon = tag.GetBool("blueMoon");

			gennedBandits = tag.GetBool("gennedBandits");
			gennedTower = tag.GetBool("gennedTower");

			sepulchreComplete = tag.GetBool("sepulchreComplete");
            jadeStaffComplete = tag.GetBool("jadeStaffComplete");
            shadowflameComplete = tag.GetBool("shadowflameComplete");
            vibeShroomComplete = tag.GetBool("vibeShroomComplete");
            winterbornComplete = tag.GetBool("winterbornComplete");
            drBonesComplete = tag.GetBool("drBonesComplete");
            spawnHornetFish = tag.GetBool("spawnHornetFish");
			spawnVibeshrooms = tag.GetBool("spawnVibeshrooms");
			numWinterbornKilled = tag.Get<int>("numWinterbornKilled");
			numAntlionsKilled = tag.Get<int>("numAntlionsKilled");
			numWheezersKilled = tag.Get<int>("numWheezersKilled");
			numStardancersKilled = tag.Get<int>("numStardancersKilled");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if(loadVersion == 0) {
				BitsByte flags = reader.ReadByte();
				BitsByte flags1 = reader.ReadByte();
				BitsByte flags2 = reader.ReadByte();
				BitsByte flags3 = reader.ReadByte();
				downedScarabeus = flags[0];
				downedAncientFlier = flags[1];
				downedRaider = flags[2];
				downedInfernon = flags[3];
				downedDusking = flags[4];
				downedIlluminantMaster = flags[5];
				downedAtlas = flags[6];
				downedOverseer = flags[7];
				downedBlueMoon = flags[8];
				downedReachBoss = flags1[0];
				downedSpiritCore = flags1[1];

				gennedBandits = flags2[0];
				gennedTower = flags2[1];

				sepulchreComplete = flags3[0];
				spawnHornetFish = flags3[1];
				spawnVibeshrooms = flags3[2];
                jadeStaffComplete = flags3[3];
                shadowflameComplete = flags3[4];
                vibeShroomComplete = flags3[5];
                winterbornComplete = flags3[6];
                drBonesComplete = flags3[7];
            } else {
				mod.Logger.Error("Unknown loadVersion: " + loadVersion);
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte bosses1 = new BitsByte(downedScarabeus, downedAncientFlier, downedRaider, downedInfernon, downedDusking, downedIlluminantMaster, downedAtlas, downedOverseer);
			BitsByte bosses2 = new BitsByte(downedReachBoss, downedSpiritCore);
			writer.Write(bosses1);
			writer.Write(bosses2);
			BitsByte environment = new BitsByte(BlueMoon);
            BitsByte worldgen = new BitsByte(gennedBandits, gennedTower);
			BitsByte adventurerQuests = new BitsByte(sepulchreComplete, spawnHornetFish, spawnVibeshrooms, jadeStaffComplete, shadowflameComplete, vibeShroomComplete, winterbornComplete, drBonesComplete);
			writer.Write(environment);
			writer.Write(worldgen);
			writer.Write(adventurerQuests);
			writer.Write(numWinterbornKilled);
			writer.Write(numAntlionsKilled);
			writer.Write(numWheezersKilled);
			writer.Write(numStardancersKilled);
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
			downedBlueMoon = bosses1[8];
			downedReachBoss = bosses2[0];
			downedSpiritCore = bosses2[1];

			BitsByte environment = reader.ReadByte();
			BlueMoon = environment[0];

			BitsByte worldgen = reader.ReadByte();
			gennedBandits = worldgen[0];
			gennedTower = worldgen[1];

			BitsByte adventurerQuests = reader.ReadByte();
			sepulchreComplete = adventurerQuests[0];
			spawnHornetFish = adventurerQuests[1];
			spawnVibeshrooms = adventurerQuests[2];
            jadeStaffComplete = adventurerQuests[3];
            shadowflameComplete = adventurerQuests[4];
            vibeShroomComplete = adventurerQuests[5];
            winterbornComplete = adventurerQuests[6];
            drBonesComplete = adventurerQuests[7];
            numWinterbornKilled = reader.ReadInt32();
			numAntlionsKilled = reader.ReadInt32();
			numWheezersKilled = reader.ReadInt32();
			numStardancersKilled = reader.ReadInt32();
		}

		public override void Initialize()
		{
			BlueMoon = false;
			dayTimeLast = Main.dayTime;
			dayTimeSwitched = false;

			if(NPC.downedBoss2 == true)
				gmOre = true;
			else
				gmOre = false;

			if(NPC.downedBoss1 == true)
				Magicite = true;
			else
				Magicite = false;

			if(NPC.downedMechBoss3 == true || NPC.downedMechBoss2 == true || NPC.downedMechBoss1 == true)
				spiritBiome = true;
			else
				spiritBiome = false;
			if(Main.hardMode) {
				rockCandy = true;
			} else {
				rockCandy = false;
			}
			if(NPC.downedBoss3)
				starMessage = true;
			else
				starMessage = false;

			if(NPC.downedPlantBoss)
				Thermite = true;
			else
				Thermite = false;

			if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
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
			downedBlueMoon = false;
		}
        private void LoadSpecialNPCs(TagCompound compound)
        {
            int npcTest = 0;
            while (compound.ContainsKey($"SpiritSavedNPC{npcTest}"))
            {
                TagCompound npc = (TagCompound)compound[$"SpiritSavedNPC{npcTest++}"];

                NPC.NewNPC((int)npc["x"], (int)npc["y"], (int)npc["type"]);
            }
        }

        private void SaveSpecialNPCs(TagCompound compound)
        {
            int current = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.type == ModContent.NPCType<NPCs.PagodaGhostPassive>() || npc.type == ModContent.NPCType<NPCs.SamuraiPassive>())
                {
                    TagCompound tag = new TagCompound();
                    tag["type"] = npc.type;
                    tag["x"] = (int)npc.position.X;
                    tag["y"] = (int)npc.position.Y;
                    compound.Add($"SpiritSavedNPC{current++}", tag);
                }
            }
        }
        #region Asteroid
        public static ushort OreRoller(ushort glowstone, ushort none)
		{
			ushort iron = MyWorld.GetNonOre(WorldGen.IronTierOre);
			ushort silver = MyWorld.GetNonOre(WorldGen.SilverTierOre);
			ushort gold = MyWorld.GetNonOre(WorldGen.GoldTierOre);

			int OreRoll = Main.rand.Next(1120);
			if(OreRoll < 300) {
				return GetNonOre(iron);
			} else if(OreRoll < 500) {
				return GetNonOre(silver);
			} else if(OreRoll < 800) {
				return GetNonOre(gold);
			} else if(OreRoll < 850) {
				return 37; //meteorite
			} else if(OreRoll < 1100) {
				return glowstone;
			} else {
				return glowstone;
			}
		}
		public static ushort GetNonOre(ushort ore)
		{
			switch(ore) {
				case TileID.Copper: //copper ==> tin
					return TileID.Tin;
				case TileID.Tin: //tin ==> copper
					return TileID.Copper;
				case TileID.Iron: //iron ==> lead
					return TileID.Lead;
				case TileID.Lead: //lead ==> iron
					return TileID.Iron;
				case TileID.Silver: //silver ==> tungsten
					return TileID.Tungsten;
				case TileID.Tungsten: //tungsten ==> silver
					return TileID.Silver;
				case TileID.Gold: //gold ==> platinum
					return TileID.Platinum;
				case TileID.Platinum: //platinum ==> gold
					return TileID.Gold;
			}
			return 0;
		}
		public static void PlaceBlob(int x, int y, float xsize, float ysize, int size, int type, int roundness, bool placewall = false, int walltype = 0)
		{
			int distance = size;
			for(int i = 0; i < 360; i++) {
				if((360 - i) <= ((Math.Abs(size - distance)) / Math.Sqrt(size)) * 50) {
					if(size > distance) {
						distance++;
					} else {
						distance--;
					}
				} else {
					int increase = Main.rand.Next(roundness);
					if(increase == 0 && distance > 3) {
						distance--;
					}
					if(increase == 1) {
						distance++;
					}
				}
				int offsetX = (int)(Math.Sin(i * (Math.PI / 180)) * distance * xsize);
				int offsetY = (int)(Math.Cos(i * (Math.PI / 180)) * distance * ysize);
				drawLine(x, y, x + offsetX, y + offsetY, type, placewall, walltype);
			}

		}
		public static void drawLine(int xpoint1, int ypoint1, int xpoint2, int ypoint2, int type, bool placewall, int walltype)
		{
			int xdist = xpoint2 - xpoint1;
			int ydist = ypoint2 - ypoint1;
			float distance = (float)Math.Sqrt((Math.Abs(xpoint2 - xpoint1) ^ 2) + (Math.Abs(ypoint2 - ypoint1) ^ 2));
			float xDistRelative = (float)xdist / distance;
			float yDistRelative = (float)ydist / distance;
			for(float i = 0; i < distance; i += (float)0.1) {
				int tilePlaceX = xpoint1 + (int)(xDistRelative * i);
				int tilePlaceY = ypoint1 + (int)(yDistRelative * i);
				Tile tile = Main.tile[tilePlaceX, tilePlaceY];
				tile.active(true);
				tile.type = (ushort)type;
				if(i < distance - 1 && placewall) {
					tile.wall = (ushort)walltype;
				}
			}
		}

		/// <summary>
		/// Checks if the given area is more or less flattish.
		/// Returns false if the average tile height variation is greater than the threshold.
		/// Expects that the first tile is solid, and traverses from there.
		/// Use the weight parameters to change the importance of up/down checks.
		/// </summary>
		/// <param name="startX"></param>
		/// <param name="startY"></param>
		/// <param name="width"></param>
		/// <param name="threshold"></param>
		/// <param name="goingDownWeight"></param>
		/// <param name="goingUpWeight"></param>
		/// <returns></returns>
		public static bool CheckFlat(int startX, int startY, int width, float threshold, int goingDownWeight = 0, int goingUpWeight = 0)
		{
			// Fail if the tile at the other end of the check plane isn't also solid
			if(!WorldGen.SolidTile(startX + width, startY)) return false;

			float totalVariance = 0;
			for(int i = 0; i < width; i++) {
				if(startX + i >= Main.maxTilesX) return false;

				// Fail if there is a tile very closely above the check area
				for(int k = startY - 1; k > startY - 100; k--) {
					if(WorldGen.SolidTile(startX + i, k)) return false;
				}

				// If the tile is solid, go up until we find air
				// If the tile is not, go down until we find a floor
				int offset = 0;
				bool goingUp = WorldGen.SolidTile(startX + i, startY);
				offset += goingUp ? goingUpWeight : goingDownWeight;
				while((goingUp && WorldGen.SolidTile(startX + i, startY - offset))
					|| (!goingUp && !WorldGen.SolidTile(startX + i, startY + offset))) {
					offset++;
				}
				if(goingUp) offset--; // account for going up counting the first tile
				totalVariance += offset;
			}
			return totalVariance / width <= threshold;
		}

		public void PlaceAsteroids(int i, int j)
		{
			bool success = false;
			int attempts = 0;
			while(!success) {
				attempts++;
				if(attempts > 1000) {
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
				if(Main.maxTilesX == 4200) {
					numberOfAsteroids = 33;
					numberOfBigs = 1;
					numberOfOres = 140;
					numJunkPiles = 15;
					width = 200;
					height = 40;
				}
				if(Main.maxTilesX == 6400) {
					numberOfAsteroids = 50;
					numberOfBigs = 2;
					numberOfOres = 220;
					width = 275;
					numJunkPiles = 21;
					height = 60;
				}
				if(Main.maxTilesX == 8400) {
					numberOfAsteroids = 79;
					numberOfBigs = 4;
					numberOfOres = 300;
					width = 350;
					numJunkPiles = 32;
					height = 75;
				}
				int radius = (int)Math.Sqrt((width * width) + (height * height));

				for(int b = 0; b < numberOfAsteroids; b++) //small asteroids
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
				for(int b = 0; b < numJunkPiles; b++) //junkPiles
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
				for(int b = 0; b < numberOfBigs; b++) //big asteroids
				{
					x = basex + (int)(Main.rand.Next(0 - width, width) / 1.5f);
					y = basey + Main.rand.Next(0 - height, height);
					float xsize = (float)(Main.rand.Next(75, 133)) / 100;
					float ysize = (float)(Main.rand.Next(75, 133)) / 100;
					int size = Main.rand.Next(11, 17);
					PlaceBlob(x, y, xsize, ysize, size, ModContent.TileType<BigAsteroid>(), 10, true, ModContent.WallType<AsteroidWall>());
				}
				for(int b = 0; b < numberOfOres; b++) //ores
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
				while(Ctries < CmaxTries && Csuccesses < 4) {
					x = i + WorldGen.genRand.Next(0 - width, width);
					y = j + WorldGen.genRand.Next(0 - height, height);
					if(WorldGen.PlaceChest(x, y, (ushort)ModContent.TileType<Tiles.Furniture.SpaceJunk.AsteroidChest>(), false, 0) != -1) {
						Csuccesses++;
					}
					Ctries++;
				}

				success = true;
			}

		}
		#endregion
		#region MageTower
		private void PlaceTower(int i, int j, int[,] ShrineArray, int[,] HammerArray, int[,] WallsArray, int[,] LootArray)
		{

			for(int y = 0; y < WallsArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
							case 1:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, WallID.GrassUnsafe, mute: true); // Stone Slab
								break;
							case 2:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, WallID.ArcaneRunes, mute: true); // Stone Slab
								break;
							case 4:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceTile(k, l, TileID.WoodenBeam, mute: true); // Platforms
								tile.active(true);
								break;
							case 5:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, WallID.WoodenFence, mute: true); // Platforms
								break;
							case 8:
								Framing.GetTileSafely(k, l).ClearTile();
								WorldGen.PlaceWall(k, l, WallID.StoneSlab, mute: true); // Stone Slab
								break;
						}
					}
				}
			}
			for(int y = 0; y < ShrineArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for(int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(ShrineArray[y, x]) {
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
								WorldGen.PlaceTile(k, l, 0, mute: true);
								tile.active(true);
								break;
						}
					}
				}
			}

			int shingleColor = WorldGen.genRand.NextBool() ? TileID.RedDynastyShingles : TileID.BlueDynastyShingles;
			for(int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(ShrineArray[y, x]) {
							case 1:
								WorldGen.PlaceTile(k, l, TileID.StoneSlab, mute: true); // Stone Slab
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceTile(k, l, TileID.Platforms, mute: true); // Platforms
								tile.active(true);
								break;
							case 3:
								WorldGen.PlaceTile(k, l, TileID.WoodBlock, mute: true); // Wood
								tile.active(true);
								break;
							case 6:
								WorldGen.PlaceTile(k, l, shingleColor, mute: true); // Roofing
								tile.active(true);
								break;
						}
					}
				}
			}
			for(int y = 0; y < LootArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(LootArray[y, x]) {
							case 1:
								WorldGen.PlaceTile(k, l, TileID.Pots, mute: true);  // Pot
								tile.active(true);
								break;
							case 2:
								WorldGen.PlaceObject(k, l, TileType<GoblinStatueTile>(), mute: true);
								break;
							case 4:
								WorldGen.PlaceObject(k, l - 1, TileType<ShadowflameStone>(), mute: true);
								break;
							case 5:
								WorldGen.PlaceObject(k, l, TileID.Books, mute: true, style: Main.rand.Next(5)); // Book
								break;
							case 6:
								WorldGen.PlaceObject(k, l, TileID.FishingCrate, mute: true); // Crate
								break;
							case 7:
								WorldGen.PlaceChest(k, l, (ushort)TileType<GoblinChest>(), false, 0); // Gold Chest
								break;
							case 8:
								WorldGen.PlaceObject(k, l, TileID.Bottles, mute: true); // Crate
								break;
							case 9:
								WorldGen.PlaceObject(k, l - 1, TileType<GoblinStandardTile>(), mute: true); // Crate
								break;
						}
					}
				}
			}
			for(int y = 0; y < HammerArray.GetLength(0); y++) {
				for(int x = 0; x < HammerArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						WorldGen.SlopeTile(k, l, HammerArray[y, x]);
						if(TileID.Sets.Platforms[Main.tile[k, l].type]) {
							WorldGen.SquareTileFrame(k, l);
						}
					}
				}
			}
		}
		public bool GenerateTower()
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
				{0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0},
				{0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
				{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0},
				{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0},

			};

			// Hammer tiles for the tower
			int[,] TowerHammered = new int[,]
			{
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
				{0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},

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
				{0,0,0,0,0,0,0,0,4,8,1,8,0,8,0,2,2,8,8,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,1,8,8,8,8,8,8,8,8,4,1,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,4,1,1,0,0,0,0,0,0},
				{0,0,0,5,5,5,5,5,4,8,8,8,8,8,8,0,0,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,4,8,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,5,4,8,2,2,8,8,8,1,1,1,1,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,1,8,2,2,8,8,8,1,1,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,0,0,8,8,8,1,8,8,4,5,5,5,5,5,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,0,0,8,8,8,1,8,8,4,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,0,8,8,8,1,8,8,4,5,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,1,1,8,8,8,8,8,2,2,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,1,1,8,8,8,8,8,2,2,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,1,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,4,8,8,8,8,8,8,8,8,8,8,0,0,0,0,0,0,0,0,0},
				{0,0,0,5,5,5,5,5,4,8,2,2,8,8,8,8,1,1,1,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,4,2,8,2,8,8,8,8,1,8,8,1,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,1,1,1,8,8,8,8,0,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,1,1,1,8,8,8,8,0,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,0,4,1,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,1,4,1,8,8,8,8,8,8,8,8,8,4,0,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,0,1,1,8,8,8,8,8,8,8,1,1,8,4,1,0,0,0,0,0,0,0},
				{0,0,0,0,4,0,1,1,1,1,1,1,8,8,8,1,1,1,8,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,4,0,1,1,4,8,8,8,8,8,8,8,8,8,8,1,1,1,0,0,0,0,0,0},
				{0,0,0,0,4,0,1,1,1,8,8,8,8,8,8,8,8,8,8,8,1,1,0,0,0,0,0,0},
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
			int attempts = 0;
			while(!placed && attempts++ < 100000) {
				// Select a place in the first 6th of the world, avoiding the oceans
				int towerX = WorldGen.genRand.Next(300, Main.maxTilesX / 6); // from 50 since there's a unaccessible area at the world's borders
																			 // 50% of choosing the last 6th of the world
																			 // Choose which side of the world to be on randomly
				if(WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}

				//Start at 200 tiles above the surface instead of 0, to exclude floating islands
				int towerY = (int)Main.worldSurface - 200;

				// We go down until we hit a solid tile or go under the world's surface
				while(!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}

				// If we went under the world's surface, try again
				if(towerY > Main.worldSurface) {
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if(!(tile.type == TileID.Dirt
					|| tile.type == TileID.Grass
					|| tile.type == TileID.Stone
					|| tile.type == TileID.Mud
					|| tile.type == TileID.FleshGrass
					|| tile.type == TileID.CorruptGrass
					|| tile.type == TileID.JungleGrass
					|| tile.type == TileID.Sand
					|| tile.type == TileID.Crimsand
					|| tile.type == TileID.Ebonsand)) {
					continue;
				}

				// Don't place the tower if the area isn't flat
				if(!CheckFlat(towerX, towerY, TowerShape.GetLength(1), 3))
					continue;

				// place the tower
				PlaceTower(towerX, towerY - 37, TowerShape, TowerHammered, TowerWallsShape, TowerLoot);
				// extend the base a bit
				for(int i = towerX - 2; i < towerX + TowerShape.GetLength(1) - 4; i++) {
					for(int k = towerY + 3; k < towerY + 12; k++) {
						WorldGen.PlaceTile(i, k, TileID.StoneSlab, mute: true, forced: true);
						WorldGen.SlopeTile(i, k);
					}
				}
				// place the Rogue
				int num = NPC.NewNPC((towerX + 12) * 16, (towerY - 24) * 16, NPCType<BoundGambler>(), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num].homeTileX = -1;
				Main.npc[num].homeTileY = -1;
				Main.npc[num].direction = 1;
				Main.npc[num].homeless = true;
				placed = true;
			}
			if(!placed) mod.Logger.Error("Worldgen: FAILED to place Goblin Tower, ground not flat enough?");
			return placed;
		}
		#endregion
		#region Ziggurat
		private void PlaceZiggurat(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
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
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
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
			for(int y = 0; y < LootArray.GetLength(0); y++) {
				for(int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(LootArray[y, x]) {
							case 0:
								break;
							case 4:
								WorldGen.PlaceObject(k, l, TileType<Tiles.Ambient.ScarabIdol>());
								break;
							case 5:
								WorldGen.PlaceChest(k, l, (ushort)TileType<GoldScarabChest>(), false, 0);
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
			while(!placed) {
				// Select a place in the first 6th of the world
				int hideoutX = Main.rand.Next(Main.maxTilesX / 6, Main.maxTilesX / 6 * 5); // from 50 since there's a unaccessible area at the world's borders
																						   // 50% of choosing the last 6th of the world
				if(WorldGen.genRand.NextBool()) {
					hideoutX = Main.maxTilesX - hideoutX;
				}
				int hideoutY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while(!WorldGen.SolidTile(hideoutX, hideoutY) && hideoutY <= Main.worldSurface) {
					hideoutY++;
				}
				// If we went under the world's surface, try again
				if(hideoutY > Main.worldSurface) {
					continue;
				}
				Tile tile = Main.tile[hideoutX, hideoutY];
				// If the type of the tile we are placing the hideout on doesn't match what we want, try again
				if(tile.type != TileID.Sand && tile.type != TileID.Ebonsand && tile.type != TileID.Crimsand && tile.type != TileID.Sandstone) {
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
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
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
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
							case 0:
								break;
							case 3:
								WorldGen.PlaceWall(k, l, 27);
								break;
						}
					}
				}
			}
			for(int y = 0; y < LootArray.GetLength(0); y++) {
				for(int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(LootArray[y, x]) {
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
								if(WorldGen.genRand.NextBool(3)) {
									objects = TileID.Statues;
								} else if(WorldGen.genRand.NextBool(2)) {
									objects = TileID.Anvils;
								} else if(WorldGen.genRand.NextBool(4)) {
									objects = TileID.Pianos;
								} else if(WorldGen.genRand.NextBool(4)) {
									objects = TileID.WorkBenches;
								} else {
									objects = TileID.Pots;
								}
								WorldGen.PlaceObject(k, l, (ushort)objects);  // Misc
								break;
							case 7:
								WorldGen.PlaceObject(k, l - 1, TileType<GemsPickaxeSapphire>());  // Special Pick		
								break;
							case 8:
								if(WorldGen.genRand.NextBool(3)) {
									objects = TileID.Statues;
								} else if(WorldGen.genRand.NextBool(2)) {
									objects = TileID.Anvils;
								} else if(WorldGen.genRand.NextBool(4)) {
									objects = TileID.Pianos;
								} else if(WorldGen.genRand.NextBool(4)) {
									objects = TileID.WorkBenches;
								} else {
									objects = TileID.Pots;
								}
								WorldGen.PlaceObject(k, l, (ushort)objects);  // Another Misc Obj
								break;
							case 9:
								WorldGen.PlaceObject(k, l - 1, TileType<GemsPickaxeRuby>());  // Special Pick		
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
			while(!placed) {
				int hideoutX = (Main.spawnTileX + Main.rand.Next(-800, 800)); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.spawnTileY + Main.rand.Next(120, 400);
				// place the hideout
				if(WorldGen.genRand.Next(2) == 0) {
					PlaceGemStash(hideoutX, hideoutY, StashRoomMain, StashMainWalls, StashMainLoot);
				} else {
					PlaceGemStash(hideoutX, hideoutY, StashRoomMain1, StashMainWalls, StashMainLoot1);
				}
				if(WorldGen.genRand.Next(2) == 0) {
					PlaceGemStash(hideoutX + (Main.rand.Next(-5, 5)), hideoutY - 8, StashRoom1, Stash1Walls, Stash1Loot);
				}
				placed = true;
			}
		}
		#endregion
		#region StructureLoaderMicros
		public void GenerateCrateStash()
		{
			bool placed = false;
			while(!placed) {
				int hideoutX = Main.rand.Next(300, Main.maxTilesX); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 450);
				Tile tile = Main.tile[hideoutX, hideoutY];
				if(!tile.active() || tile.type != TileID.Stone) {
					continue;
				}
				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				StructureLoader.GetStructure("CrateStashRegular").PlaceForce(hideoutX, hideoutY, out containers);
				placed = true;
			}
		}
		public void GenerateBoneIsland(int islands, int section)
        {
            bool placed = false;
            while (!placed)
            {
                {
                    // Select a place in the first 6th of the world
					int sectionSize = Main.maxTilesX / islands;
                    int towerX = Main.rand.Next((sectionSize * section) + 50, (sectionSize * (section + 1)) - 50);
                    int towerY = WorldGen.genRand.Next(Main.maxTilesY / 10, Main.maxTilesY / 9);
                    Tile tile = Main.tile[towerX, towerY];
                    if (tile.active())
                    {
                        continue;
                    }
                    List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
                    Point[] containers = location.ToArray();
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        StructureLoader.GetStructure("BoneIsland").PlaceForce(towerX, towerY, out containers);
                    }
                    else
                    {
                        StructureLoader.GetStructure("BoneIsland1").PlaceForce(towerX, towerY, out containers);
                    }
                    placed = true;
                }
            }
        }
		public void GeneratePagoda()
		{
			bool placed = false;
			pagodaX = 0;
			while(!placed) {
				if(asteroidSide == 0) {
					pagodaX = Main.maxTilesX - Main.rand.Next(200, 350);
				} else {
					pagodaX = Main.rand.Next(200, 350);
				}
				pagodaY = (int)(Main.worldSurface / 5.0);
				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				StructureLoader.GetStructure("Pagoda").PlaceForce(pagodaX, pagodaY, out containers);
				foreach(Point chestLocation in containers) //foreach incase we decide to add a second chest.
				{
					for(int x = 0; x < 2; x++) {
						for(int y = 0; y < 2; y++) {
							Main.tile[chestLocation.X + x, chestLocation.Y + y].active(false);
							Main.tile[chestLocation.X + x, chestLocation.Y + y].type = 0;
						}
					}
					WorldGen.PlaceChest(chestLocation.X, chestLocation.Y + 1, 21, true, 28);
				}
				for(int i = 0; i < Main.rand.Next(8, 10); i++) {
					yureis.Add(new Point((pagodaX + Main.rand.Next(0, 100)) * 16, (pagodaY + Main.rand.Next(-10, 50)) * 16));
                }
				for(int i = 0; i < 3; i++) {
					samurais.Add(new Point((pagodaX + Main.rand.Next(0, 100)) * 16, (pagodaY + Main.rand.Next(-10, 50)) * 16));
                }
				placed = true;
			}
		}
		public void GenerateCrateStashJungle()
		{
			bool placed = false;
			while(!placed) {
				int hideoutX = Main.rand.Next(300, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 450);
				Tile tile = Main.tile[hideoutX, hideoutY];
				if(!tile.active() || tile.type != 60) {
					continue;
				}
				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				StructureLoader.GetStructure("CrateStashJungle").PlaceForce(hideoutX, hideoutY, out containers);
				placed = true;
			}
		}
		public void GenerateStoneDungeon()
		{
			bool placed = false;
			while(!placed) {
				int hideoutX = Main.rand.Next(50, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY);
				Tile tile = Main.tile[hideoutX, hideoutY];
				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				if(!tile.active() || tile.type != TileID.Stone) {
					continue;
				}
				if(WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("StoneDungeon1").PlaceForce(hideoutX, hideoutY, out containers);
				} else if(WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("StoneDungeon2").PlaceForce(hideoutX, hideoutY, out containers);
				} else {
					StructureLoader.GetStructure("StoneDungeon3").PlaceForce(hideoutX, hideoutY, out containers);
				}
				placed = true;
			}
		}
		public void GenerateBismiteCavern()
		{
			bool placed = false;
			while(!placed) {
				int hideoutX = Main.rand.Next(300, Main.maxTilesX - 200); // from 50 since there's a unaccessible area at the world's borders
				int hideoutY = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				Tile tile = Main.tile[hideoutX, hideoutY];
				List<Point> location = new List<Point>(); //these are for ease of use if we ever want to add containers to these existing structures
				Point[] containers = location.ToArray();
				if(!tile.active() || tile.type != TileID.Stone) {
					continue;
				}
				if(WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("BismiteCavern1").PlaceForce(hideoutX, hideoutY, out containers);
				} else if(WorldGen.genRand.Next(2) == 0) {
					StructureLoader.GetStructure("BismiteCavern2").PlaceForce(hideoutX, hideoutY, out containers);
				} else {
					StructureLoader.GetStructure("BismiteCavern3").PlaceForce(hideoutX, hideoutY, out containers);
				}
				placed = true;
			}
		}
		#endregion
		#region Sepulchre
		private void PlaceSepulchre(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
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
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
							case 0:
								break;
							case 1:
								WorldGen.PlaceTile(k, l, ModContent.TileType<SepulchreBrick>());
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
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
							case 0:
								break;
							case 2:
								WorldGen.PlaceWall(k, l, ModContent.WallType<SepulchreWallTile>());
								break;
						}
					}
				}
			}
			for(int y = 0; y < LootArray.GetLength(0); y++) {
				for(int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(LootArray[y, x]) {
							case 3:
								if(Main.rand.Next(2) == 0)
									WorldGen.PlaceObject(k, l, 50, true, Main.rand.Next(0, 5)); //Books
								break;
							case 4:
								WorldGen.PlaceObject(k, l, ModContent.TileType<SepulchreBannerTile>(), true);
								break;
							case 5:
								int pots;
								if(Main.rand.Next(3) == 0) {
									pots = mod.TileType("SepulchrePot1");
								} else if(Main.rand.Next(2) == 0) {
									pots = mod.TileType("SepulchrePot1");
								} else {
									pots = mod.TileType("SepulchrePot2");
								}
								WorldGen.PlaceTile(k, l, (ushort)pots);
								break;
							case 6:
								WorldGen.PlaceChest(k, l, (ushort)ModContent.TileType<SepulchreChestTile>(), false, 0); // Gold Chest
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
			int hideoutX = 0;
			int hideoutY = 0;
			bool validPos = false;
			int attempts = 0;
			while(!validPos && attempts++ < 100000) {
				hideoutX = Main.rand.Next(Main.maxTilesX / 3, Main.maxTilesX / 3 * 2); // from 50 since there's a unaccessible area at the world's borders
				hideoutY = Main.rand.Next(Main.spawnTileY + 200, Main.maxTilesY - 250);
				validPos = true;
				for(int x = hideoutX - 8 - 3; x < hideoutX - 3 + SepulchreRoom1.GetLength(1) + 8; x++) {
					for(int y = hideoutY - 10 - 6; y < hideoutY - 6 + SepulchreRoom1.GetLength(0) * 2 + 9; y++) {
						// Don't allow spawning in pyramids, dungeon, granite, marble, underground desert, other sepulchres...
						if(Main.tile[x, y].active() 
							&& (!CaveHouseBiome._blacklistedTiles[Main.tile[x, y].type]
								|| Main.tile[x, y].type == TileID.Granite
								|| Main.tile[x, y].type == TileID.Marble
								|| Main.tile[x, y].type == TileID.HardenedSand
								|| Main.tile[x, y].type == TileID.MushroomGrass
								|| Main.tile[x, y].type == TileID.WoodBlock
								|| Main.tile[x, y].type == TileID.Mud
								|| Main.tile[x, y].type == TileID.SnowBlock
								|| Main.tile[x, y].type == TileID.IceBlock
								|| Main.tile[x, y].type == TileType<SepulchreBrick>()
								|| Main.tile[x, y].liquid > 0)) {
							validPos = false;
							break;
						}
					}
					if(!validPos) break;
				}
			}
			if(Main.rand.Next(2) == 0) {
				PlaceSepulchre(hideoutX, hideoutY + 9, SepulchreRoom3, SepulchreWalls3, SepulchreLoot3);

				PlaceSepulchre(hideoutX + Main.rand.Next(-8, 8), hideoutY, SepulchreRoom1, SepulchreWalls1, SepulchreLoot1);
			} else {

				PlaceSepulchre(hideoutX, hideoutY - 10, SepulchreRoom3, SepulchreWalls3, SepulchreLoot3);

				PlaceSepulchre(hideoutX, hideoutY + 1, SepulchreRoom2, SepulchreWalls2, SepulchreLoot2);
			}
		}
		#endregion
		#region BanditHideout
		private void PlaceBanditHideout(int i, int j, int[,] BlocksArray, int[,] WallsArray, int[,] LootArray)
		{
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
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
			for(int y = 0; y < LootArray.GetLength(0); y++) {
				for(int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(LootArray[y, x]) {
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
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < WallsArray.GetLength(0); y++) {
				for(int x = 0; x < WallsArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallsArray[y, x]) {
							case 0:
								break;
							case 4:
								WorldGen.PlaceWall(k, l, 4);
								break;
						}
					}
				}
			}
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < LootArray.GetLength(0); y++) {
				for(int x = 0; x < LootArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(LootArray[y, x]) {
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
								// TODO: Add this chest tile so this is valid
								//WorldGen.PlaceChest(k, l, (ushort)TileType<BanditChest>(), false, 0); // Gold Chest
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
			while(!placed) {
				int towerX = WorldGen.genRand.Next(50, Main.maxTilesX / 4);
				if(WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}
				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while(!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}
				// If we went under the world's surface, try again
				if(towerY > Main.worldSurface) {
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if(tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone && tile.type != TileID.SnowBlock) {
					continue;
				}
				PlaceBanditHideout(towerX, towerY - 22, BanditTiles, BanditWalls, BanditLoot);
				int num = NPC.NewNPC((towerX + 31) * 16, (towerY - 20) * 16, ModContent.NPCType<BoundRogue>(), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num].homeTileX = -1;
				Main.npc[num].homeTileY = -1;
				Main.npc[num].direction = 1;
				Main.npc[num].homeless = true;

				placed = true;
			}
		}
		#endregion
		#region SurfaceMicros
		private void PlaceCampsite(int i, int j, int[,] BlocksArray)
		{
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			for(int y = 0; y < BlocksArray.GetLength(0); y++) {
				for(int x = 0; x < BlocksArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BlocksArray[y, x]) {
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
			while(!placed) {
				// Select a place in the first 6th of the world
				int towerX = Main.spawnTileX + Main.rand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
																		  // 50% of choosing the last 6th of the world
				if(WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}
				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while(!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}
				// If we went under the world's surface, try again
				if(towerY > Main.worldSurface) {
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if(tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone) {
					continue;
				}
				// place the tower
				PlaceCampsite(towerX, towerY + 1, CampShape1);
				placed = true;
			}
		}
		private void PlaceGrave(int i, int j, int[,] ShrineArray, int[,] WallArray)
		{

			for(int y = 0; y < WallArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < WallArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(WallArray[y, x]) {
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
								if(Main.rand.Next(2) == 0) {
									WorldGen.PlaceWall(k, l, 106);
								} else {
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
			for(int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(ShrineArray[y, x]) {
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
			for(int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(ShrineArray[y, x]) {
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
			for(int y = 0; y < ShrineArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < ShrineArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(ShrineArray[y, x]) {
							case 2:
								int num256 = Sign.ReadSign(k, l, true);
								if(num256 >= 0) {
									switch(Main.rand.Next(6)) {
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
			while(!placed) {
				// Select a place in the first 6th of the world
				int towerX = Main.spawnTileX + Main.rand.Next(-800, 800); // from 50 since there's a unaccessible area at the world's borders
																		  // 50% of choosing the last 6th of the world
				if(WorldGen.genRand.NextBool()) {
					towerX = Main.maxTilesX - towerX;
				}
				int towerY = 0;
				// We go down until we hit a solid tile or go under the world's surface
				while(!WorldGen.SolidTile(towerX, towerY) && towerY <= Main.worldSurface) {
					towerY++;
				}
				// If we went under the world's surface, try again
				if(towerY > Main.worldSurface) {
					continue;
				}
				Tile tile = Main.tile[towerX, towerY];
				// If the type of the tile we are placing the tower on doesn't match what we want, try again
				if(tile.type != TileID.Dirt && tile.type != TileID.Grass && tile.type != TileID.Stone) {
					continue;
				}
				// place the tower
				PlaceGrave(towerX, towerY + 2, GraveShape, GraveWalls);
				placed = true;
			}
		}
		#endregion
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int GuideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Sunflowers"));
			if(GuideIndex == -1) {
				// Guide pass removed by some other mod.
				return;
			}
			tasks.Insert(GuideIndex, new PassLegacy("SpiritMicros",
				delegate (GenerationProgress progress) {
					bool success = false;
					int attempts = 0;
					while(!success) {
						attempts++;
						if(attempts > 1000) {
							success = true;
							continue;
						}

						progress.Message = "Spirit Mod: Adding Microstructures...";
						{
							if(Main.rand.Next(2) == 0) {
								if(WorldGen.genRand.Next(2) == 0) {
									GenerateGrave();
								}
							} else {
								if(WorldGen.genRand.Next(2) == 0) {
									GenerateCampsite();
								}
							}
							if(Main.rand.Next(2) == 0) {
								gennedBandits = new BanditHideout().Generate();
								//GenerateBanditHideout();
								//gennedBandits = true;
							} else {
								gennedTower = new GoblinTower().Generate();
								//gennedTower = GenerateTower();
							}
							int num584 = 1;
							if(Main.maxTilesX == 4200) {
								num584 = Main.rand.Next(7, 10);
							} else if(Main.maxTilesX == 6400) {
								num584 = Main.rand.Next(12, 14);
							} else if(Main.maxTilesX == 8400) {
								num584 = Main.rand.Next(15, 19);
							}
							for(int k = 0; k < num584 - 2; k++) {
								GenerateCrateStash();
							}
							for(int k = 0; k < (num584 / 2 + 1); k++) {
								GenerateCrateStashJungle();
							}
							for(int k = 0; k < (num584 / 3 * 2 + 2); k++) {
								GenerateBismiteCavern();
							}
							if(WorldGen.genRand.Next(2) == 0) {
								for(int k = 0; k < (num584 / 4); k++) {
									GenerateStoneDungeon();
								}
							}
							int num67 = 1;
							if(Main.maxTilesX == 4200) {
								num67 = Main.rand.Next(12, 15);
							} else if(Main.maxTilesX == 6400) {
								num67 = Main.rand.Next(15, 19);
							} else if(Main.maxTilesX == 8400) {
								num67 = Main.rand.Next(21, 24);
							}
							for(int j = 0; j < num67; j++) {
								GenerateSepulchre();
							}
							for(int k = 0; k < Main.rand.Next(5, 7); k++) {
								GenerateGemStash();
							}
							int num32 = 1;
							if(Main.maxTilesX == 4200) {
								num32 = Main.rand.Next(2, 5);
							} else if(Main.maxTilesX == 6400) {
								num32 = Main.rand.Next(5, 9);
							} else if(Main.maxTilesX == 8400) {
								num32 = Main.rand.Next(7, 12);
							}
                            int num8827 = 1;
                            if (Main.maxTilesX == 4200)
                            {
                                num8827 = 2;
                            }
                            else if (Main.maxTilesX == 6400)
                            {
                                num8827 = 3;
                            }
                            else if (Main.maxTilesX == 8400)
                            {
                                num8827 = 4;
                            }
                            for (int i = 0; i < num8827; i++)
                            {
                                GenerateBoneIsland(num8827, i);
                            }
                            GeneratePagoda();
							GenerateZiggurat();
				
							success = true;
						}
					}
				}));
			int TrapsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Traps"));

			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
			if(ShiniesIndex == -1) {
				// Shinies pass removed by some other mod.
				return;
			}
			tasks.Insert(TrapsIndex + 2, new PassLegacy("Asteroids", delegate (GenerationProgress progress) {
				bool success = false;
				int attempts = 0;
				while(!success) {
					attempts++;
					if(attempts > 1000) {
						success = true;
						continue;
					}
					progress.Message = "Creating an asteroid belt";
					{
						int width = 350;
						int height = 75;
						int x = 0;
						int y = 0;
						if(Main.maxTilesX == 4200) {
							width = 200;
							height = 40;
						}
						if(Main.maxTilesX == 6400) {
							width = 275;
							height = 60;
						}
						if(Main.maxTilesX == 8400) {
							width = 350;
							height = 75;
						}

						if(Main.rand.Next(2) == 0) //change to check for dungeon later, idk how rn.
						{
							x = width + 80;
						} else {
							asteroidSide = 1;
							x = Main.maxTilesX - (width + 80);
						}

						y = height + 60;
						PlaceAsteroids(x, y);

					}
					success = true;
				}
			}));
			tasks.Insert(ShiniesIndex + 1, new PassLegacy("Piles", delegate (GenerationProgress progress) {

				progress.Message = "Spirit Mod: Adding Ambient Objects...";
				{

					if(WorldGen.CopperTierOre == TileID.Copper) {
						for(int i = 0; i < Main.maxTilesX * 19.5; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<CopperPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<CopperPile>(), 0, 0, -1, -1);
							}
						}
					} else if(WorldGen.CopperTierOre == TileID.Tin) {
						for(int i = 0; i < Main.maxTilesX * 19.5; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<TinPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<TinPile>(), 0, 0, -1, -1);
							}
						}
					}
					if(WorldGen.IronTierOre == TileID.Iron) {
						for(int i = 0; i < Main.maxTilesX * 12; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<IronPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<IronPile>(), 0, 0, -1, -1);
							}
						}
					} else if(WorldGen.IronTierOre == TileID.Lead) {
						for(int i = 0; i < Main.maxTilesX * 12; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<LeadPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<LeadPile>(), 0, 0, -1, -1);
							}
						}
					}
					if(WorldGen.SilverTierOre == TileID.Silver) {
						for(int i = 0; i < Main.maxTilesX * 9.75f; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<SilverPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<SilverPile>(), 0, 0, -1, -1);
							}
						}
					} else if(WorldGen.SilverTierOre == TileID.Tungsten) {
						for(int i = 0; i < Main.maxTilesX * 9.75f; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<TungstenPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<TungstenPile>(), 0, 0, -1, -1);
							}
						}
					}
					if(WorldGen.GoldTierOre == TileID.Gold) {
						for(int i = 0; i < Main.maxTilesX * 6f; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<GoldPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<GoldPile>(), 0, 0, -1, -1);
							}
						}
					} else if(WorldGen.GoldTierOre == TileID.Platinum) {
						for(int i = 0; i < Main.maxTilesX * 6f; i++) {
							int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
							int num4 = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							Tile tile = Main.tile[num3, num4];
							if(tile.type == TileID.Stone && tile.active()) {
								WorldGen.PlaceObject(num3, num4 - 1, ModContent.TileType<PlatinumPile>());
								NetMessage.SendObjectPlacment(-1, num3, num4 - 1, ModContent.TileType<PlatinumPile>(), 0, 0, -1, -1);
							}
						}
					}
					for(int C = 0; C < Main.maxTilesX * 45; C++) {
						int[] sculptures = new int[] { ModContent.TileType<IceWheezerPassive>(), ModContent.TileType<IceFlinxPassive>(), ModContent.TileType<IceBatPassive>(), ModContent.TileType<IceVikingPassive>(), ModContent.TileType<IceWheezerHostile>(), ModContent.TileType<IceFlinxHostile>(), ModContent.TileType<IceBatHostile>(), ModContent.TileType<IceVikingHostile>() };
						{
							int X = WorldGen.genRand.Next(0, Main.maxTilesX);
							int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
							if((Main.tile[X, Y].type == TileID.IceBlock || Main.tile[X, Y].type == TileID.SnowBlock) && Main.tile[X, Y + 1].type != 162) {
								WorldGen.PlaceObject(X, Y, (ushort)sculptures[Main.rand.Next(8)]);
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)sculptures[Main.rand.Next(4)], 0, 0, -1, -1);
							}
						}
					}
					for(int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 18.2f) * 6E-03); k++) {
						{
							int X = WorldGen.genRand.Next(0, Main.maxTilesX);
							int Y = WorldGen.genRand.Next(0, Main.maxTilesY);
							if((Main.tile[X, Y].type == ModContent.TileType<Asteroid>() || Main.tile[X, Y].type == ModContent.TileType<BigAsteroid>())) {
								WorldGen.PlaceObject(X, Y, (ushort)ModContent.TileType<BlueShardBig>());
								NetMessage.SendObjectPlacment(-1, X, Y, (ushort)ModContent.TileType<BlueShardBig>(), 0, 0, -1, -1);
							}
						}
					}
				}
			}));
		}
		public override void PostWorldGen()
		{
			int[] commonItems1 = new int[] { ItemID.CopperBar, ItemID.IronBar, ItemID.TinBar, ItemID.LeadBar };
			int[] ammo1 = new int[] { ItemID.WoodenArrow, ItemID.Shuriken };
			int[] potions = new int[] { ItemID.SwiftnessPotion, ItemID.IronskinPotion, ItemID.ShinePotion, ItemID.NightOwlPotion, ItemID.ArcheryPotion, ItemID.HunterPotion };
			int[] recall = new int[] { ItemID.RecallPotion };
			int[] potionscorrupt = new int[] { ItemID.WrathPotion };
			int[] potionscrim = new int[] { ItemID.RagePotion, ItemID.HeartreachPotion };
			int[] other1 = new int[] { ItemID.HerbBag, ItemID.Grenade };
			int[] other2 = new int[] { ItemID.Bottle, ItemID.Torch };
			int[] moddedMaterials = new int[] { ItemType<BismiteCrystal>(), ItemType<OldLeather>() };
			int stack;
				
			foreach (Point spawn in yureis)
			{
				int num = NPC.NewNPC(spawn.X, spawn.Y, ModContent.NPCType<PagodaGhostPassive>(), 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[num].homeTileX = -1;
                    Main.npc[num].homeTileY = -1;
                    Main.npc[num].direction = 1;
                    Main.npc[num].homeless = true;
                    Main.npc[num].townNPC = true;
			}
			foreach (Point spawn in samurais)
			{
				int num = NPC.NewNPC(spawn.X, spawn.Y, ModContent.NPCType<SamuraiPassive>(), 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num].homeTileX = -1;
                    Main.npc[num].homeTileY = -1;
                    Main.npc[num].direction = 1;
                    Main.npc[num].homeless = true;
                    Main.npc[num].townNPC = true;
			}
			//int itemsToPlaceInPagodaChestsChoice = 0;
			for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
				if(chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 28 * 36) {
					for(int inventoryIndex = 0; inventoryIndex < 5; inventoryIndex++) {
						if(chest.item[inventoryIndex].type == ItemID.None) {

							if(inventoryIndex == 0) {
								int[] itemsToPlaceInPagodaChests1 = { ItemType<JadeStaff>(), ItemType<JadeStaff>() };
								stack = 1;
								chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInPagodaChests1));
								chest.item[inventoryIndex].stack = stack;
							}
							if(inventoryIndex == 1) {
								int[] itemsToPlaceInPagodaChests2 = { ItemType<DynastyFan>(), ItemType<DynastyFan>() };
								stack = 1;
								chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInPagodaChests2));
								chest.item[inventoryIndex].stack = stack;
							}
							chest.item[2].SetDefaults(commonItems1[Main.rand.Next(4)], false);
							chest.item[2].stack = WorldGen.genRand.Next(3, 10);
							chest.item[3].SetDefaults(ammo1[Main.rand.Next(2)], false);
							chest.item[3].stack = WorldGen.genRand.Next(20, 50);
							chest.item[4].SetDefaults(potions[Main.rand.Next(6)], false);
							chest.item[4].stack = WorldGen.genRand.Next(2, 3);
							chest.item[5].SetDefaults(recall[Main.rand.Next(1)], false);
							chest.item[5].stack = WorldGen.genRand.Next(2, 3);
							chest.item[6].SetDefaults(other1[Main.rand.Next(2)], false);
							chest.item[6].stack = WorldGen.genRand.Next(1, 4);
							chest.item[7].SetDefaults(other2[Main.rand.Next(2)], false);
							chest.item[7].stack = WorldGen.genRand.Next(1, 4);
							chest.item[8].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
							chest.item[8].stack = WorldGen.genRand.Next(2, 6);
							chest.item[9].SetDefaults(72, false);
							chest.item[9].stack = WorldGen.genRand.Next(12, 30);
						}
					}
				}
				if(chest != null && Main.tile[chest.x, chest.y].type == TileType<AsteroidChest>()) {
					for(int inventoryIndex = 0; inventoryIndex < 5; inventoryIndex++) {
						if(chest.item[inventoryIndex].type == ItemID.None) {

							if(inventoryIndex == 0) {
								int[] itemsToPlaceInPagodaChests1 = { ItemType<ZiplineGun>(), ItemType<HighGravityBoots>(), ItemType<MagnetHook>() };
								stack = 1;
								chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInPagodaChests1));
								chest.item[inventoryIndex].stack = stack;
							}
							if(inventoryIndex == 1) {
								if(WorldGen.genRand.Next(2) == 0) {
									int[] itemsToPlaceInPagodaChests2 = { ItemType<JumpPadItem>(), ItemID.SuspiciousLookingEye };
									stack = 1;
									chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInPagodaChests2));
									chest.item[inventoryIndex].stack = stack;
								} else if(WorldGen.genRand.Next(2) == 0) {
									int[] itemsToPlaceInPagodaChests2 = { ItemType<TargetCan>() };
									stack = WorldGen.genRand.Next(10, 15);
									chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInPagodaChests2));
									chest.item[inventoryIndex].stack = stack;
								} else {
									int[] itemsToPlaceInPagodaChests2 = { ItemType<SpaceJunkItem>(), ItemType<SpaceJunkItem>() };
									stack = WorldGen.genRand.Next(30, 55);
									chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInPagodaChests2));
									chest.item[inventoryIndex].stack = stack;
								}
							}
							chest.item[2].SetDefaults(commonItems1[Main.rand.Next(4)], false);
							chest.item[2].stack = WorldGen.genRand.Next(3, 10);
							chest.item[3].SetDefaults(ammo1[Main.rand.Next(2)], false);
							chest.item[3].stack = WorldGen.genRand.Next(20, 50);
							chest.item[4].SetDefaults(potions[Main.rand.Next(6)], false);
							chest.item[4].stack = WorldGen.genRand.Next(2, 3);
							chest.item[5].SetDefaults(recall[Main.rand.Next(1)], false);
							chest.item[5].stack = WorldGen.genRand.Next(2, 3);
							chest.item[6].SetDefaults(other1[Main.rand.Next(2)], false);
							chest.item[6].stack = WorldGen.genRand.Next(1, 4);
							chest.item[7].SetDefaults(other2[Main.rand.Next(2)], false);
							chest.item[7].stack = WorldGen.genRand.Next(1, 4);
							chest.item[8].SetDefaults(moddedMaterials[Main.rand.Next(2)], false);
							chest.item[8].stack = WorldGen.genRand.Next(2, 6);
							chest.item[9].SetDefaults(72, false);
							chest.item[9].stack = WorldGen.genRand.Next(12, 30);
						}
					}
				}
			}
			//Tile tile;
			for(int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 3.2f) * 15E-05); k++) {
				int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				if(Main.tile[EEXX, WHHYY] != null) {
					if(Main.tile[EEXX, WHHYY].active()) {
						if(Main.tile[EEXX, WHHYY].type == 161) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)ModContent.TileType<CryoliteOreTile>());
						} else if(Main.tile[EEXX, WHHYY].type == 163) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)ModContent.TileType<CryoliteOreTile>());
						} else if(Main.tile[EEXX, WHHYY].type == 164) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)ModContent.TileType<CryoliteOreTile>());
						} else if(Main.tile[EEXX, WHHYY].type == 200) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)ModContent.TileType<CryoliteOreTile>());
						}
					}
				}
			}
			for(int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 5.5f) * 15E-05); k++) {
				int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				if(Main.tile[EEXX, WHHYY] != null) {
					if(Main.tile[EEXX, WHHYY].active()) {
						if(Main.tile[EEXX, WHHYY].type == 161) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(6, 7), (ushort)ModContent.TileType<CreepingIceTile>());
						} else if(Main.tile[EEXX, WHHYY].type == 163) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(6, 7), (ushort)ModContent.TileType<CreepingIceTile>());
						} else if(Main.tile[EEXX, WHHYY].type == 164) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(6, 7), (ushort)ModContent.TileType<CreepingIceTile>());
						} else if(Main.tile[EEXX, WHHYY].type == 200) {
							WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(6, 7), (ushort)ModContent.TileType<CreepingIceTile>());
						}
					}
				}
			}
			for(int i = 1; i < Main.rand.Next(4, 6); i++) {
				//int itemsToPlaceInGlassChestsSecondaryChoice = 0;
				for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
					Chest chest = Main.chest[chestIndex];
					if(chest != null && Main.tile[chest.x, chest.y].frameX == 13 * 36 && Main.rand.Next(3) == 0) {
						chest.item[6].SetDefaults(ItemType<Items.Consumable.ChaosPearl>(), false);
						chest.item[6].stack = WorldGen.genRand.Next(20, 30);
					}
					if(chest != null && Main.tile[chest.x, chest.y].frameX == 2 * 36 && Main.rand.Next(10) == 0) {
						if(WorldGen.crimson) {
							chest.item[1].SetDefaults(ItemType<Tenderizer>(), false);
						} else {
							chest.item[1].SetDefaults(ItemType<Slugger>(), false);
						}
					}

				}
			}
            for (int i = 1; i < Main.rand.Next(4, 6); i++)
            {
                int[] itemsToPlacePrimary = new int[] { ItemType<SepulchreStaff>(), ItemType<SepulchrePendant>() };
                int[] ammoToPlace = new int[] { ItemType<SepulchreArrow>(), ItemType<SepulchreBullet>() };
                //int itemsToPlaceInGlassChestsSecondaryChoice = 0;	
                for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
                {
                    Chest chest = Main.chest[chestIndex];
                    if (chest != null && Main.tile[chest.x, chest.y].type == TileType<SepulchreChestTile>())
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

                }
            }
            for (int i = 1; i < Main.rand.Next(4, 6); i++) {
				int[] itemsToPlacePrimary = new int[] { ItemType<CleftHorn>(), ItemType<CactusStaff>() };
				//int itemsToPlaceInGlassChestsSecondaryChoice = 0;
				for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
					Chest chest = Main.chest[chestIndex];
					if(chest != null && Main.tile[chest.x, chest.y].type == TileType<GoldScarabChest>()) {
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
			for(int i = 1; i < Main.rand.Next(4, 6); i++) {
				int[] itemsToPlacePrimary = new int[] { ModContent.ItemType<Glyph>(), ItemID.MagicMirror, ItemID.WandofSparking };
				//int itemsToPlaceInGlassChestsSecondaryChoice = 0;
				for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
					Chest chest = Main.chest[chestIndex];
					if(chest != null && Main.tile[chest.x, chest.y].type == ModContent.TileType<GoblinChest>()) {
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
						chest.item[7].stack = WorldGen.genRand.Next(2, 6);
						chest.item[8].SetDefaults(72, false);
						chest.item[8].stack = WorldGen.genRand.Next(12, 30);
						break;
					}
				}
				int[] itemsToPlacePrimary1 = new int[] { ModContent.ItemType<Glyph>(), ItemID.Radar, ItemID.Blowpipe, ItemID.WandofSparking };
				for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
					Chest chest = Main.chest[chestIndex];
					if(chest != null && Main.tile[chest.x, chest.y].type == ModContent.TileType<ReachHideoutWoodChest>()) {
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
				for(int i = 1; i < Main.rand.Next(4, 6); i++) {
					//int itemsToPlaceInGlassChestsSecondaryChoice = 0;
					for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
						Chest chest = Main.chest[chestIndex];
						if(chest != null && Main.tile[chest.x, chest.y].type == ModContent.TileType<ReachChest>()) {
							for(int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++) {
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
								if(Main.rand.Next(4) == 0) {
									chest.item[9].SetDefaults(ModContent.ItemType<GladeWreath>(), false);
								}

							}
						}
					}
				}
			}
			int[] itemsToPlaceInGlassChests = new int[] { ModContent.ItemType<ReachChestMagic>(), ModContent.ItemType<Items.Tool.ThornHook>(), ModContent.ItemType<ReachStaffChest>(), ModContent.ItemType<Items.Weapon.Returning.ReachBoomerang>(), ModContent.ItemType<ReachBrooch>() };
			int itemsToPlaceInGlassChestsChoice = 0;
			for(int chestIndex = 0; chestIndex < 1000; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if(chest != null && Main.tile[chest.x, chest.y].type == ModContent.TileType<ReachChest>()) {
					for(int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++) {
						itemsToPlaceInGlassChestsChoice = Main.rand.Next(itemsToPlaceInGlassChests.Length);
						chest.item[0].SetDefaults(itemsToPlaceInGlassChests[itemsToPlaceInGlassChestsChoice]);

					}
				}
			}
		}
		private void PlaceBoneIsland(int i, int j, int[,] BoneIslandArray)
		{

			for(int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for(int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BoneIslandArray[y, x]) {
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
			for(int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BoneIslandArray[y, x]) {
							case 3:
								WorldGen.PlaceTile(k, l, (ushort)mod.TileType("SkullPile1")); // Gold Chest
								break;
							case 4:
								WorldGen.PlaceTile(k, l, (ushort)mod.TileType("SkullPile2")); // Gold Chest
								break;
							case 5:
								WorldGen.PlaceTile(k, l - 1, (ushort)ModContent.TileType<AvianEgg>()); // Gold Chest
								break;
						}
					}
				}
			}
		}
		private void PlaceBoneIslandWalls(int i, int j, int[,] BoneIslandArray)
		{

			for(int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for(int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BoneIslandArray[y, x]) {
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
			for(int y = 0; y < BoneIslandArray.GetLength(0); y++) { // This Loop Placzs Furnitures.(So that they have blocks to spawn on).
				for(int x = 0; x < BoneIslandArray.GetLength(1); x++) {
					int k = i - 3 + x;
					int l = j - 6 + y;
					if(WorldGen.InWorld(k, l, 30)) {
						Tile tile = Framing.GetTileSafely(k, l);
						switch(BoneIslandArray[y, x]) {
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
			if(modPlayer.ZoneSpirit) {
				if(!aurora) {
					aurora = true;
				}
				auroraType = 5;
			}
			if(Main.bloodMoon) {
				MyWorld.auroraType = 6;
			}
			if(Main.pumpkinMoon) {
				MyWorld.auroraType = 7;
			}
			if(Main.snowMoon) {
				auroraType = 8;
			}

			if(!Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon && !modPlayer.ZoneSpirit) {
				auroraType = auroraTypeFixed;
			}
			if(Main.dayTime != dayTimeLast)
				dayTimeSwitched = true;
			else
				dayTimeSwitched = false;
			dayTimeLast = Main.dayTime;

			if(dayTimeSwitched) {
				if(Main.rand.Next(2) == 0 && !spaceJunkWeather) {
					stardustWeather = true;
				} else {
					stardustWeather = false;
				}
				if(Main.rand.Next(2) == 0 && !stardustWeather) {
					spaceJunkWeather = true;
				} else {
					spaceJunkWeather = false;
				}
				if(Main.rand.Next(4) == 0) {
					meteorShowerWeather = true;
				} else {
					meteorShowerWeather = false;
				}
				if(!Main.dayTime && Main.hardMode) {
					if(!Main.fastForwardTime && !Main.bloodMoon && WorldGen.spawnHardBoss == 0 && ((Main.rand.Next(20) == 1 && !downedBlueMoon) || (Main.rand.Next(40) == 1 && !downedBlueMoon))) {
						Main.NewText("A Mystic Moon is rising...", 0, 90, 220);
						BlueMoon = true;
						downedBlueMoon = true;
					}
				} else {
					BlueMoon = false;
				}
				{
					if(!Main.dayTime && Main.rand.Next(6) == 0) {
						auroraTypeFixed = Main.rand.Next(new int[] { 1, 2, 3, 5 });
						aurora = true;
					} else {
						aurora = false;
					}
				}
				if(!Main.dayTime && Main.rand.Next(22) == 0) {
					luminousType = Main.rand.Next(new int[] { 1, 2, 3 });
					luminousOcean = true;
				} else {
					luminousOcean = false;
				}
			}
			if(NPC.downedBoss2) {
				if(!gmOre) {
					for(int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 76) * 15E-05); k++) {
						int EEXX = WorldGen.genRand.Next(0, Main.maxTilesX);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
						if(Main.tile[EEXX, WHHYY] != null) {
							if(Main.tile[EEXX, WHHYY].active()) {
								if(Main.tile[EEXX, WHHYY].type == 368) {
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)ModContent.TileType<GraniteOre>());
								}
							}
						}
					}
					for(int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 200) * 15E-05); k++) {
						int EEXX = WorldGen.genRand.Next(0, Main.maxTilesX);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
						if(Main.tile[EEXX, WHHYY] != null) {
							if(Main.tile[EEXX, WHHYY].active()) {
								if(Main.tile[EEXX, WHHYY].type == 367) {
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(4, 9), (ushort)ModContent.TileType<MarbleOre>());
								}
							}
						}
					}
					{
						Main.NewText("Energy seeps into marble and granite caverns...", 61, 255, 142);
						gmOre = true;
					}
				}
			}
			if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				if(!essenceMessage) {
					Main.NewText("The Essences are bursting!", 61, 255, 142);

					essenceMessage = true;
				}
			}
			if(NPC.downedPlantBoss) {
				if(!Thermite) {
					for(int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY * 1.13f) * 15E-05); k++) {
						int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 500);
						if(Main.tile[EEXX, WHHYY] != null) {
							if(Main.tile[EEXX, WHHYY].active()) {
								if(Main.tile[EEXX, WHHYY].type == 1) {
									WorldGen.OreRunner(EEXX, WHHYY, WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(4, 6), (ushort)TileType<Tiles.Block.ThermiteOre>());
								}
							}
						}
					}
					Main.NewText("The Caverns have been flooded with lava!", 61, 255, 142);
					Thermite = true;
				}
			}

			if(Main.hardMode && !rockCandy) {
				rockCandy = true;
				//Main.NewText("ROCK CANDAYYYYYYYYYYY", Color.Orange.R, Color.Orange.G, Color.Orange.B);
				for(int C = 0; C < Main.maxTilesX * 9; C++) {
					{
						int X = WorldGen.genRand.Next(0, Main.maxTilesX);
						int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
						if(Main.tile[X, Y].type == TileID.Stone) {
							WorldGen.PlaceObject(X, Y, ModContent.TileType<GreenShardBig>());
							NetMessage.SendObjectPlacment(-1, X, Y, ModContent.TileType<GreenShardBig>(), 0, 0, -1, -1);
						}
					}


				}
				for(int C = 0; C < Main.maxTilesX * 9; C++) {
					{
						int X = WorldGen.genRand.Next(0, Main.maxTilesX);
						int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
						if(Main.tile[X, Y].type == TileID.Stone) {
							WorldGen.PlaceObject(X, Y, ModContent.TileType<PurpleShardBig>());
							NetMessage.SendObjectPlacment(-1, X, Y, ModContent.TileType<PurpleShardBig>(), 0, 0, -1, -1);
						}
					}


				}
			}
			if(NPC.downedMechBoss3 == true || NPC.downedMechBoss2 == true || NPC.downedMechBoss1 == true) {
				if(!spiritBiome) {
					spiritBiome = true;
					Main.NewText("The Spirits spread through the Land...", Color.Orange.R, Color.Orange.G, Color.Orange.B);
					Random rand = new Random();
					int XTILE;
					if(Terraria.Main.dungeonX > Main.maxTilesX / 2) //rightside dungeon
					{
						XTILE = WorldGen.genRand.Next((Main.maxTilesX / 2) + 300, Main.maxTilesX - 500);
					} else //leftside dungeon
					  {
						XTILE = WorldGen.genRand.Next(75, (Main.maxTilesX / 2) - 600);
					}
					int xAxis = XTILE;
					int xAxisMid = xAxis + 70;
					int xAxisEdge = xAxis + 380;
					int yAxis = 0;
					for(int y = 0; y < Main.maxTilesY; y++) {
						yAxis++;
						xAxis = XTILE;
						for(int i = 0; i < 450; i++) {
							xAxis++;
							if(Main.tile[xAxis, yAxis] != null) {
								if(Main.tile[xAxis, yAxis].active()) {
									int[] TileArray = { 0 };
									if(TileArray.Contains(Main.tile[xAxis, yAxis].type)) {
										if(Main.tile[xAxis, yAxis + 1] == null) {
											if(Main.rand.Next(0, 50) == 1) {
												WillGenn = 0;
												if(xAxis < xAxisMid - 1) {
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if(xAxis > xAxisEdge + 1) {
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if(WillGenn < 10) {
													Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritDirt>();
												}
											}
										} else {
											WillGenn = 0;
											if(xAxis < xAxisMid - 1) {
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if(xAxis > xAxisEdge + 1) {
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if(WillGenn < 10) {
												Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritDirt>();
											}
										}
									}
									int[] TileArray84 = { 2, 23, 109, 199 };
									if(TileArray84.Contains(Main.tile[xAxis, yAxis].type)) {
										if(Main.tile[xAxis, yAxis + 1] == null) {
											if(rand.Next(0, 50) == 1) {
												WillGenn = 0;
												if(xAxis < xAxisMid - 1) {
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if(xAxis > xAxisEdge + 1) {
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if(WillGenn < 18) {
													Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritGrass>();
												}
											}
										} else {
											WillGenn = 0;
											if(xAxis < xAxisMid - 1) {
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if(xAxis > xAxisEdge + 1) {
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if(WillGenn < 18) {
												Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritGrass>();
											}
										}
									}
									int[] TileArray1 = { 161, 163, 164, 200 };
									if(TileArray1.Contains(Main.tile[xAxis, yAxis].type)) {
										if(Main.tile[xAxis, yAxis + 1] == null) {
											if(rand.Next(0, 50) == 1) {
												WillGenn = 0;
												if(xAxis < xAxisMid - 1) {
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if(xAxis > xAxisEdge + 1) {
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if(WillGenn < 18) {
													Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritIce>();
												}
											}
										} else {
											WillGenn = 0;
											if(xAxis < xAxisMid - 1) {
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if(xAxis > xAxisEdge + 1) {
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if(WillGenn < 18) {
												Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritIce>();
											}
										}
									}
									int[] TileArray2 = { 1, 25, 117, 203 };
									if(TileArray2.Contains(Main.tile[xAxis, yAxis].type)) {
										if(Main.tile[xAxis, yAxis + 1] == null) {
											if(rand.Next(0, 50) == 1) {
												WillGenn = 0;
												if(xAxis < xAxisMid - 1) {
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if(xAxis > xAxisEdge + 1) {
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if(WillGenn < 18) {
													Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritStone>();
												}
											}
										} else {
											WillGenn = 0;
											if(xAxis < xAxisMid - 1) {
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if(xAxis > xAxisEdge + 1) {
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if(WillGenn < 18) {
												Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<SpiritStone>();
											}
										}
									}

									int[] TileArray89 = { 3, 24, 110, 113, 115, 201, 205, 52, 62, 32, 165 };
									if(TileArray89.Contains(Main.tile[xAxis, yAxis].type)) {
										if(Main.tile[xAxis, yAxis + 1] == null) {
											if(rand.Next(0, 50) == 1) {
												WillGenn = 0;
												if(xAxis < xAxisMid - 1) {
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if(xAxis > xAxisEdge + 1) {
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if(WillGenn < 18) {
													Main.tile[xAxis, yAxis].active(false);
												}
											}
										} else {
											WillGenn = 0;
											if(xAxis < xAxisMid - 1) {
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if(xAxis > xAxisEdge + 1) {
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if(WillGenn < 18) {
												Main.tile[xAxis, yAxis].active(false);
											}
										}
									}


									int[] TileArray3 = { 53, 116, 112, 234 };
									if(TileArray3.Contains(Main.tile[xAxis, yAxis].type)) {
										if(Main.tile[xAxis, yAxis + 1] == null) {
											if(rand.Next(0, 50) == 1) {
												WillGenn = 0;
												if(xAxis < xAxisMid - 1) {
													Meme = xAxisMid - xAxis;
													WillGenn = Main.rand.Next(Meme);
												}
												if(xAxis > xAxisEdge + 1) {
													Meme = xAxis - xAxisEdge;
													WillGenn = Main.rand.Next(Meme);
												}
												if(WillGenn < 18) {
													Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<Spiritsand>();
												}
											}
										} else {
											WillGenn = 0;
											if(xAxis < xAxisMid - 1) {
												Meme = xAxisMid - xAxis;
												WillGenn = Main.rand.Next(Meme);
											}
											if(xAxis > xAxisEdge + 1) {
												Meme = xAxis - xAxisEdge;
												WillGenn = Main.rand.Next(Meme);
											}
											if(WillGenn < 18) {
												Main.tile[xAxis, yAxis].type = (ushort)ModContent.TileType<Spiritsand>();
											}
										}
									}
                                }
                                if (Main.tile[xAxis, yAxis].type == mod.TileType("SpiritStone") && yAxis > (int)((Main.rockLayer + Main.maxTilesY - 500) / 2f) && Main.rand.Next(1500) == 5)
                                {
                                    WorldGen.TileRunner(xAxis, yAxis, (double)WorldGen.genRand.Next(5, 7), 1, mod.TileType("SpiritOreTile"), false, 0f, 0f, true, true);
                                }
                            }
						}
					}
                }
			}
		}






		#region Gray's stupid shenanagins with boffins godlike code


		
		#endregion
	}
}