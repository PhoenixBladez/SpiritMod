using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.Briar;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class BriarGrass : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][mod.TileType("BriarGrass")] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.Grass[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Dirt;
			TileID.Sets.Conversion.Grass[Type] = true;

			AddMapEntry(new Color(104, 156, 70));
			SetModTree(new ReachTree());

			drop = ItemID.DirtBlock;
		}

		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			//Try place vine
			if (WorldGen.genRand.NextBool(15) && !tileBelow.active() && !tileBelow.lava())
			{
				if (!tile.bottomSlope())
				{
					tileBelow.type = (ushort)ModContent.TileType<BriarVines>();
					tileBelow.active(true);
					WorldGen.SquareTileFrame(i, j + 1, true);
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
					}
				}
			}

			//try place foliage
			if (WorldGen.genRand.NextBool(25) && !tileAbove.active() && !tileBelow.lava())
			{
				if (!tile.bottomSlope() && !tile.topSlope() && !tile.halfBrick() && !tile.topSlope())
				{
					tileAbove.type = (ushort)ModContent.TileType<BriarFoliage>();
					tileAbove.active(true);
					tileAbove.frameY = 0;
					tileAbove.frameX = (short)(WorldGen.genRand.Next(8) * 18);
					WorldGen.SquareTileFrame(i, j + 1, true);
					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
				}
			}

			// Try place super sunflower
			if (Main.hardMode && WorldGen.genRand.NextBool(500))
				if (WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SuperSunFlower>(), true))
					MyWorld.superSunFlowerPositions.Add(new Point16(i, j - 1));

			//Try spread grass
			if (Main.rand.NextBool(Main.dayTime && j < Main.worldSurface ? 5 : 8))
			{
				List<Point> adjacents = OpenAdjacents(i, j, TileID.Dirt);
				if (adjacents.Count > 0)
				{
					Point p = adjacents[Main.rand.Next(adjacents.Count)];
					if (HasOpening(p.X, p.Y))
					{
						Framing.GetTileSafely(p.X, p.Y).type = (ushort)ModContent.TileType<BriarGrass>();
						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, p.X, p.Y, 1, TileChangeType.None);
					}
				}
			}
		}

		private List<Point> OpenAdjacents(int i, int j, int type)
		{
			var p = new List<Point>();
			for (int k = -1; k < 2; ++k)
				for (int l = -1; l < 2; ++l)
					if (!(l == 0 && k == 0) && Framing.GetTileSafely(i + k, j + l).active() && Framing.GetTileSafely(i + k, j + l).type == type)
						p.Add(new Point(i + k, j + l));
			return p;
		}

		private bool HasOpening(int i, int j)
		{
			for (int k = -1; k < 2; ++k)
				for (int l = -1; l < 2; ++l)
					if (!Framing.GetTileSafely(i + k, j + l).active())
						return true;
			return false;
		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return ModContent.TileType<ReachSapling>();
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!fail) //Change self into dirt
			{
				fail = true;
				Framing.GetTileSafely(i, j).type = TileID.Dirt;
			}
		}
	}
}
