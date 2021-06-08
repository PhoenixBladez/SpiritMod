using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.Briar;
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

		public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1)
		{
			TileObject toBePlaced;
			if (!TileObject.CanPlace(x, y, type, style, direction, out toBePlaced, false)) {
				return false;
			}
			toBePlaced.random = random;
			if (TileObject.Place(toBePlaced) && !mute) {
				WorldGen.SquareTileFrame(x, y, true);
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Tile tileBelow = Framing.GetTileSafely(i, j + 1);
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			//Try place vine
			if (WorldGen.genRand.NextBool(15) && !tileBelow.active() && !tileBelow.lava()) {
				if (!tile.bottomSlope()) {
					tileBelow.type = (ushort)ModContent.TileType<Tiles.Ambient.Briar.BriarVines>();
					tileBelow.active(true);
					WorldGen.SquareTileFrame(i, j + 1, true);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, i, j + 1, 3, TileChangeType.None);
					}
				}
			}

			//try place foliage
			if (WorldGen.genRand.NextBool(25) && !tileAbove.active() && !tileBelow.lava()) {
				if (!tile.bottomSlope() && !tile.topSlope() && !tile.halfBrick() && !tile.topSlope())
                {
					tileAbove.type = (ushort)ModContent.TileType<BriarFoliage>();
					tileAbove.active(true);
					tileAbove.frameY = 0;
					tileAbove.frameX = (short)(WorldGen.genRand.Next(8) * 18);
					WorldGen.SquareTileFrame(i, j + 1, true);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, i, j - 1, 3, TileChangeType.None);
					}
				}
			}

			// Try place super sunflower
			if (Main.hardMode && WorldGen.genRand.NextBool(500))
				if (WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SuperSunFlower>(), true))
					MyWorld.superSunFlowerPositions.Add(new Point16(i, j - 1));
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

