using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;

namespace SpiritMod.World
{
	public class OceanGeneration
	{
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

			for (int side = 0; side < 2; side++)
			{
				int worldEdge = side == 0 ? 0 : Main.maxTilesX - WorldGen.genRand.Next(125, 200) - 50;
				int initialWidth = side == 0 ? WorldGen.genRand.Next(125, 200) + 50 : Main.maxTilesX; //num468
				float depth;
				int tilesFromInnerEdge = 0;

				if (side == 0)
				{
					if (dungeonSide == 1)
						initialWidth = 275;

					int oceanTop;
					for (oceanTop = 0; !Main.tile[initialWidth - 1, oceanTop].active(); oceanTop++)
					{
					}
					num462 = oceanTop;
					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = initialWidth - 1; placeX >= worldEdge; placeX--)
					{
						tilesFromInnerEdge++;

						depth = GetOceanSlope(tilesFromInnerEdge);

						if (tilesFromInnerEdge == 235)
							num465 = placeX;
						if (tilesFromInnerEdge == 235)
							num464 = placeX;

						int num473 = WorldGen.genRand.Next(20, 28); //Sand lining is a bit thicker than vanilla
						for (int placeY = 0; placeY < oceanTop + depth + num473; placeY++)
							PlaceTileOrLiquid(placeX, placeY, oceanTop, depth);
					}
				}
				else
				{
					if (dungeonSide == -1)
						worldEdge = Main.maxTilesX - 275;

					int oceanTop;
					for (oceanTop = 0; !Main.tile[worldEdge, oceanTop].active(); oceanTop++)
					{
					}
					num463 = oceanTop;
					oceanTop += WorldGen.genRand.Next(1, 5);
					for (int placeX = worldEdge; placeX < initialWidth; placeX++) //repeat X loop
					{
						tilesFromInnerEdge++;

						depth = GetOceanSlope(tilesFromInnerEdge);

						if (tilesFromInnerEdge == 235)
							num465 = placeX;

						int num479 = WorldGen.genRand.Next(20, 28); //Sand lining is a bit thicker than vanilla
						for (int placeY = 0; placeY < oceanTop + depth + num479; placeY++)
							PlaceTileOrLiquid(placeX, placeY, oceanTop, depth);
					}
				}
			}

			//Not sure what these two do but I'll keep em for 0 reason
			for (; !Main.tile[num464, num462].active(); num462++)
			{ }
			for (; !Main.tile[num465, num463].active(); num463++)
			{ }
		}

		private static void PlaceTileOrLiquid(int placeX, int placeY, int oceanTop, float depth)
		{
			if (placeY < oceanTop + depth - 3f)
			{
				Main.tile[placeX, placeY].active(active: false);

				if (placeY > oceanTop)
					Main.tile[placeX, placeY].liquid = byte.MaxValue;
				else if (placeY == oceanTop)
					Main.tile[placeX, placeY].liquid = 127;
			}
			else if (placeY > oceanTop)
			{
				Main.tile[placeX, placeY].type = 53;
				Main.tile[placeX, placeY].active(active: true);
			}
			Main.tile[placeX, placeY].wall = 0;
		}

		private static float GetOceanSlope(int tilesFromInnerEdge)
		{
			const int SlopeSize = 15;

			//s_0sin(1/s_0 x) + x
			return (SlopeSize * (float)Math.Sin((1f / SlopeSize) * tilesFromInnerEdge)) + tilesFromInnerEdge;
		}
	}
}