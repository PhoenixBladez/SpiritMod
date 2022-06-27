using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Ambient.CrystalFlower
{
	public class CrystalFlowerGlobal : GlobalTile
	{
		public override void RandomUpdate(int i, int j, int type)
		{
			Tile t = Framing.GetTileSafely(i, j);

			bool validFrameX = t.TileFrameX <= 18 && t.TileFrameX >= 72 && t.TileFrameX < 108;
			if (type == TileID.Cactus && validFrameX && t.TileFrameY == 108 && Main.rand.NextBool(8))
			{
				WorldGen.PlaceTile(i, j - 1, ModContent.TileType<CrystalFlower>(), false, true, -1, 0);
			}
		}
	}
}
