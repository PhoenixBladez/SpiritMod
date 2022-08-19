using Terraria;
using Terraria.ID;
using System.Linq;
using System;
using Microsoft.Xna.Framework;

namespace SpiritMod.World.Micropasses
{
	public class WaterfallMicropass
	{
		static int[] ValidTypes = new int[] { TileID.Stone, TileID.ArgonMoss, TileID.BlueMoss, TileID.BrownMoss, TileID.GreenMoss, TileID.KryptonMoss, TileID.LavaMoss, 
			TileID.PurpleMoss, TileID.RedMoss, TileID.XenonMoss, TileID.Dirt, TileID.Mud, TileID.JungleGrass };

		public static void Run()
		{
			float worldSize = Main.maxTilesX / 4200f;
			for (int i = 0; i < 16 * worldSize; i++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 300);

				Tile tile = Main.tile[x, y];

				if (tile.HasTile) //Find open space
				{
					i--;
					continue;
				}

				while (!Main.tile[x, y].HasTile)
					y--;

				//We've found an opening and went to the ceiling, try and place a waterfall
				if (TryPlaceWaterfall(x, y))
				{
					i--;
					continue;
				}
			}
		}

		public static bool TryPlaceWaterfall(int i, int j)
		{
			if (ValidTypes.Contains(Main.tile[i, j].TileType))
			{
				PlaceWaterfall(i, j);
				return true;
			}
			return false;
		}

		private static void PlaceWaterfall(int i, int j)
		{
			int type = Main.tile[i, j].TileType;
			int height = WorldGen.genRand.Next(60, 80);

			GenerateChasm(i, j, height);
			AddFalls(i, j);
		}

		private static void AddFalls(int i, int j)
		{
			int realX = i + WorldGen.genRand.Next(-5, 0);
			int width = WorldGen.genRand.Next(2, 5);

			for (int x = realX; x < realX + (width * 2); x += 2)
			{
				DigFall(x, j, WorldGen.genRand.Next(1, 6));

				if (WorldGen.genRand.NextBool(4))
					x++;
			}
		}

		private static void DigFall(int x, int y, int minHeight)
		{
			bool CanPlaceAt(int i, int j, int dir) => Main.tile[i, j].HasTile && Main.tile[i + dir, j].HasTile && Main.tile[i + dir + dir, j - 1].HasTile;

			void PlaceAt(int i, int j, int dir)
			{
				Tile liquidTile = Main.tile[i + dir, j - 1];
				liquidTile.ClearTile();
				liquidTile.LiquidAmount = 255;
				liquidTile.LiquidType = LiquidID.Water;

				Tile halfTile = Main.tile[i, j - 1];
				halfTile.IsHalfBlock = true;

				if (halfTile.TileType == TileID.Silt || liquidTile.TileType == TileID.Sand)
					liquidTile.TileType = TileID.Stone;

				Tile silt = Main.tile[i + dir + dir, j - 1];
				if (silt.TileType == TileID.Silt || silt.TileType == TileID.Sand)
					silt.TileType = TileID.Stone;

				silt = Main.tile[i + dir, j];
				if (silt.TileType == TileID.Silt || silt.TileType == TileID.Sand)
					silt.TileType = TileID.Stone;

				int adjY = 0;
				while (Main.tile[i - dir, j - 1 + adjY].HasTile)
					Main.tile[i - dir, j - 1 + adjY++].ClearTile();
			}

			const int MaxHeight = 16;

			for (int j = y; j > y - MaxHeight; --j)
			{
				if (Math.Abs(j - y) < minHeight)
					continue;

				if (CanPlaceAt(x, j, -1))
				{
					PlaceAt(x, j, -1);
					return;
				}
				else if (CanPlaceAt(x, j, 1))
				{
					PlaceAt(x, j, 1);
					return;
				}
			}
		}

		private static void GenerateChasm(int i, int j, int height)
		{
			ushort type = Main.tile[i, j].TileType;
			int lOffsetX = 5;
			int rOffsetX = 5;

			for (int y = j; y < j + height; ++y)
			{
				for (int x = i - lOffsetX - 1; x < i + rOffsetX + 1; ++x)
				{
					Tile tile = Main.tile[x, y];

					if ((x == i - lOffsetX - 1 || x == i + rOffsetX) && tile.HasTile) //Line the walls
						WorldGen.ReplaceTile(x, y, type, 0);
					else if (y - j < height * 0.9f)//Clear the way
					{
						tile.ClearTile();

						if (y - j > height * 0.75f) //(and add water at the bottom)
						{
							tile.LiquidAmount = 255;
							tile.LiquidType = LiquidID.Water;
						}
					}
					else if (y - j >= height * 0.9f)
					{
						if (WorldMethods.AdjacentOpening(x, y) && tile.HasTile)
							WorldGen.ReplaceTile(x, y, type, 0);
					}
				}

				if (y - j > height * 0.75f) //Randomize sizes
				{
					if (y - j > height * 0.8f)
					{
						lOffsetX -= WorldGen.genRand.Next(0, 3);
						rOffsetX -= WorldGen.genRand.Next(0, 3);
					}
					else
					{
						lOffsetX += WorldGen.genRand.Next(0, 3);
						rOffsetX += WorldGen.genRand.Next(0, 3);
					}
				}
				else
				{
					lOffsetX += WorldGen.genRand.Next(-1, 2);
					rOffsetX += WorldGen.genRand.Next(-1, 2);
				}

				int max = y - j > height * 0.75f ? 14 : 7;
				lOffsetX = (int)MathHelper.Clamp(lOffsetX, 3, max);
				rOffsetX = (int)MathHelper.Clamp(rOffsetX, 3, max);
			}
		}
	}
}
