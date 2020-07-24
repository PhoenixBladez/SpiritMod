using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Ambient.Briar;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class BriarGrass : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			SetModTree(new ReachTree());
			Main.tileMerge[Type][mod.TileType("BriarGrass")] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			TileID.Sets.Grass[Type] = true;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Dirt;
			//Main.tileLighted[Type] = true;
			AddMapEntry(new Color(104, 156, 70));
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
				//   Main.PlaySound(SoundID.Dig, x * 16, y * 16, 1, 1f, 0f);
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
				if (!tile.bottomSlope() && !tile.topSlope()) {
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
		}

		public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return mod.TileType("ReachSapling");
		}

	}
}

