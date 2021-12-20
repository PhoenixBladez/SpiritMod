using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SpiritMod.Tiles.Ambient.Corals;
using SpiritMod.Tiles.Ambient.Kelp;
using SpiritMod.Tiles.Ambient.Ocean;
using System.Collections.Generic;
using Terraria.Utilities;
using SpiritMod.Items.Sets.PirateStuff.DuelistLegacy;
using SpiritMod.Items.Sets.GunsMisc.LadyLuck;

namespace SpiritMod.World
{
	public class OceanGeneration
	{
		private static float PiecewiseVScale = 1f;
		private static float PiecewiseVMountFactor = 1f;

		private static int _roughTimer = 0;
		private static float _rough = 0f;

		/// <summary>Generates the Ocean ("Beaches"). Heavily based on vanilla code.</summary>
		/// <param name="progress"></param>
		public static void GenerateOcean(GenerationProgress progress)
		{
			//Basic Shape
			progress.Message = Lang.gen[22].Value; //replace later

			int dungeonSide = Main.dungeonX < Main.maxTilesX / 2 ? -1 : 1;
			(Rectangle, Rectangle) oceanInfos = (new Rectangle(), new Rectangle());

			void GenSingleOceanSingleStep(int oceanTop, int placeX, ref int tilesFromInnerEdge)
			{
				tilesFromInnerEdge++;

				float depth = GetOceanSlope(tilesFromInnerEdge);
				depth += OceanSlopeRoughness();

				int num473 = WorldGen.genRand.Next(20, 28); //Sand lining is a bit thicker than vanilla
				for (int placeY = 0; placeY < oceanTop + depth + num473; placeY++)
					PlaceTileOrLiquid(placeX, placeY, oceanTop, depth);
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
					for (oceanTop = 0; !Main.tile[initialWidth - 1, oceanTop].active(); oceanTop++)
					{ } //Get top of ocean

					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = initialWidth - 1; placeX >= worldEdge; placeX--)
						GenSingleOceanSingleStep(oceanTop, placeX, ref tilesFromInnerEdge);

					oceanInfos.Item1.X = worldEdge;
					oceanInfos.Item1.Y = oceanTop - 5;
					oceanInfos.Item1.Width = initialWidth;
					oceanInfos.Item1.Height = (int)GetOceanSlope(tilesFromInnerEdge) + 20;
				}
				else
				{
					if (dungeonSide == -1)
						worldEdge = Main.maxTilesX - 275;

					int oceanTop;
					for (oceanTop = 0; !Main.tile[worldEdge - 1, oceanTop].active(); oceanTop++)
					{ } //Get top of ocean

					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = worldEdge; placeX < initialWidth; placeX++) //repeat X loop
						GenSingleOceanSingleStep(oceanTop, placeX, ref tilesFromInnerEdge);

