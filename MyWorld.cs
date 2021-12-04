using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Ammo.Arrow;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Glyphs;
using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Items.Sets.StarplateDrops;
using SpiritMod.Items.Sets.BriarChestLoot;
using SpiritMod.Items.Sets.ToolsMisc.Evergreen;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Magic.CreepingVine;
using SpiritMod.Items.Books;
using SpiritMod.Items.Books.MaterialPages;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.NPCs.Town;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.SpaceCrystals;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Furniture;
using SpiritMod.Tiles.Furniture.SpaceJunk;
using SpiritMod.World;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using SpiritMod.World.Sepulchre;
using SpiritMod.Tiles;
using Terraria.DataStructures;
using SpiritMod.Utilities;
using SpiritMod.Items.Sets.SepulchreLoot.ToxicBottle;
using SpiritMod.Items.Sets.SepulchreLoot.AccursedBlade;
using SpiritMod.Items.Sets.SepulchreLoot.OldCross;
using SpiritMod.Mechanics.BackgroundSystem;
using SpiritMod.Items.Sets.SummonsMisc.Toucane;
using static Terraria.ModLoader.ModContent;
using static SpiritMod.Utilities.ChestPoolUtils;

namespace SpiritMod
{
	public class MyWorld : ModWorld
	{
		public static float rottime = 0;
		private static bool dayTimeLast;
		public static bool dayTimeSwitched;

		public static bool aurora = false;
		public static bool ashRain = false;
		public static int auroraType = 1;
		public static int auroraTypeFixed;
		public static int auroraChance = 4;

		public static bool luminousOcean = false;
		public static bool calmNight = false;
		public static int luminousType = 1;

		public static bool stardustWeather = false;
		public static bool spaceJunkWeather = false;
		public static bool meteorShowerWeather = false;

		public static float asteroidLight = 0;
		public static float spiritLight = 0;

		public static bool BlueMoon = false;
		public static bool jellySky = false;
		public static bool rareStarfallEvent = false;

		public static int SpiritTiles = 0;
		public static int AsteroidTiles = 0;
		public static int MarbleTiles = 0;
		public static int GraniteTiles = 0;
		public static int ReachTiles = 0;
		public static int HiveTiles = 0;
		public static int CorruptHazards = 0;
		public static int CrimHazards = 0;

		public static bool Magicite = false;
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
		public static bool downedMoonWizard = false;
		public static bool downedReachBoss = false;
		public static bool downedDusking = false;

		public static bool downedMechromancer = false;
		public static bool downedOccultist = false;
		public static bool downedGladeWraith = false;
		public static bool downedTome = false;
		public static bool downedSnaptrapper = false;
		public static bool downedBeholder = false;
		public static bool downedJellyDeluge = false;
		public static bool downedTide = false;
		public static bool downedBlueMoon = false;
		public static bool downedGazer = false;

		//pagoda enemy spawn variables
		public static int pagodaX = 0;
		public static int pagodaY = 0;
		public static bool spawnedPagodaEnemies = false;

		public static Dictionary<string, bool> droppedGlyphs = new Dictionary<string, bool>();

		public static HashSet<Point16> superSunFlowerPositions = new HashSet<Point16>();

		//bool night = false;
		public bool txt = false;

		public override void TileCountsAvailable(int[] tileCounts)
		{
			SpiritTiles = tileCounts[TileType<SpiritDirt>()] + tileCounts[TileType<SpiritStone>()]
			+ tileCounts[TileType<Spiritsand>()] + tileCounts[TileType<SpiritIce>()] + tileCounts[TileType<SpiritGrass>()];
			//now you don't gotta have 6 separate things for tilecount
			ReachTiles = tileCounts[TileType<BriarGrass>()];
			AsteroidTiles = tileCounts[TileType<Asteroid>()] + tileCounts[TileType<BigAsteroid>()] + tileCounts[TileType<SpaceJunkTile>()] + tileCounts[TileType<Glowstone>()];
			CorruptHazards = tileCounts[TileType<Corpsebloom>()] + tileCounts[TileType<Corpsebloom1>()] + tileCounts[TileType<Corpsebloom2>()];
			CrimHazards = tileCounts[TileType<CrimsonPustuleTile>()];
			MarbleTiles = tileCounts[TileID.Marble];
			GraniteTiles = tileCounts[TileID.Granite];
			HiveTiles = tileCounts[TileID.Hive];
		}

