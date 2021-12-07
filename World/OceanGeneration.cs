using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

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
			int num462 = 0;
			int num463 = 0;
			int num464 = 20;
			int num465 = Main.maxTilesX - 20;
			progress.Message = Lang.gen[22].Value; //replace later

			int dungeonSide = Main.dungeonX < Main.maxTilesX / 2 ? -1 : 1;

			void GenSingleOceanSingleStep(int initialWidth, int oceanTop, int worldEdge, int placeX, ref int tilesFromInnerEdge)
			{
				tilesFromInnerEdge++;

				float depth = GetOceanSlope(tilesFromInnerEdge);
				depth += OceanSlopeRoughness();

				if (tilesFromInnerEdge == 235)
					num464 = placeX;

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

					num462 = oceanTop;
					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = initialWidth - 1; placeX >= worldEdge; placeX--)
						GenSingleOceanSingleStep(initialWidth, oceanTop, worldEdge, placeX, ref tilesFromInnerEdge);
				}
				else
				{
					if (dungeonSide == -1)
						worldEdge = Main.maxTilesX - 275;

					int oceanTop;
					for (oceanTop = 0; !Main.tile[worldEdge - 1, oceanTop].active(); oceanTop++)
					{ } //Get top of ocean

					num463 = oceanTop;
					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = worldEdge; placeX < initialWidth; placeX++) //repeat X loop
						GenSingleOceanSingleStep(initialWidth, oceanTop, worldEdge, placeX, ref tilesFromInnerEdge);
				}
			}

			//Not sure what these two do but I'll keep em for 0 reason
			for (; !Main.tile[num464, num462].active(); num462++)
			{ }
			for (; !Main.tile[num465, num463].active(); num463++)
			{ }
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
				Main.tile[placeX, placeY].type = 53;
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
			Piecewise_V, //My heavily modified piecewise with variable
		}
	}
}