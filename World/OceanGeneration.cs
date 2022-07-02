using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SpiritMod.Tiles.Ambient.Corals;
using SpiritMod.Tiles.Ambient.Kelp;
using SpiritMod.Tiles.Ambient.Ocean;
using System.Collections.Generic;
using Terraria.Utilities;
using SpiritMod.Items.Sets.PirateStuff.DuelistLegacy;
using SpiritMod.Items.Sets.GunsMisc.LadyLuck;
using SpiritMod.Items.Sets.FloatingItems;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.IO;

namespace SpiritMod.World
{
	public class OceanGeneration
	{
		private static float PiecewiseVScale = 1f;
		private static float PiecewiseVMountFactor = 1f;

		private static int _roughTimer = 0;
		private static float _rough = 0f;
		private static (Rectangle, Rectangle) _oceanInfos = (new Rectangle(), new Rectangle());

		/// <summary>Generates the Ocean ("Beaches"). Heavily based on vanilla code.</summary>
		/// <param name="progress"></param>
		public static void GenerateOcean(GenerationProgress progress, GameConfiguration config)
		{
			//Basic Shape
			progress.Message = Language.GetText("LegacyWorldGen.22").Value;

			int dungeonSide = Main.dungeonX < Main.maxTilesX / 2 ? -1 : 1;

			void GenSingleOceanSingleStep(int oceanTop, int placeX, ref int tilesFromInnerEdge)
			{
				tilesFromInnerEdge++;

				float depth = GetOceanSlope(tilesFromInnerEdge);
				depth += OceanSlopeRoughness();

				int thickness = WorldGen.genRand.Next(20, 28); //Sand lining is a bit thicker than vanilla
				bool passedTile = false;

				for (int placeY = 0; placeY < oceanTop + depth + thickness; placeY++)
				{
					bool liq = PlaceTileOrLiquid(placeX, placeY, oceanTop, depth);

					if (!passedTile && !Framing.GetTileSafely(placeX, placeY + 1).HasTile)
					{
						if (!liq)
							thickness++;
					}
					else
						passedTile = true;
				}
			}

			void CheckOceanHeight(ref int height)
			{
				float depth = GetOceanSlope(250);

				do
				{
					height--;
				} while (height + depth + 20 > Main.worldSurface + 50);
			}

			for (int side = 0; side < 2; side++)
			{
				PiecewiseVScale = 1f + (WorldGen.genRand.Next(-1000, 2500) * 0.0001f);
				PiecewiseVMountFactor = WorldGen.genRand.Next(150, 750);

				int worldEdge = side == 0 ? 0 : Main.maxTilesX - WorldGen.genRand.Next(125, 200) - 50;
				int initialWidth = side == 0 ? WorldGen.genRand.Next(125, 200) + 50 : Main.maxTilesX; //num468
				int tilesFromInnerEdge = 0;

				if (side == 0)
				{
					if (dungeonSide == 1)
						initialWidth = 275;

					int oceanTop;
					for (oceanTop = 0; !Main.tile[initialWidth - 1, oceanTop].HasTile; oceanTop++)
					{ } //Get top of ocean

					CheckOceanHeight(ref oceanTop);

					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = initialWidth - 1; placeX >= worldEdge; placeX--)
						GenSingleOceanSingleStep(oceanTop, placeX, ref tilesFromInnerEdge);

					_oceanInfos.Item1 = new Rectangle(worldEdge, oceanTop - 5, initialWidth, (int)GetOceanSlope(tilesFromInnerEdge) + 20);
				}
				else
				{
					if (dungeonSide == -1)
						worldEdge = Main.maxTilesX - 275;

					int oceanTop;
					for (oceanTop = 0; !Main.tile[worldEdge - 1, oceanTop].HasTile; oceanTop++)
					{ } //Get top of ocean

					CheckOceanHeight(ref oceanTop);

					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = worldEdge; placeX < initialWidth; placeX++) //repeat X loop
						GenSingleOceanSingleStep(oceanTop, placeX, ref tilesFromInnerEdge);

					_oceanInfos.Item2 = new Rectangle(worldEdge, oceanTop - 5, initialWidth - worldEdge, (int)GetOceanSlope(tilesFromInnerEdge) + 20);
				}
			}