					oceanInfos.Item2.X = worldEdge;
					oceanInfos.Item2.Y = oceanTop - 5;
					oceanInfos.Item2.Width = initialWidth - worldEdge;
					oceanInfos.Item2.Height = (int)GetOceanSlope(tilesFromInnerEdge) + 20;
				}
			}

			PopulateOcean(oceanInfos.Item1, 0);
			PopulateOcean(oceanInfos.Item2, 1);
		}

		private static void PopulateOcean(Rectangle bounds, int side)
		{
			bool ValidGround(int i, int j, int width, int type = TileID.Sand)
			{
				for (int k = i; k < i + width; ++k)
				{
					Tile t = Framing.GetTileSafely(k, j);
					if (!t.active() || t.type != type || t.topSlope())
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
						if (t.active() || t.liquid < 200)
							return false;
					}
				}
				return true;
			}

			PlacePirateChest(side == 0 ? bounds.Right : bounds.Left);

			for (int i = bounds.Left; i < bounds.Right; ++i)
			{
				for (int j = bounds.Top; j < bounds.Bottom; ++j)
				{
					int tilesFromInnerEdge = bounds.Right - i;
					if (side == 1)
						tilesFromInnerEdge = i - bounds.Left;

					if (WorldGen.genRand.NextBool(5) && ValidGround(i, j, 2, TileID.Sand) && OpenArea(i, j - 3, 2, 3))
					{
						int type = ModContent.TileType<Coral2x2>();

						if (tilesFromInnerEdge > 75)
						{
							int choice = WorldGen.genRand.Next(3);
							if (tilesFromInnerEdge > 161)
								choice = WorldGen.genRand.Next(2);

							if (choice == 0)
								type = ModContent.TileType<Kelp2x2>();
							else if (choice == 1)
								type = ModContent.TileType<Kelp2x3>();
						}

						int styleRange = type == ModContent.TileType<Coral2x2>() ? 3 : 1;
						int offset = type == ModContent.TileType<Kelp2x3>() ? 3 : 2;

						WorldGen.PlaceObject(i, j - offset, type, true, WorldGen.genRand.Next(styleRange));
						NetMessage.SendObjectPlacment(-1, i, j, type, 0, 0, -1, -1);
						continue;
					}

					if (WorldGen.genRand.Next(5) < 2 && tilesFromInnerEdge > 133 && ValidGround(i, j, 1, TileID.Sand) && OpenArea(i, j - 3, 1, 3))
					{
						int type = WorldGen.genRand.NextBool(3) ? ModContent.TileType<HydrothermalVent1x3>() : ModContent.TileType<HydrothermalVent1x2>();
						int offset = type == ModContent.TileType<HydrothermalVent1x2>() ? 2 : 3;

						WorldGen.PlaceObject(i, j - offset, type, true, WorldGen.genRand.Next(2));
						NetMessage.SendObjectPlacment(-1, i, j, type, 0, 0, -1, -1);
						continue;
					}

					if (WorldGen.genRand.Next(3) < 2 && tilesFromInnerEdge < 133 && ValidGround(i, j, 1, TileID.Sand))
					{
						int height = WorldGen.genRand.Next(4, 20);
						int offset = 1;
						while (OpenArea(i, j - offset, 1, 1) && height > 0)
						{
							WorldGen.PlaceTile(i, j - offset++, ModContent.TileType<OceanKelp>());
							height--;
						}
					}
				}
			}
		}

		private static void PlacePirateChest(int innerEdge)
		{
			int guaranteeChestX = innerEdge - WorldGen.genRand.Next(133, innerEdge - 40);
			var chest = new Point(guaranteeChestX, (int)(Main.maxTilesY * 0.35f / 16f));
			while (!WorldGen.SolidTile(chest.X, chest.Y - 1))
				chest.Y++;
			chest.Y--;

			int BarStack() => WorldGen.genRand.Next(3, 7);

			for (int i = 0; i < 3; ++i)
			{
				WorldGen.KillTile(chest.X + i, chest.Y + 1, false, false, true);
				WorldGen.PlaceTile(chest.X + i, chest.Y + 1, TileID.Sand, true, false);
				Framing.GetTileSafely(chest.X + i, chest.Y + 1).slope(0);
			}

			PlaceChest(chest.X, chest.Y - 1, ModContent.TileType<OceanPirateChest>(), 
				new (int, int)[] //Primary items
				{
					(ModContent.ItemType<DuelistLegacy>(), 1), (ModContent.ItemType<LadyLuck>(), 1)
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
				true, WorldGen.genRand, WorldGen.genRand.Next(15, 21), 0, true, 2, 2);
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

		private static void PlaceTileOrLiquid(int placeX, int placeY, int oceanTop, float depth)
		{
			if (placeY < oceanTop + depth - 3f)
			{
				Main.tile[placeX, placeY].active(active: false);

				if (placeY > oceanTop + 5)
					Main.tile[placeX, placeY].liquid = byte.MaxValue;
				else if (placeY == oceanTop + 5)
					Main.tile[placeX, placeY].liquid = 127;
			}
			else if (placeY > oceanTop)
			{
				Main.tile[placeX, placeY].type = TileID.Sand;
				Main.tile[placeX, placeY].active(active: true);
			}
			Main.tile[placeX, placeY].wall = 0;
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