		public override TagCompound Save()
		{
			var data = new TagCompound();
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
			if (downedMoonWizard)
				downed.Add("moonWizard");
			if (downedDusking)
				downed.Add("dusking");
			if (downedAtlas)
				downed.Add("atlas");
			if (downedBlueMoon)
				downed.Add("bluemoon");
			if (downedJellyDeluge)
				downed.Add("jellyDeluge");
			if (downedTide)
				downed.Add("tide");
			if (downedMechromancer)
				downed.Add("mechromancer");
			if (downedOccultist)
				downed.Add("occultist");
			if (downedGladeWraith)
				downed.Add("gladeWraith");
			if (downedTome)
				downed.Add("hauntedTome");
			if (downedBeholder)
				downed.Add("beholder");
			if (downedSnaptrapper)
				downed.Add("snaptrapper");
			if (downedGazer)
				downed.Add("bloodGazer");

			data.Add("downed", downed);

			TagCompound droppedGlyphTag = new TagCompound();
			foreach (KeyValuePair<string, bool> entry in droppedGlyphs)
			{
				droppedGlyphTag.Add(entry.Key, entry.Value);
			}
			data.Add("droppedGlyphs", droppedGlyphTag);

			data.Add("blueMoon", BlueMoon);
			data.Add("jellySky", jellySky);
			data.Add("gennedBandits", gennedBandits);
			data.Add("gennedTower", gennedTower); ;

			data.Add("pagodaX", pagodaX);
			data.Add("pagodaY", pagodaY);
			data.Add("spawnedPagodaEnemies", spawnedPagodaEnemies);

			//SaveSpecialNPCs(data);

			data.Add("superSunFlowerPositions", superSunFlowerPositions.ToList());

			if (BackgroundItemManager.Loaded)
			{
				List<TagCompound> backgroundItems = BackgroundItemManager.Save();
				data.Add("backgroundItems", backgroundItems);
			}
			data.Add("asteroidSide", asteroidSide);
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
			downedMoonWizard = downed.Contains("moonWizard");
			downedAtlas = downed.Contains("atlas");
			downedTide = downed.Contains("tide");
			downedMechromancer = downed.Contains("mechromancer");
			downedOccultist = downed.Contains("occultist");
			downedGladeWraith = downed.Contains("gladeWraith");
			downedTome = downed.Contains("hauntedTome");
			downedBeholder = downed.Contains("beholder");
			downedSnaptrapper = downed.Contains("snaptrapper");
			downedGazer = downed.Contains("bloodGazer");
			downedBlueMoon = downed.Contains("bluemoon");
			downedJellyDeluge = downed.Contains("jellyDeluge");
			//LoadSpecialNPCs(tag);
			TagCompound droppedGlyphTag = tag.GetCompound("droppedGlyphs");
			droppedGlyphs.Clear();
			foreach (KeyValuePair<string, object> entry in droppedGlyphTag)
			{
				droppedGlyphs.Add(entry.Key, entry.Value is byte ? (byte)entry.Value != 0 : entry.Value as bool? ?? false);
			}

			BlueMoon = tag.GetBool("blueMoon");
			jellySky = tag.GetBool("jellySky");
			gennedBandits = tag.GetBool("gennedBandits");
			gennedTower = tag.GetBool("gennedTower");

			pagodaX = tag.Get<int>("pagodaX");
			pagodaY = tag.Get<int>("pagodaY");
			spawnedPagodaEnemies = tag.Get<bool>("spawnedPagodaEnemies");

			superSunFlowerPositions = new HashSet<Point16>(tag.GetList<Point16>("superSunFlowerPositions"));
			// verify that there are super sunflowers at the loaded positions
			foreach (Point16 point in superSunFlowerPositions.ToList())
				if (Framing.GetTileSafely(point).type != TileType<SuperSunFlower>())
					superSunFlowerPositions.Remove(point);

			var bgItems = tag.GetList<TagCompound>("backgroundItems");
			BackgroundItemManager.Load(bgItems);
			asteroidSide = tag.Get<int>("asteroidSide");
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			int loadVersion = reader.ReadInt32();
			if (loadVersion == 0)
			{
				BitsByte flags = reader.ReadByte();
				BitsByte flags1 = reader.ReadByte();
				BitsByte flags2 = reader.ReadByte();
				BitsByte flags3 = reader.ReadByte();
				BitsByte flags4 = reader.ReadByte();

				downedScarabeus = flags[0];
				downedAncientFlier = flags[1];
				downedRaider = flags[2];
				downedInfernon = flags[3];
				downedDusking = flags[4];
				downedAtlas = flags[6];
				downedBlueMoon = flags[8];

				downedReachBoss = flags1[0];
				downedMoonWizard = flags1[1];
				downedTide = flags1[2];
				downedMechromancer = flags1[3];
				downedOccultist = flags2[4];
				downedGladeWraith = flags2[5];
				downedBeholder = flags2[6];
				downedSnaptrapper = flags2[7];
				downedJellyDeluge = flags2[8];

				gennedBandits = flags2[0];
				gennedTower = flags2[1];

			}
			else
			{
				mod.Logger.Error("Unknown loadVersion: " + loadVersion);
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte bosses1 = new BitsByte(downedScarabeus, downedAncientFlier, downedRaider, downedInfernon, downedDusking, downedAtlas, downedReachBoss, downedMoonWizard);
			BitsByte bosses2 = new BitsByte(downedTide, downedMechromancer, downedOccultist, downedGladeWraith, downedBeholder, downedSnaptrapper, downedTome, downedGazer);
			writer.Write(bosses1);
			writer.Write(bosses2);
			BitsByte environment = new BitsByte(BlueMoon, jellySky, downedBlueMoon, downedJellyDeluge);
			BitsByte worldgen = new BitsByte(gennedBandits, gennedTower);
			writer.Write(environment);
			writer.Write(worldgen);
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
			downedAtlas = bosses1[5];
			downedReachBoss = bosses1[6];
			downedMoonWizard = bosses1[7];

			downedTide = bosses2[0];
			downedMechromancer = bosses2[1];
			downedOccultist = bosses2[2];
			downedGladeWraith = bosses2[3];
			downedBeholder = bosses2[4];
			downedSnaptrapper = bosses2[5];
			downedTome = bosses2[6];
			downedGazer = bosses2[7];

			BitsByte environment = reader.ReadByte();
			BlueMoon = environment[0];
			jellySky = environment[1];
			downedBlueMoon = environment[2];
			downedJellyDeluge = environment[3];

			BitsByte worldgen = reader.ReadByte();
			gennedBandits = worldgen[0];
			gennedTower = worldgen[1];
		}

		public override void PreUpdate()
		{
			rottime += (float)Math.PI / 60;
			if (rottime >= Math.PI * 2) rottime = 0;
		}

		public override void Initialize()
		{
			BlueMoon = false;
			jellySky = false;
			ashRain = false;
			dayTimeLast = Main.dayTime;
			dayTimeSwitched = false;
			BossTitles.Reset();
			if (!Main.dedServ)
				AdditiveCallManager.Load();

			if (SpiritMod.TrailManager != null)
				SpiritMod.TrailManager.ClearAllTrails(); //trails break on world unload and reload(their projectile is still counted as being active???), so this just clears them all on reload

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

			if (Main.hardMode)
				rockCandy = true;
			else
				rockCandy = false;

			if (NPC.downedBoss3)
				starMessage = true;
			else
				starMessage = false;

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
			downedMoonWizard = false;
			downedTide = false;
			downedMechromancer = false;
			downedOccultist = false;
			downedGladeWraith = false;
			downedBeholder = false;
			downedSnaptrapper = false;
			downedTome = false;

			//Portrait system - Gabe
			//PortraitManager.Load(); //Load portraits so the detour can access them
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
			if (!WorldGen.SolidTile(startX + width, startY)) return false;

			float totalVariance = 0;
			for (int i = 0; i < width; i++)
			{
				if (startX + i >= Main.maxTilesX) return false;

				// Fail if there is a tile very closely above the check area
				for (int k = startY - 1; k > startY - 100; k--)
				{
					if (WorldGen.SolidTile(startX + i, k)) return false;
				}

				// If the tile is solid, go up until we find air
				// If the tile is not, go down until we find a floor
				int offset = 0;
				bool goingUp = WorldGen.SolidTile(startX + i, startY);
				offset += goingUp ? goingUpWeight : goingDownWeight;
				while ((goingUp && WorldGen.SolidTile(startX + i, startY - offset))
					|| (!goingUp && !WorldGen.SolidTile(startX + i, startY + offset)))
				{
					offset++;
				}
				if (goingUp) offset--; // account for going up counting the first tile
				totalVariance += offset;
			}
			return totalVariance / width <= threshold;
		}

		#region MageTower
		private void PlaceTower(int i, int j, int[,] ShrineArray, int[,] HammerArray, int[,] WallsArray, int[,] LootArray)
		{
			for (int y = 0; y < WallsArray.GetLength(0); y++)
			{ // This Loop Places Furnitures.(So that they have blocks to spawn on).
				for (int x = 0; x < WallsArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x])
						{
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
			for (int y = 0; y < ShrineArray.GetLength(0); y++)
			{ // This loops clears the area (makes the proper hemicircle) and placed dirt in the bottom if there are no blocks (so that the chest and fireplace can be placed).
				for (int x = 0; x < ShrineArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (ShrineArray[y, x])
						{
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
			for (int y = 0; y < HammerArray.GetLength(0); y++)
			{
				for (int x = 0; x < HammerArray.GetLength(1); x++)
				{
					int k = i - 3 + x;
					int l = j - 6 + y;
					if (WorldGen.InWorld(k, l, 30))
					{
						WorldGen.SlopeTile(k, l, HammerArray[y, x]);
						if (TileID.Sets.Platforms[Main.tile[k, l].type])
						{
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
			while (!placed && attempts++ < 100000)
			{
				// Select a place in the first 6th of the world, avoiding the oceans
				int towerX = WorldGen.genRand.Next(300, Main.maxTilesX / 6); // from 50 since there's a unaccessible area at the world's borders
																			 // 50% of choosing the last 6th of the world
																			 // Choose which side of the world to be on randomly
				if (WorldGen.genRand.NextBool()) towerX = Main.maxTilesX - towerX;

				//Start at 200 tiles above the surface instead of 0, to exclude floating islands
				int towerY = (int)Main.worldSurface - 200;

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
				if (!(tile.type == TileID.Dirt
					|| tile.type == TileID.Grass
					|| tile.type == TileID.Stone
					|| tile.type == TileID.Mud
					|| tile.type == TileID.FleshGrass
					|| tile.type == TileID.CorruptGrass
					|| tile.type == TileID.JungleGrass
					|| tile.type == TileID.Sand
					|| tile.type == TileID.Crimsand
					|| tile.type == TileID.Ebonsand))
				{
					continue;
				}

				// Don't place the tower if the area isn't flat
				if (!CheckFlat(towerX, towerY, TowerShape.GetLength(1), 3))
					continue;

				// place the tower
				PlaceTower(towerX, towerY - 37, TowerShape, TowerHammered, TowerWallsShape, TowerLoot);
				// extend the base a bit
				for (int i = towerX - 2; i < towerX + TowerShape.GetLength(1) - 4; i++)
				{
					for (int k = towerY + 3; k < towerY + 12; k++)
					{
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
			if (!placed) mod.Logger.Error("Worldgen: FAILED to place Goblin Tower, ground not flat enough?");
			return placed;
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
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (WallsArray[y, x])
						{
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
					if (WorldGen.InWorld(k, l, 30))
					{
						Tile tile = Framing.GetTileSafely(k, l);
						switch (BlocksArray[y, x])
						{
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
			while (!placed)
			{
				int towerX = WorldGen.genRand.Next(50, Main.maxTilesX / 4);
				if (WorldGen.genRand.NextBool()) towerX = Main.maxTilesX - towerX;
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

		public override void ResetNearbyTileEffects()
		{
			MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
			modPlayer.ZoneSynthwave = false;
			modPlayer.ZoneLantern = false;
		}
		#endregion
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			int Sunflowers = tasks.FindIndex(genpass => genpass.Name.Equals("Sunflowers"));
			if (Sunflowers != -1) //Add only if Sunflowers pass exists
				tasks.Insert(Sunflowers, new PassLegacy("SpiritMicros", SpiritGenPasses.MicrosPass));

			int FinalCleanup = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
			if (FinalCleanup != -1) //Add only if Final Cleanup exists...
				tasks.Insert(FinalCleanup + 1, new PassLegacy("Piles", SpiritGenPasses.PilesPass));

			int TrapsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Traps"));
			tasks.Insert(TrapsIndex + 2, new PassLegacy("Asteroids", SpiritGenPasses.AsteroidsPass));

			int beachIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Beaches")); //Replace beach gen
			if (beachIndex != -1)
				tasks[beachIndex] = new PassLegacy("New Beach", OceanGeneration.GenerateOcean);
		}

		public override void PostWorldGen()
		{
			int[] commonItems1 = new int[] { ItemID.CopperBar, ItemID.IronBar, ItemID.TinBar, ItemID.LeadBar };
			int[] ammo1 = new int[] { ItemID.WoodenArrow, ItemID.Shuriken };
			int[] potions = new int[] { ItemID.SwiftnessPotion, ItemID.IronskinPotion, ItemID.ShinePotion, ItemID.NightOwlPotion, ItemID.ArcheryPotion, ItemID.HunterPotion };
			int[] potionscorrupt = new int[] { ItemID.WrathPotion };
			int[] potionscrim = new int[] { ItemID.RagePotion, ItemID.HeartreachPotion };
			int[] other1 = new int[] { ItemID.HerbBag, ItemID.Grenade };
			int[] other2 = new int[] { ItemID.Bottle, ItemID.Torch };
			int[] moddedMaterials = new int[] { ItemType<Items.Sets.BismiteSet.BismiteCrystal>(), ItemType<OldLeather>() };

			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 15E-05); k++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				Tile t = Framing.GetTileSafely(x, y);
				if (t.active() && t.type == TileID.IceBlock || t.type == TileID.CorruptIce || t.type == TileID.HallowedIce || t.type == TileID.FleshIce)
					WorldGen.OreRunner(x, y, WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 6), (ushort)TileType<Items.Sets.CryoliteSet.CryoliteOreTile>());
			}
			for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY * 5.5f) * 15E-05); k++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
				Tile t = Framing.GetTileSafely(x, y);
				if (t.active() && t.type == TileID.IceBlock || t.type == TileID.CorruptIce || t.type == TileID.HallowedIce || t.type == TileID.FleshIce)
					WorldGen.OreRunner(x, y, WorldGen.genRand.Next(6, 7), WorldGen.genRand.Next(6, 7), (ushort)TileType<CreepingIceTile>());
			}

			AddToVanillaChest(new ChestInfo(ItemType<ChaosPearl>(), Main.rand.Next(20, 30)), skyChests, 4);
			AddToVanillaChest(new ChestInfo(new int[] {
				ItemType<CimmerianScepter>() },
				1, 0.33f), lockedgoldChests, 1);
			AddToVanillaChest(new ChestInfo(new int[] {
				ItemType<Items.Sets.SummonsMisc.FairyWhistle.FairyWhistleItem>() },
				1, 0.2f), woodChests, 1);
			AddToVanillaChest(new ChestInfo(new int[] { ItemType<MetalBand>(), ItemType<ShortFuse>(), ItemType<LongFuse>() }, 1, 0.1f), goldChests, 1);
			AddToVanillaChest(new ChestInfo(ItemType<HollowNail>()), spiderChests, 1);
			AddToVanillaChest(new ChestInfo(new int[] {
				ItemType<Book_AccessoryGuide>(),
				ItemType<Book_Alchemist1>(),
				ItemType<Book_ArmorGuide>(),
				ItemType<Book_FoodGuide>(),
				ItemType<Book_WeaponGuide>(),
				ItemType<BismitePage>() },
				1, 0.5f), woodChests, 1);
			AddToVanillaChest(new ChestInfo(new int[] {
				ItemType<Book_Lumoth>(),
				ItemType<GranitePage>(),
				ItemType<MarblePage>(),
				ItemType<EnchantedLeafPage>(),
				ItemType<HeartScalePage>(),
				ItemType<FrigidFragmentPage>(),
				ItemType<BismitePage>(),
				ItemType<GlowrootPage>(),
				ItemType<Book_Soulbloom>(),
				ItemType<Book_Blossmoon>(),
				ItemType<FrigidFragmentPage>(),
				ItemType<Book_Amea>(),
				ItemType<Book_Slime>(),
				ItemType<Book_Lava>(),
				ItemType<Book_MJW>(),
				ItemType<Book_Yeremy>(),
				ItemType<Book_Mushroom>(),
				ItemType<Book_Jellyfish>(),
				ItemType<Book_Gunslinger>()},
				1, 0.33f), goldChests, 2);
			AddToVanillaChest(new ChestInfo(ItemType<Book_LuminousArt>(), 1, 0.33f), waterChests, 2);
			AddToVanillaChest(new ChestInfo(new int[] { ItemType<UnfellerOfEvergreens>(), ItemType<ToucaneItem>(), ItemType<CreepingVine>() }, 1, 0.4f), ivyChests, 1);

			List<ChestInfo> PagodaPool = new List<ChestInfo> {
				new ChestInfo(ItemType<JadeStaff>()),
				new ChestInfo(new int[]{ ItemType<DynastyFan>(), ItemType<Items.Weapon.Swung.AnimeSword.AnimeSword>() }),
				new ChestInfo(ItemType<FestivalLanternItem>()),
				new ChestInfo(commonItems1, WorldGen.genRand.Next(3, 10)),
				new ChestInfo(ammo1, WorldGen.genRand.Next(20, 50)),
				new ChestInfo(potions, WorldGen.genRand.Next(2, 3)),
				new ChestInfo(ItemID.RecallPotion, WorldGen.genRand.Next(2, 3)),
				new ChestInfo(other1, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(other2, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(moddedMaterials, WorldGen.genRand.Next(2, 6)),
				new ChestInfo(ItemID.SilverCoin, WorldGen.genRand.Next(12, 30))
			};
			AddToVanillaChest(PagodaPool, dynastyChests);

			List<ChestInfo> AsteroidPool = new List<ChestInfo> {
				new ChestInfo(new int[]{ ItemType<ZiplineGun>(), ItemType<HighGravityBoots>(), ItemType<MagnetHook>() }),
				new ChestInfo(new int[]{ItemType<JumpPadItem>(), ItemID.SuspiciousLookingEye }, 1, 0.5f),
				new ChestInfo(ItemType<TargetCan>(), WorldGen.genRand.Next(10, 15), 0.5f),
				new ChestInfo(ItemType<SpaceJunkItem>(), WorldGen.genRand.Next(30, 55), 0.5f),
				new ChestInfo(commonItems1, WorldGen.genRand.Next(3, 10)),
				new ChestInfo(ammo1, WorldGen.genRand.Next(20, 50)),
				new ChestInfo(potions, WorldGen.genRand.Next(2, 4)),
				new ChestInfo(ItemID.RecallPotion, WorldGen.genRand.Next(1, 3)),
				new ChestInfo(other1, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(other2, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(moddedMaterials, WorldGen.genRand.Next(2, 6)),
				new ChestInfo(ItemID.SilverCoin, WorldGen.genRand.Next(12, 30))
			};
			AddToModdedChestWithOverlapCheck(AsteroidPool, TileType<AsteroidChest>());

			List<ChestInfo> sepulchreLootPool = new List<ChestInfo>
			{
				new ChestInfo(new int[] { ItemType<ToxicBottle>(), ItemType<AccursedBlade>(), ItemType<OldCross>() }),
				new ChestInfo(ItemID.SuspiciousLookingEye, 1, 0.5f),
				new ChestInfo(ItemType<SepulchreArrow>(), WorldGen.genRand.Next(20, 50), 0.5f),
				new ChestInfo(ItemID.Book, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(new int[]{ItemID.SilverBar, ItemID.GoldBar, ItemID.TungstenBar, ItemID.PlatinumBar }, WorldGen.genRand.Next(5, 12), 0.5f),
				new ChestInfo(potions, WorldGen.genRand.Next(2, 4), 0.66f),
				new ChestInfo((WorldGen.crimson) ? potionscorrupt : potionscrim, WorldGen.genRand.Next(2, 4), 0.66f),
				new ChestInfo(moddedMaterials, WorldGen.genRand.Next(2, 6), 0.5f),
				new ChestInfo(ItemID.CursedTorch, WorldGen.genRand.Next(15, 31), 0.75f),
				new ChestInfo(ItemID.GoldCoin, WorldGen.genRand.Next(1, 3)),
				new ChestInfo(ItemID.SilverCoin, WorldGen.genRand.Next(100)),
			};
			AddToModdedChest(sepulchreLootPool, TileType<SepulchreChestTile>());

			List<ChestInfo> scarabChestPool = new List<ChestInfo> {
				new ChestInfo(ItemType<CleftHorn>()),
				new ChestInfo(commonItems1, WorldGen.genRand.Next(3, 10)),
				new ChestInfo(ammo1, WorldGen.genRand.Next(20, 50)),
				new ChestInfo(potions, WorldGen.genRand.Next(2, 4)),
				new ChestInfo(ItemID.RecallPotion, WorldGen.genRand.Next(1, 3)),
				new ChestInfo(other1, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(other2, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(moddedMaterials, WorldGen.genRand.Next(2, 6)),
				new ChestInfo(ItemID.SilverCoin, WorldGen.genRand.Next(12, 30))
			};
			AddToModdedChest(scarabChestPool, TileType<GoldScarabChest>());

			List<ChestInfo> goblinPool = new List<ChestInfo> {
				new ChestInfo(new int[] { ItemType<Glyph>(), ItemID.MagicMirror, ItemID.WandofSparking }),
				new ChestInfo(commonItems1, WorldGen.genRand.Next(3, 10)),
				new ChestInfo(ammo1, WorldGen.genRand.Next(20, 50)),
				new ChestInfo(potions, WorldGen.genRand.Next(2, 4)),
				new ChestInfo(ItemID.RecallPotion, WorldGen.genRand.Next(1, 3)),
				new ChestInfo(other1, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(other2, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(moddedMaterials, WorldGen.genRand.Next(2, 6)),
				new ChestInfo(ItemID.SilverCoin, WorldGen.genRand.Next(12, 30))
			};
			AddToModdedChest(goblinPool, TileType<GoblinChest>());

			List<ChestInfo> briarPool = new List<ChestInfo> {
				new ChestInfo(new int[] { ItemType<ReachChestMagic>(), ItemType<TwigStaff>(), ItemType<ThornHook>(), ItemType<ReachBoomerang>(), ItemType<ReachBrooch>() }),
				new ChestInfo(new int[]{ ItemType<Book_Briar>(), ItemType<Book_BriarArt>(), ItemType<GladeWreath>(), ItemType<LivingElderbarkWand>() }, 1, 0.25f),
				new ChestInfo(commonItems1, WorldGen.genRand.Next(3, 10)),
				new ChestInfo(ammo1, WorldGen.genRand.Next(20, 50)),
				new ChestInfo(potions, WorldGen.genRand.Next(2, 4)),
				new ChestInfo(ItemID.RecallPotion, WorldGen.genRand.Next(1, 3)),
				new ChestInfo(other1, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(other2, WorldGen.genRand.Next(1, 4)),
				new ChestInfo(moddedMaterials, WorldGen.genRand.Next(2, 6)),
				new ChestInfo(ItemID.SilverCoin, WorldGen.genRand.Next(12, 30)),
			};
			AddToModdedChest(briarPool, TileType<ReachChest>());
		}

		public override void PostUpdate()
		{
			Player player = Main.LocalPlayer;
			MyPlayer modPlayer = player.GetSpiritPlayer();

			if (modPlayer.ZoneSpirit)
			{
				if (!aurora)
					aurora = true;
				auroraType = 10;
			}

			if (Main.bloodMoon)
				auroraType = 6;

			if (Main.pumpkinMoon)
				auroraType = 7;

			if (Main.snowMoon)
				auroraType = 8;

			if (BlueMoon)
				auroraType = 9;

			if (!Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon && !modPlayer.ZoneSpirit)
				auroraType = auroraTypeFixed;

			if (Main.dayTime != dayTimeLast)
				dayTimeSwitched = true;
			else
				dayTimeSwitched = false;

			dayTimeLast = Main.dayTime;

			if (BlueMoon && dayTimeSwitched && !downedBlueMoon)
				downedBlueMoon = true;

			if (jellySky && dayTimeSwitched && !downedJellyDeluge)
				downedJellyDeluge = true;

			if (dayTimeSwitched)
			{
				if (Main.rand.Next(2) == 0 && !spaceJunkWeather)
					stardustWeather = true;
				else
					stardustWeather = false;

				if (Main.rand.Next(2) == 0 && !stardustWeather)
					spaceJunkWeather = true;
				else
					spaceJunkWeather = false;

				if (Main.rand.Next(4) == 0)
					meteorShowerWeather = true;
				else
					meteorShowerWeather = false;

				if (!Main.dayTime && Main.hardMode)
				{
					if (!Main.fastForwardTime && !Main.bloodMoon && WorldGen.spawnHardBoss == 0 && ((Main.rand.Next(20) == 1 && !downedBlueMoon) || (Main.rand.Next(40) == 1 && !downedBlueMoon)))
					{
						Main.NewText("A Mystic Moon is rising...", 61, 255, 142);
						BlueMoon = true;
						downedBlueMoon = true;
					}
				}
				else
					BlueMoon = false;

				if (!Main.dayTime && Main.rand.Next(6) == 0)
				{
					auroraTypeFixed = Main.rand.Next(new int[] { 1, 2, 3, 5 });
					aurora = true;
				}
				else
					aurora = false;

				if (!Main.dayTime && Main.rand.Next(32) == 0)
					rareStarfallEvent = true;
				else
					rareStarfallEvent = false;

				if (!Main.dayTime && Main.rand.Next(6) == 0)
				{
					luminousType = Main.rand.Next(new int[] { 1, 2, 3 });
					luminousOcean = true;
				}
				else
					luminousOcean = false;

				if (!Main.dayTime && (Main.moonPhase == 2 || Main.moonPhase == 6) && !Main.bloodMoon && Main.rand.Next(2) == 0)
					calmNight = true;
				else
					calmNight = false;

				if (Main.rand.NextBool(8))
					ashRain = true;
				else
					ashRain = false;

				if (!Main.dayTime && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || downedScarabeus || downedReachBoss || downedRaider || downedAncientFlier) && (!downedMoonWizard && Main.rand.Next(4) == 0 || downedMoonWizard && Main.rand.Next(36) == 0))
				{
					Main.NewText("Strange jellyfish are raining from the skies!", 61, 255, 142);
					jellySky = true;
				}
				else
					jellySky = false;
			}

			//pagoda enemy spawning
			if (!spawnedPagodaEnemies && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Rectangle pagodaSpawnArea = new Rectangle(pagodaX - 65, pagodaY - 37, 256, 140);
				bool shouldSpawn = false;
				if (Main.netMode == NetmodeID.SinglePlayer && pagodaSpawnArea.Contains(player.Center.ToTileCoordinates())) shouldSpawn = true;
				else if (Main.netMode == NetmodeID.Server)
				{
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						if (Main.player[i].active && pagodaSpawnArea.Contains(Main.player[i].Center.ToTileCoordinates()))
						{
							shouldSpawn = true;
							break;
						}
					}
				}
				if (shouldSpawn)
				{
					spawnedPagodaEnemies = true;
					for (int i = 0; i < Main.rand.Next(8, 10); i++)
						NPC.NewNPC((pagodaX + Main.rand.Next(0, 126)) * 16, (pagodaY + Main.rand.Next(-10, 50)) * 16, NPCType<NPCs.Yurei.PagodaGhostPassive>());
					for (int i = 0; i < 3; i++)
						NPC.NewNPC((pagodaX + Main.rand.Next(0, 126)) * 16, (pagodaY + Main.rand.Next(-10, 50)) * 16, NPCType<NPCs.SamuraiGhost.SamuraiPassive>());
				}
			}
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				if (!essenceMessage)
				{
					Main.NewText("The Essences are bursting!", 61, 255, 142);
					essenceMessage = true;
				}
			}

			if (Main.hardMode && !rockCandy)
			{
				rockCandy = true;
				for (int C = 0; C < Main.maxTilesX * 9; C++)
				{
					int X = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
					int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if (Main.tile[X, Y].type == TileID.Stone)
					{
						WorldGen.PlaceObject(X, Y, TileType<GreenShardBig>());
						NetMessage.SendObjectPlacment(-1, X, Y, TileType<GreenShardBig>(), 0, 0, -1, -1);
					}
				}
				for (int C = 0; C < Main.maxTilesX * 9; C++)
				{
					int X = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
					int Y = WorldGen.genRand.Next((int)WorldGen.rockLayer, Main.maxTilesY);
					if (Main.tile[X, Y].type == TileID.Stone)
					{
						WorldGen.PlaceObject(X, Y, TileType<PurpleShardBig>());
						NetMessage.SendObjectPlacment(-1, X, Y, TileType<PurpleShardBig>(), 0, 0, -1, -1);
					}
				}
			}
		}
	}
}