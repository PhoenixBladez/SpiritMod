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
using System.Runtime.Serialization.Formatters.Binary;

namespace SpiritMod
{
	public class MyWorld : ModWorld
	{
		private static bool dayTimeLast;
		public static bool dayTimeSwitched;

		public static bool BlueMoon = false;
		public static int SpiritTiles = 0;
		public static int ReachTiles = 0;
		public static bool Magicite = false;
		public static bool Thermite = false;
		public static bool Cryolite = false;
		public static bool spiritBiome = false;
		public static bool gmOre = false;
		public static bool starMessage = false;
		public static bool essenceMessage = false;
		public static bool flierMessage = false;

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
			ReachTiles = tileCounts[mod.TileType("SkullStick")] +tileCounts[mod.TileType("SkullStick2")] + tileCounts[mod.TileType("ReachGrassTile")];
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

			TagCompound droppedGlyphTag = tag.GetCompound("droppedGlyphs");
			droppedGlyphs.Clear();
			foreach (KeyValuePair<string, object> entry in droppedGlyphTag)
			{
				droppedGlyphs.Add(entry.Key, entry.Value is byte ? (byte)entry.Value != 0 : entry.Value as bool? ?? false);
			}

			BlueMoon = tag.GetBool("blueMoon");
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

		public void PlaceReach(int x, int y)
		{
			//initial pit
			WorldMethods.TileRunner(x, y, (double)150, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, true, true); //improve basic shape later
			bool leftpit = false;
			int PitX;
			int PitY;
			if (Main.rand.Next(2) == 0)
			{
				leftpit = true;
			}
			if (leftpit)
			{
				PitX = x - Main.rand.Next(5, 15);
			}
			else
			{
				PitX = x + Main.rand.Next(5, 15);
			}
			for (PitY = y - 16; PitY < y + 25; PitY++)
			{
				WorldGen.digTunnel(PitX, PitY, 0, 0, 1, 4, false);
				WorldGen.TileRunner(PitX, PitY, 11, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, false, true);
			}
			//tunnel off of pit
			int tunnellength = Main.rand.Next(50, 110);
			int TunnelEndX = 0;
			if (leftpit)
			{
				for (int TunnelX = PitX; TunnelX < PitX + tunnellength; TunnelX++)
				{
					WorldGen.digTunnel(TunnelX, PitY, 0, 0, 1, 4, false);
					WorldGen.TileRunner(TunnelX, PitY, 13, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, false, true);
					TunnelEndX = TunnelX;
				}
			}
			else
			{
				for (int TunnelX = PitX; TunnelX > PitX - tunnellength; TunnelX--)
				{
					WorldGen.digTunnel(TunnelX, PitY, 0, 0, 1, 4, false);
					WorldGen.TileRunner(TunnelX, PitY, 13, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, false, true);
					TunnelEndX = TunnelX;
				}
			}
			//More pits and spikes
			int TrapX;
			for (int TrapNum = 0; TrapNum < 10; TrapNum++)
			{
				if (leftpit)
				{
					TrapX = Main.rand.Next(PitX, PitX + tunnellength);
				}
				else
				{
					TrapX = Main.rand.Next(PitX - tunnellength, PitX);
				}
				for (int TrapY = PitY; TrapY < PitY + 15; TrapY++)
				{
					WorldGen.digTunnel(TrapX, TrapY, 0, 0, 1, 3, false);
					WorldGen.TileRunner(TrapX, TrapY, 11, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, false, true);
				}
				WorldGen.TileRunner(TrapX, PitY + 18, 9, 1, 48, false, 0f, 0f, false, true);
			}
			//Additional hole and tunnel
			int PittwoY = 0;
			for (PittwoY = PitY; PittwoY < PitY + 40; PittwoY++)
			{
				WorldGen.digTunnel(TunnelEndX, PittwoY, 0, 0, 1, 4, false);
				WorldGen.TileRunner(TunnelEndX, PittwoY, 11, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, false, true);
			}
			int PittwoX = 0;
			for (PittwoX = TunnelEndX - 50; PittwoX < TunnelEndX + 50; PittwoX++)
			{
				WorldGen.digTunnel(PittwoX, PittwoY, 0, 0, 1, 4, false);
				WorldGen.TileRunner(PittwoX, PittwoY, 13, 1, mod.TileType("ReachGrassTile"), false, 0f, 0f, false, true);
				WorldGen.PlaceChest(PittwoX, PittwoY, 21, false, 2);
				WorldGen.PlaceChest(PittwoX + 5, PittwoY + 3, 21, false, 2);
				WorldGen.PlaceChest(PittwoX + 1, PittwoY + 2, 21, false, 2);
			}
			//grass walls
			for (int wallx = x - 100; wallx < x + 100; wallx++)
			{
				for (int wally = y - 25; wally < y + 100; wally++)
				{
					if (Main.tile[wallx, wally].wall != 0)
					{
						WorldGen.KillWall(wallx, wally);
						WorldGen.PlaceWall(wallx, wally, 63);
					}
				}
			}
			//campfires and shit
			int SkullStickY = 0;
			Tile tile = Main.tile[1, 1];
			for (int SkullStickX = x - 90; SkullStickX < x + 90; SkullStickX++)
			{
				if (Main.rand.Next(4) == 1)
				{
					for (SkullStickY = y - 80; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0)
						{
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 2, 215);//i dont know which of these is correct but i cant be bothered to test.
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 1, 215);
							WorldGen.PlaceObject(SkullStickX, SkullStickY, 215);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 2, 215, 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 1, 215, 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY, 215, 0, 0, -1, -1);
						}
					}
				}
				if (Main.rand.Next(9) == 1)
				{
					for (SkullStickY = y - 60; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0 || tile.type == mod.TileType("ReachGrassTile"))
						{
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 3, mod.TileType("SkullStick")); //i dont know which of these is correct but i cant be bothered to test.
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 2, mod.TileType("SkullStick"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 1, mod.TileType("SkullStick"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY, mod.TileType("SkullStick"));
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 3, mod.TileType("SkullStick"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 2, mod.TileType("SkullStick"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 1, mod.TileType("SkullStick"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY, mod.TileType("SkullStick"), 0, 0, -1, -1);
						}
					}
				}
				if (Main.rand.Next(12) == 1)
				{
					for (SkullStickY = y - 60; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0 || tile.type == mod.TileType("ReachGrassTile"))
						{
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 3, mod.TileType("SkullStick2")); //i dont know which of these is correct but i cant be bothered to test.
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 2, mod.TileType("SkullStick2"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 1, mod.TileType("SkullStick2"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY, mod.TileType("SkullStick2"));
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 3, mod.TileType("SkullStick2"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 2, mod.TileType("SkullStick2"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 1, mod.TileType("SkullStick2"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY, mod.TileType("SkullStick2"), 0, 0, -1, -1);
						}
					}
				}
				if (Main.rand.Next(10) == 1)
				{
					for (SkullStickY = y - 60; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0 || tile.type == mod.TileType("ReachGrassTile"))
						{
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 3, mod.TileType("SkullStick3")); //i dont know which of these is correct but i cant be bothered to test.
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 2, mod.TileType("SkullStick3"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 1, mod.TileType("SkullStick3"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY, mod.TileType("SkullStick3"));
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 3, mod.TileType("SkullStick3"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 2, mod.TileType("SkullStick3"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 1, mod.TileType("SkullStick3"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY, mod.TileType("SkullStick3"), 0, 0, -1, -1);
						}
					}
				}
				if (Main.rand.Next(25) == 1)
				{
					for (SkullStickY = y - 60; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0 || tile.type == mod.TileType("ReachGrassTile"))
						{
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 3, mod.TileType("CreationAltarTile")); //i dont know which of these is correct but i cant be bothered to test.
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 2, mod.TileType("CreationAltarTile"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 1, mod.TileType("CreationAltarTile"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY, mod.TileType("CreationAltarTile"));
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 3, mod.TileType("CreationAltarTile"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 2, mod.TileType("CreationAltarTile"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 1, mod.TileType("CreationAltarTile"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY, mod.TileType("CreationAltarTile"), 0, 0, -1, -1);
						}
					}
				}
				if (Main.rand.Next(10) == 1)
				{
					for (SkullStickY = y - 60; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0 || tile.type == mod.TileType("ReachGrassTile"))
						{
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 3, mod.TileType("ReachGrass1")); //i dont know which of these is correct but i cant be bothered to test.
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 2, mod.TileType("ReachGrass1"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY - 1, mod.TileType("ReachGrass1"));
							WorldGen.PlaceObject(SkullStickX, SkullStickY, mod.TileType("ReachGrass1"));
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 3, mod.TileType("ReachGrass1"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 2, mod.TileType("ReachGrass1"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY - 1, mod.TileType("ReachGrass1"), 0, 0, -1, -1);
							NetMessage.SendObjectPlacment(-1, SkullStickX, SkullStickY, mod.TileType("ReachGrass1"), 0, 0, -1, -1);
						}
					}
				}
				if (Main.rand.Next(16) == 1)
				{
					for (SkullStickY = y - 60; SkullStickY < y + 75; SkullStickY++)
					{
						tile = Main.tile[SkullStickX, SkullStickY];
						if (tile.type == 2 || tile.type == 1 || tile.type == 0 || tile.type == mod.TileType("ReachGrassTile"))
						{
							WorldGen.PlaceChest(SkullStickX, SkullStickY - 3, (ushort)mod.TileType("ReachChest"), false, 0);
							WorldGen.PlaceChest(SkullStickX, SkullStickY - 2, (ushort)mod.TileType("ReachChest"), false, 0);
							WorldGen.PlaceChest(SkullStickX, SkullStickY - 1, (ushort)mod.TileType("ReachChest"), false, 0);
						}
					}
				}
			}
			//loot placement
			for (PittwoX = TunnelEndX - 20; PittwoX < TunnelEndX + 20; PittwoX++)
			{
				if (Main.rand.Next(30) == 1)
				{
					Main.tile[PittwoX, PittwoY + 1].active(true);
					Main.tile[PittwoX + 1, PittwoY + 1].active(true);
					Main.tile[PittwoX, PittwoY + 1].type = 1;
					Main.tile[PittwoX + 1, PittwoY + 1].type = 1;
					WorldGen.AddLifeCrystal(PittwoX + 1, PittwoY);
					WorldGen.AddLifeCrystal(PittwoX + 1, PittwoY + 1);
					break;
				}
			}
			for (int trees = 0; trees < 5000; trees++)
			{
				int E = x + Main.rand.Next(-200, 200);
				int F = y  + Main.rand.Next(-30, 30);
				tile = Framing.GetTileSafely(E, F);
				if (tile.type == mod.TileType("ReachGrassTile"))
				{
					WorldGen.GrowTree(E, F);
				}
			}
		}

		static bool CanPlaceReach(int x, int y)
		{
			for (int i = x - 32; i < x + 32; i++)
			{
				for (int j = y - 32; j < y + 32; j++)
				{
					int[] TileArray = { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.Cloud, TileID.RainCloud,
						TileID.SnowBlock, TileID.JungleGrass, TileID.Sand, TileID.ClayBlock, TileID.FleshGrass, TileID.CorruptGrass, TileID.Ebonstone, TileID.Crimstone };
					for (int block = 0; block < TileArray.Length; block++)
					{
						if (Main.tile[i, j].type == (ushort)TileArray[block])
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{

			int GuideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Guide"));
			if (GuideIndex == -1)
			{
				// Guide pass removed by some other mod.
				return;
			}
			tasks.Insert(GuideIndex + 1, new PassLegacy("Reach", 
				delegate (GenerationProgress progress)
			{
				progress.Message = "Creating Hostile Settlements";
				bool placed = false;
				bool success = false;
				int inset = 200;
				int spawnProtect = Main.maxTilesX >> 5;
				int spawnStart = (Main.maxTilesX >> 1) - (spawnProtect >> 1);
				int limit = Main.maxTilesX - spawnProtect - inset;
				int attempts = 0;
				int x = 0;
				int y = (int)WorldGen.worldSurface;
				while (!success)
				{
					attempts++;
					if (attempts > 1000)
					{
						success = true;
						continue;
					}
					x = WorldGen.genRand.Next(inset, limit);
					if (x > spawnStart)
						x += spawnProtect;
					y = (int)WorldGen.worldSurfaceLow;
					while (!Main.tile[x, y].active() && (double)y < Main.worldSurface)
					{
						y++;
					}
					if (Main.tile[x, y].type == TileID.Grass || Main.tile[x, y].type == TileID.Dirt)
					{
						y--;
						if (y > 150 && CanPlaceReach(x, y))
						{
							success = true;
							placed = true;
							continue;
						}
					}
				}
				PlaceReach(x, y);
			}));

			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			if (ShiniesIndex == -1)
			{
				// Shinies pass removed by some other mod.
				return;
			}
			tasks.Insert(ShiniesIndex + 1, new PassLegacy("Rune Shrines", delegate (GenerationProgress progress)
			{
				progress.Message = "Honoring the Dead...";
				for (int num = 0; num < Main.maxTilesX / 390; num++)
				{
					int xAxis = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
					int yAxis = WorldGen.genRand.Next((int)WorldGen.rockLayer + 150, Main.maxTilesY - 250);
					WorldMethods.RoundHill2(xAxis, yAxis, 30, 30, 16, true, 2);
					for (int A = xAxis-40; A < xAxis+40; A++)
					{
						for (int B = yAxis-40; B < yAxis+40; B++)
						{
							if (Main.tile[A, B] != null)
							{
								if (Main.tile[A, B].type == TileID.Grass) // A = x, B = y.
								{
									WorldGen.KillWall(A, B);
									WorldGen.PlaceWall(A, B, 65);
								}
							}
						}
					}
					for (int RuneX = xAxis - 45; RuneX < xAxis + 45; RuneX++)
					{
						if (Main.rand.Next(4) == 1)
						{
							for (int RuneY = yAxis - 45; RuneY < yAxis + 45; RuneY++)
							{
								Tile tile = Main.tile[RuneX, RuneY];
								if (tile.type == 2 || tile.type == 0 && Main.rand.Next(15) == 0)
								{
									WorldGen.PlaceObject(RuneX, RuneY - 2, mod.TileType("RuneStone"));//i dont know which of these is correct but i cant be bothered to test.
									WorldGen.PlaceObject(RuneX, RuneY - 1, mod.TileType("RuneStone"));
									WorldGen.PlaceObject(RuneX, RuneY, mod.TileType("RuneStone"));
									NetMessage.SendObjectPlacment(-1, RuneX, RuneY - 2, mod.TileType("RuneStone"), 0, 0, -1, -1);
									NetMessage.SendObjectPlacment(-1, RuneX, RuneY - 1, mod.TileType("RuneStone"), 0, 0, -1, -1);
									NetMessage.SendObjectPlacment(-1, RuneX, RuneY, mod.TileType("RuneStone"), 0, 0, -1, -1);
								}
							}
						}

					}
				}
			}));
		}

		public override void Initialize()
		{
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

		public override void PostWorldGen()
		{
			for (int i = 1; i < 2; i++)
			{
				if (Main.rand.Next(20) == 0)
				{
					int[] itemsToPlaceInGlassChestsSecondary = new int[] { Items.Glyphs.Glyph._type };
					int itemsToPlaceInGlassChestsSecondaryChoice = 0;
					for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
					{
						Chest chest = Main.chest[chestIndex];
						if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("ReachChest"))
						{
							for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
							{
								if (chest.item[inventoryIndex].type == 0)
								{
									chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGlassChestsSecondary[itemsToPlaceInGlassChestsSecondaryChoice]); //the error is at this line
									chest.item[inventoryIndex].stack = Main.rand.Next(1, 1);
									itemsToPlaceInGlassChestsSecondaryChoice = (itemsToPlaceInGlassChestsSecondaryChoice + 1) % itemsToPlaceInGlassChestsSecondary.Length;
									break;
								}
							}
						}
					}
				}
			}
			{
				for (int i = 1; i < Main.rand.Next(4, 6); i++)
				{
					int[] itemsToPlaceInGlassChestsSecondary = new int[] { mod.ItemType("BismiteCrystal"), mod.ItemType("AncientBark"), ItemID.SilverCoin, ItemID.Bottle, ItemID.Rope };
					int itemsToPlaceInGlassChestsSecondaryChoice = 0;
					for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
					{
						Chest chest = Main.chest[chestIndex];
						if (chest != null && Main.tile[chest.x, chest.y].type == mod.TileType("ReachChest"))
						{
							for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
							{
								if (chest.item[inventoryIndex].type == 0)
								{
									chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGlassChestsSecondary[itemsToPlaceInGlassChestsSecondaryChoice]); //the error is at this line
									chest.item[inventoryIndex].stack = Main.rand.Next(4, 10);
									itemsToPlaceInGlassChestsSecondaryChoice = (itemsToPlaceInGlassChestsSecondaryChoice + 1) % itemsToPlaceInGlassChestsSecondary.Length;
									break;
								}
							}
						}
					}
				}
			}
			int[] itemsToPlaceInGlassChests = new int[] { mod.ItemType("ReachStaffChest"), mod.ItemType("ReachBoomerang"), mod.ItemType("ReachBrooch") };
			int itemsToPlaceInGlassChestsChoice = 0;
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type/*.frameX == 47 * 36*/ == mod.TileType("ReachChest")) // if glass chest
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						itemsToPlaceInGlassChestsChoice = Main.rand.Next(itemsToPlaceInGlassChests.Length);
						chest.item[0].SetDefaults(itemsToPlaceInGlassChests[itemsToPlaceInGlassChestsChoice]);
						//itemsToPlaceInGlassChestsChoice = (itemsToPlaceInGlassChestsChoice + 1) % itemsToPlaceInGlassChests.Length;
						break;
					}
				}
			}
		}

		public override void PostUpdate()
		{
			if (Main.dayTime != dayTimeLast)
				dayTimeSwitched = true;
			else
				dayTimeSwitched = false;
			dayTimeLast = Main.dayTime;

			if (dayTimeSwitched && Main.hardMode)
			{
				if (!Main.dayTime)
				{
					if (!Main.fastForwardTime && !Main.bloodMoon && WorldGen.spawnHardBoss == 0 &&
						NPC.downedMechBossAny && Main.rand.Next(20) == 0)
					{
						Main.NewText("A Blue Moon is rising...", 0, 90, 220);
						BlueMoon = true;
					}
				}
				else
				{
					BlueMoon = false;
				}
			}

			if (NPC.downedBoss1)
			{
				if (!Magicite)
				{
					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 13) * 15E-05); k++)
					{
						int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
						if (Main.tile[EEXX, WHHYY] != null)
						{
							if (Main.tile[EEXX, WHHYY].active())
							{
								if (Main.tile[EEXX, WHHYY].type == 60)
								{
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)mod.TileType("FloranOreTile"));
								}
							}
						}
					}
					Main.NewText("The Underground Jungle seems to be glowing...", 100, 220, 100);
					Main.NewText("New enemies have spawned forth underground", 204, 153, 0);
					Magicite = true;
				}
			}
			if (NPC.downedBoss2)
			{
				if (!gmOre)
				{
					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 37) * 15E-05); k++)
					{
						int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
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
					for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY * 29) * 15E-05); k++)
					{
						int EEXX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
						int WHHYY = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 130);
						if (Main.tile[EEXX, WHHYY] != null)
						{
							if (Main.tile[EEXX, WHHYY].active())
							{
								if (Main.tile[EEXX, WHHYY].type == 367)
								{
									WorldGen.OreRunner(EEXX, WHHYY, (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)mod.TileType("MarbleOre"));
								}
							}
						}
					}
					{
						Main.NewText("Energy seeps into Marble and Granite caverns...", 100, 220, 100);
						gmOre = true;
					}
				}
			}
			if (NPC.downedQueenBee)
			{
				if (!flierMessage)
				{
					Main.NewText("Scattered bones rise into the sky...", 204, 153, 0);
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
					starMessage = true;
					if (!txt)
					{
						Main.NewText("The stars are brightening...", 66, 170, 244);
						Main.NewText("The Icy Caverns are shimmering!", 70, 170, 255);
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
							#region islands
							if (Main.rand.Next(21000) == 1)
							{
								WorldMethods.Island(xAxis, Main.rand.Next(100, 275), Main.rand.Next(10, 16), (float)(Main.rand.Next(11, 25) / 10), (ushort)mod.TileType("SpiritGrass"));
							}
							#endregion
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
			/* if (Main.hardMode)
             {
                 if (!VerdantBiome)
                 {
                     Main.NewText("The World's Primal Life begins anew", Color.Orange.R, Color.Orange.G, Color.Orange.B);
                     VerdantBiome = true;
                     for (int i = 0; i < ((int)Main.maxTilesX / 250) * 3; i++)
                     {
                         int Xvalue = WorldGen.genRand.Next(50, Main.maxTilesX - 700);
                         int Yvalue = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY - 300);
                         int XvalueHigh = Xvalue + 240;
                         int YvalueHigh = Yvalue + 160;
                         int XvalueMid = Xvalue + 120;
                         int YvalueMid = Yvalue + 80;
                         if (Main.tile[XvalueMid, YvalueMid] != null)
                         {
                             if (Main.tile[XvalueMid, YvalueMid].type == 1) // A = x, B = y.
                             {
                                 WorldGen.TileRunner(XvalueMid, YvalueMid, (double)WorldGen.genRand.Next(80, 80), 1, mod.TileType("VeridianDirt"), false, 0f, 0f, true, true); //c = x, d = y
                                 WorldGen.TileRunner(XvalueMid + 20, YvalueMid, (double)WorldGen.genRand.Next(80, 80), 1, mod.TileType("VeridianDirt"), false, 0f, 0f, true, true); //c = x, d = y
                                 WorldGen.TileRunner(XvalueMid + 40, YvalueMid, (double)WorldGen.genRand.Next(80, 80), 1, mod.TileType("VeridianDirt"), false, 0f, 0f, true, true); //c = x, d = y
                                 WorldGen.TileRunner(XvalueMid + 60, YvalueMid, (double)WorldGen.genRand.Next(80, 80), 1, mod.TileType("VeridianDirt"), false, 0f, 0f, true, true);
                                 WorldGen.TileRunner(XvalueMid + 80, YvalueMid, (double)WorldGen.genRand.Next(80, 80), 1, mod.TileType("VeridianDirt"), false, 0f, 0f, true, true);//c = x, d = y
                                                                                                                                                                                         for (int A = Xvalue; A < XvalueHigh; A++)
                                                                                                                                                                                           {
                                                                                                                                                                                               for (int B = Yvalue; B < YvalueHigh; B++)
                                                                                                                                                                                               {
                                                                                                                                                                                                   if (Main.tile[A,B] != null)
                                                                                                                                                                                                   {
                                                                                                                                                                                                       if (Main.tile[A,B].type ==  mod.TileType("CrystalBlock")) // A = x, B = y.
                                                                                                                                                                                                       { 
                                                                                                                                                                                                           WorldGen.KillWall(A, B);
                                                                                                                                                                                                           WorldGen.PlaceWall(A, B, mod.WallType("CrystalWall"));
                                                                                                                                                                                                       }
                                                                                                                                                                                                   }
                                                                                                                                                                                               }
                                                                                                                                                                                           }
                                 WorldGen.digTunnel(XvalueMid, YvalueMid, WorldGen.genRand.Next(0, 360), WorldGen.genRand.Next(0, 360), WorldGen.genRand.Next(10, 11), WorldGen.genRand.Next(8, 10), false);
                                 WorldGen.digTunnel(XvalueMid + 50, YvalueMid, WorldGen.genRand.Next(0, 360), WorldGen.genRand.Next(0, 360), WorldGen.genRand.Next(10, 11), WorldGen.genRand.Next(8, 10), false);
                                 for (int Ore = 0; Ore < 75; Ore++)
                                 {
                                     int Xore = XvalueMid + Main.rand.Next(100);
                                     int Yore = YvalueMid + Main.rand.Next(-70, 70);
                                     if (Main.tile[Xore, Yore].type == mod.TileType("VeridianDirt")) // A = x, B = y.
                                     {
                                         WorldGen.TileRunner(Xore, Yore, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(3, 6), mod.TileType("VeridianStone"), false, 0f, 0f, false, true);
                                     }
                                 }
                             }
                         }
                     }
                 }
             } */
		}
	}
}