			PopulateOcean(_oceanInfos.Item1, 0);
			PopulateOcean(_oceanInfos.Item2, 1);
		}

		private static void PopulateOcean(Rectangle bounds, int side)
		{
			bool ValidGround(int i, int j, int width, int type = TileID.Sand)
			{
				for (int k = i; k < i + width; ++k)
				{
					Tile t = Framing.GetTileSafely(k, j);
					if (!t.HasTile || t.TileType != type || t.TopSlope || !Main.tileSolid[t.TileType])
						return false;
				}
				return true;
			}

			bool OpenArea(int i, int j, int width, int height)
			{
				for (int k = i; k < i + width; ++k)
				{
					for (int l = j; l < j + height; ++l)
					{
						Tile t = Framing.GetTileSafely(k, l);
						if (t.HasTile || t.LiquidAmount < 200)
							return false;
					}
				}
				return true;
			}

			PlacePirateChest(side == 0 ? bounds.Right : bounds.Left, side);
			PlaceSunkenTreasure(side == 0 ? bounds.Right : bounds.Left, side);

			for (int i = bounds.Left; i < bounds.Right; ++i)
			{
				for (int j = bounds.Top; j < bounds.Bottom; ++j)
				{
					int tilesFromInnerEdge = bounds.Right - i;
					if (side == 1)
						tilesFromInnerEdge = i - bounds.Left;

					int coralChance = 0;
					if (tilesFromInnerEdge < 133) //First slope (I hope)
						coralChance = 3;
					else if (tilesFromInnerEdge < 161)
						coralChance = 12;

					//Coral multitiles
					if (coralChance > 0 && WorldGen.genRand.NextBool(coralChance) && ValidGround(i, j, 2, TileID.Sand) && OpenArea(i, j - 3, 2, 3))
					{
						int type = ModContent.TileType<Coral2x2>();

						WorldGen.PlaceObject(i, j - 2, type, true, WorldGen.genRand.Next(3));
						NetMessage.SendObjectPlacment(-1, i, j, type, 0, 0, -1, -1);
						continue;
					}

					int kelpChance = tilesFromInnerEdge < 100 ? 10 : 5; //Higher on first slope, then less common

					//Kelp multitile
					if (kelpChance > 0 && WorldGen.genRand.NextBool(kelpChance * 2) && ValidGround(i, j, 2, TileID.Sand) && OpenArea(i, j - 3, 2, 3))
					{
						int type = ModContent.TileType<Kelp2x3>();

						int choice = WorldGen.genRand.Next(2);
						if (choice == 0)
							type = ModContent.TileType<Kelp2x2>();

						int offset = type == ModContent.TileType<Kelp2x3>() ? 3 : 2;

						WorldGen.PlaceObject(i, j - offset, type, true, 0);
						NetMessage.SendObjectPlacment(-1, i, j, type, 0, 0, -1, -1);
						continue;
					}

					//Kelp multitile (small)
					if (kelpChance > 0 && WorldGen.genRand.NextBool(kelpChance * 2) && ValidGround(i, j, 2, TileID.Sand) && OpenArea(i, j - 2, 1, 2))
					{
						int type = ModContent.TileType<Kelp1x2>();

						WorldGen.PlaceObject(i, j - 2, type, true, 0);
						WorldGen.PlaceTile(i, j, TileID.Sand);
						NetMessage.SendObjectPlacment(-1, i, j, type, 0, 0, -1, -1);
						continue;
					}

					//Hydrothermal vents
					if (WorldGen.genRand.Next(7) < 3 && tilesFromInnerEdge > 135 && ValidGround(i, j, 1, TileID.Sand) && OpenArea(i, j - 3, 1, 3))
					{
						int type = WorldGen.genRand.NextBool(3) ? ModContent.TileType<HydrothermalVent1x3>() : ModContent.TileType<HydrothermalVent1x2>();
						int offset = type == ModContent.TileType<HydrothermalVent1x2>() ? 1 : 2;

						WorldGen.PlaceObject(i, j - offset, type, true, WorldGen.genRand.Next(2));
						NetMessage.SendObjectPlacment(-1, i, j, type, 0, 0, -1, -1);
						continue;
					}

					//Growing kelp
					if (WorldGen.genRand.Next(5) < 2 && tilesFromInnerEdge < 133 && ValidGround(i, j, 1, TileID.Sand))
					{
						int height = WorldGen.genRand.Next(6, 23) + 2;
						int offset = 1;
						while (!Framing.GetTileSafely(i, j - offset).HasTile && Framing.GetTileSafely(i, j - offset).LiquidAmount > 155 && height > 0)
						{
							WorldGen.PlaceTile(i, j - offset++, ModContent.TileType<OceanKelp>());
							height--;
						}
					}
				}
			}
		}

		private static void PlaceSunkenTreasure(int innerEdge, int side)
		{
			for (int i = 0; i < 2; ++i)
			{
				int sunkenX = innerEdge - WorldGen.genRand.Next(133, innerEdge - 40);
				if (side == 1)
					sunkenX = innerEdge + WorldGen.genRand.Next(133, Main.maxTilesX - innerEdge - 40);

				var pos = new Point(sunkenX, (int)(Main.maxTilesY * 0.35f / 16f));
				while (!WorldGen.SolidTile(pos.X, pos.Y))
					pos.Y++;

				for (int j = pos.X; j < pos.X + 3; ++j)
					for (int k = pos.Y - 1; k < pos.Y; ++k)
						WorldGen.KillTile(j, k, false, false, true);

				WorldGen.PlaceObject(pos.X, pos.Y - 1, ModContent.TileType<SunkenTreasureTile>());
			}
		}

		public static void PlacePirateChest(int innerEdge, int side)
		{
		retry:
			int guaranteeChestX = innerEdge - WorldGen.genRand.Next(100, innerEdge - 60);
			if (side == 1)
				guaranteeChestX = innerEdge + WorldGen.genRand.Next(100, Main.maxTilesX - innerEdge - 60);

			var chest = new Point(guaranteeChestX, (int)(Main.maxTilesY * 0.35f / 16f));
			while (!WorldGen.SolidTile(chest.X, chest.Y))
				chest.Y++;

			if (!WorldMethods.AreaClear(chest.X, chest.Y - 2, 2, 2))
				goto retry; //uh oh! goto! I'm a lazy programmer seethe & rage

			for (int i = 0; i < 2; ++i)
			{
				WorldGen.KillTile(chest.X + i, chest.Y, false, false, true);
				WorldGen.PlaceTile(chest.X + i, chest.Y , TileID.HardenedSand, true, false);
				Framing.GetTileSafely(chest.X + i, chest.Y).Slope = 0;
			}

			int BarStack() => WorldGen.genRand.Next(3, 7);

			PlaceChest(chest.X, chest.Y - 1, ModContent.TileType<OceanPirateChest>(), 
				new (int, int)[] //Primary items
				{
					(side == 0 ? ModContent.ItemType<LadyLuck>() : ModContent.ItemType<DuelistLegacy>(), 1)
				}, 
				new (int, int)[] //Sub items (woo)
				{   
					(ItemID.GoldCoin, WorldGen.genRand.Next(12, 30)), (ItemID.Diamond, WorldGen.genRand.Next(12, 30)), (ItemID.GoldCrown, 1), (ItemID.GoldDust, WorldGen.genRand.Next()),
					(ItemID.GoldChest, 1), (ItemID.GoldenChair, 1), (ItemID.GoldChandelier, 1), (ItemID.GoldenPlatform, WorldGen.genRand.Next(12, 18)), (ItemID.GoldenSink, 1), (ItemID.GoldenSofa, 1),
					(ItemID.GoldenTable, 1), (ItemID.GoldenToilet, 1), (ItemID.GoldenWorkbench, 1), (ItemID.GoldenPiano, 1), (ItemID.GoldenLantern, 1), (ItemID.GoldenLamp, 1), (ItemID.GoldenDresser, 1),
					(ItemID.GoldenDoor, 1), (ItemID.GoldenCrate, 1), (ItemID.GoldenClock, 1), (ItemID.GoldenChest, 1), (ItemID.GoldenCandle, WorldGen.genRand.Next(2, 4)), (ItemID.GoldenBookcase, 1),
					(ItemID.GoldenBed, 1), (ItemID.GoldenBathtub, 1), (ItemID.MythrilBar, BarStack()), (ItemID.AdamantiteBar, BarStack()), (ItemID.CobaltBar, BarStack()),
					(ItemID.TitaniumBar, BarStack()), (ItemID.PalladiumBar, BarStack()), (ItemID.OrichalcumBar, BarStack())
				},
				true, WorldGen.genRand, WorldGen.genRand.Next(15, 21), 1, true, 2, 2);
		}

		public static bool PlaceChest(int x, int y, int type, (int, int)[] mainItems, (int, int)[] subItems, bool noTypeRepeat = true, UnifiedRandom r = null, int subItemLength = 6, int style = 0, bool overRide = false, int width = 2, int height = 2)
		{
			r = r ?? Main.rand;

			if (overRide)
				for (int i = x; i < x + width; ++i)
					for (int j = y; j < y + height; ++j)
						WorldGen.KillTile(i, j - 1, false, false, true);

			int ChestIndex = WorldGen.PlaceChest(x, y, (ushort)type, false, style);
			if (ChestIndex != -1)
			{
				int main = r.Next(mainItems.Length);
				Main.chest[ChestIndex].item[0].SetDefaults(mainItems[main].Item1);
				Main.chest[ChestIndex].item[0].stack = mainItems[main].Item2;

				int reps = 0;
				var usedTypes = new List<int>();

				for (int i = 0; i < subItemLength; ++i)
				{
				repeat:
					if (reps > 50)
					{
						SpiritMod.Instance.Logger.Info("WARNING: Attempted to repeat item placement too often. Report to dev. [SPIRITMOD]");
						break;
					}

					int sub = r.Next(subItems.Length);
					int itemType = subItems[sub].Item1;
					int itemStack = subItems[sub].Item2;

					if (noTypeRepeat && usedTypes.Contains(itemType))
						goto repeat;

					usedTypes.Add(itemType);

					Main.chest[ChestIndex].item[i + 1].SetDefaults(itemType);
					Main.chest[ChestIndex].item[i + 1].stack = itemStack;
				}
				return true;
			}
			return false;
		}

		private static float OceanSlopeRoughness()
		{
			_roughTimer--;
			if (_roughTimer <= 0)
			{
				_roughTimer = WorldGen.genRand.Next(5, 9);
				_rough += WorldGen.genRand.NextFloat(0.6f, 1f) * (WorldGen.genRand.NextBool(2) ? -1 : 1);
			}
			return _rough;
		}

		private static bool PlaceTileOrLiquid(int placeX, int placeY, int oceanTop, float depth)
		{
			if (placeY < oceanTop + depth - 3f)
			{
				Main.tile[placeX, placeY].HasTile = false;

				if (placeY > oceanTop + 5)
					Main.tile[placeX, placeY].LiquidAmount = byte.MaxValue;
				else if (placeY == oceanTop + 5)
					Main.tile[placeX, placeY].LiquidAmount = 127;

				Main.tile[placeX, placeY].WallType = 0;
				return true;
			}
			else if (placeY > oceanTop)
			{
				if (placeY < oceanTop + depth + 8)
					Main.tile[placeX, placeY].TileType = TileID.Sand;
				else
					Main.tile[placeX, placeY].TileType = TileID.HardenedSand;
				Main.tile[placeX, placeY].HasTile = true;
			}

			Main.tile[placeX, placeY].WallType = 0;
			return false;
		}

		/// <summary>Gets the slope of the ocean. Reference: <seealso cref="https://www.desmos.com/calculator/xfnsmar79x"/></summary>
		/// <param name="tilesFromInnerEdge"></param>
		/// <returns></returns>
		private static float GetOceanSlope(int tilesFromInnerEdge)
		{
			OceanShape shape = ModContent.GetInstance<SpiritClientConfig>().OceanShape;
			if (shape == OceanShape.SlantedSine)
			{
				const int SlopeSize = 15;
				const float Steepness = 0.8f;

				//(s_0s_1)sin(1/s_0 x) + (s_1)x
				if (tilesFromInnerEdge > 234)
					return (SlopeSize * Steepness * (float)Math.Sin((1f / SlopeSize) * 234)) + (Steepness * 234);
				return (SlopeSize * Steepness * (float)Math.Sin((1f / SlopeSize) * tilesFromInnerEdge)) + (Steepness * tilesFromInnerEdge);
			}
			else if (shape == OceanShape.Piecewise)
			{
				if (tilesFromInnerEdge < 75)
					return (1 / 75f) * tilesFromInnerEdge * tilesFromInnerEdge;
				else if (tilesFromInnerEdge < 125)
					return 75;
				else if (tilesFromInnerEdge < 175)
					return (1 / 50f) * (float)Math.Pow(tilesFromInnerEdge - 125, 2) + 75;
				else
					return 125;
			}
			else if (shape == OceanShape.Piecewise_M)
			{
				const float CubicMultiplier = 37.5f;
				const float CubicMultiplierSq = CubicMultiplier * CubicMultiplier;

				if (tilesFromInnerEdge < 75)
					return (1 / CubicMultiplierSq) * (float)Math.Pow(tilesFromInnerEdge - CubicMultiplier, 3) + CubicMultiplier;
				else if (tilesFromInnerEdge < 125)
					return 75;
				else if (tilesFromInnerEdge < 175)
					return (1 / 50f) * (float)Math.Pow(tilesFromInnerEdge - 125, 2) + 75;
				else
					return 125;
			}
			else //if (ModContent.GetInstance<SpiritClientConfig>().OceanShape == OceanShape.Piecewise)
			{
				float Scale = PiecewiseVScale; //m_s
				const float Steepness = 25f; //m_c

				float FirstSlope(float x) => -Scale * (1 / (Steepness * Steepness)) * (float)Math.Pow(0.6f * x - Steepness, 3) - (Scale * Steepness);
				float SecondSlope(float x) => -Scale * (1 / (2 * (Steepness * Steepness))) * (float)Math.Pow(x - 75 - Steepness, 3) + ((float)Math.Pow(x - 80, 2) / PiecewiseVMountFactor) + FirstSlope(83.33f);
				float LastSlope(int x) => Scale * (1 / Steepness) * (float)Math.Pow(x - 160, 2) + SecondSlope(141.7f);

				float returnValue;
				if (tilesFromInnerEdge < 75)
					returnValue = FirstSlope(tilesFromInnerEdge);
				else if (tilesFromInnerEdge < 133)
					returnValue = SecondSlope(tilesFromInnerEdge);
				else if (tilesFromInnerEdge < 161)
					returnValue = LastSlope(tilesFromInnerEdge);
				else
					returnValue = LastSlope(160);

				return -returnValue;
			}
		}

		public enum OceanShape
		{
			Default = 0, //vanilla worldgen
			SlantedSine, //Yuyu's initial sketch
			Piecewise, //Musicano's original sketch
			Piecewise_M, //Musicano's sketch with Sal/Yuyu's cubic modification
			Piecewise_V, //My heavily modified piecewise with variable height
		}
	}
}