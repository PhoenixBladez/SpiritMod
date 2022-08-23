using SpiritMod.Tiles.Furniture;
using SpiritMod.Tiles.Furniture.Hanging;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.World.Micropasses
{
	internal class SunkenSurfaceMicropass
	{
		private static int[] ValidTypes = new int[] { TileID.Stone, TileID.Dirt, TileID.ClayBlock, TileID.ArgonMoss, TileID.BlueMoss, TileID.BrownMoss, TileID.GreenMoss, TileID.KryptonMoss, TileID.LavaMoss, TileID.PurpleMoss, 
			TileID.RedMoss, TileID.XenonMoss };

		public static void Run()
		{
			float worldSize = Main.maxTilesX / 4200f;
			for (int i = 0; i < 16 * worldSize; i++)
			{
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next((int)Main.worldSurface, (int)(Main.maxTilesY * 0.6f));

				Tile tile = Main.tile[x, y];
				if (tile.HasTile) //Find open space
				{
					i--;
					continue;
				}

				if (ValidTypes.Contains(tile.TileType))
					BuildSunkenSurface(x, y);
			}
		}

		public static void BuildSunkenSurface(int x, int y)
		{
			if (y > Main.maxTilesY - 200)
				return; //Hard stop if we're in the underworld

			int left = GetBound(x, y, -1);
			int right = GetBound(x, y, 1);
			int depth = 4;

			if (x - left < 15 || right - x < 15)
				depth = 2;
			else if (x - left < 7 || right - x < 7)
				depth = 1;

			for (int i = left; i < right; ++i)
			{
				int newY = WorldMethods.FindNearestBelow(i, y);

				if (newY > Main.maxTilesY - 200)
					continue; //Skip if we're in the underworld

				for (int j = newY - 2; j < newY + depth; ++j)
				{
					Tile tile = Main.tile[i, j];
					if (tile.HasTile && WorldGen.SolidTile(i, j))
					{
						WorldGen.ReplaceTile(i, j, TileID.Dirt, 0);

						if (WorldMethods.AdjacentOpening(i, j))
						{
							tile.TileType = TileID.Grass;

							GrowOnGrass(i, j, 0.05f);
						}
					}
				}
			}

			for (int i = left; i < right; ++i)
			{
				int newY = WorldMethods.FindNearestBelow(i, y);

				if (newY > Main.maxTilesY - 200)
					continue; //Skip if we're in the underworld

				Tile tile = Main.tile[i, newY + 1];
				if (WorldGen.genRand.NextBool(3) && WorldGen.SolidTile(i, newY + 1) && WorldGen.SolidTile(i + 1, newY + 1) && tile.TileType == TileID.Grass)
					WorldGen.PlaceObject(i, newY, ModContent.TileType<SunPotTile>(), true);

				newY = WorldMethods.FindNearestAbove(i, y);
				tile = Main.tile[i, newY];
				if (tile.HasTile && WorldGen.SolidTile(i, newY) && WorldGen.genRand.NextBool(8))
					WorldGen.PlaceObject(i, newY + 1, ModContent.TileType<HangingSunPotTile>(), true);
			}
		}

		private static void GrowOnGrass(int i, int j, float mod = 1f)
		{
			int type = Main.tile[i, j].TileType;
			int plantType = -1;

			int GetMod(int top)
			{
				int ret = (int)(top * mod);
				return ret <= 1 ? 1 : ret;
			}

			if (type == TileID.Grass && WorldGen.genRand.NextBool(GetMod(12)))
				plantType = TileID.Plants;
			else if (type == TileID.CorruptGrass && WorldGen.genRand.NextBool(GetMod(10)))
				plantType = TileID.CorruptPlants;
			else if (type == TileID.HallowedGrass && WorldGen.genRand.NextBool(GetMod(10)))
				plantType = TileID.HallowedPlants;
			else if (type == TileID.CrimsonGrass && WorldGen.genRand.NextBool(GetMod(10)))
				plantType = TileID.CrimsonPlants;

			if (plantType != -1 && !WorldGen.SolidTile(i, j - 1) && WorldGen.PlaceTile(i, j - 1, plantType, mute: true))
			{
				WorldGen.paintTile(i, j - 1, Main.tile[i, j].TileColor);

				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendTileSquare(-1, i, j - 1);
			}
		}

		private static int GetBound(int x, int y, int dir)
		{
			while (!Main.tile[x, y].HasTile || !WorldGen.SolidTile(x, y))
				x += dir;
			return x;
		}
	}
}
