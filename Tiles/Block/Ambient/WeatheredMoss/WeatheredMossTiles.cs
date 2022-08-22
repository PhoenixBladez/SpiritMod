using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace SpiritMod.Tiles.Block.Ambient.WeatheredMoss
{
	public abstract class WeatheredMossTile : ModTile
	{
		internal abstract Color MapColor { get; } 

		public override void SetStaticDefaults()
		{
			AmbientTileDefaults.SetTileData(Type);

			Main.tileMerge[Type][ModContent.TileType<WeatheredStoneTile>()] = true;
			Main.tileMerge[ModContent.TileType<WeatheredStoneTile>()][Type] = true;

			TileID.Sets.Grass[Type] = true;
			TileID.Sets.Conversion.Grass[Type] = true;

			AddMapEntry(MapColor);

			ItemDrop = ModContent.ItemType<WeatheredStoneItem>();
			HitSound = SoundID.Grass;
			DustType = DustID.Stone;
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			fail = true;

			Tile tile = Main.tile[i, j];
			tile.TileType = (ushort)ModContent.TileType<WeatheredStoneTile>();
		}

		public override void RandomUpdate(int i, int j)
		{
			CheckMoss(i - 1, j);
			CheckMoss(i + 1, j);
			CheckMoss(i, j - 1);
			CheckMoss(i, j + 1);
		}

		private void CheckMoss(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (Main.rand.NextBool(4) && Main.tile[i, j].HasTile && Main.tile[i, j].TileType == ModContent.TileType<WeatheredStoneTile>())
				tile.TileType = Type;
		}
	}

	public class WeatheredBlueMoss : WeatheredMossTile { internal override Color MapColor => new Color(43, 86, 140); }
	public class WeatheredGreenMoss : WeatheredMossTile { internal override Color MapColor => new Color(49, 134, 114); }
	public class WeatheredPurpleMoss : WeatheredMossTile { internal override Color MapColor => new Color(121, 49, 134); }
	public class WeatheredRedMoss : WeatheredMossTile { internal override Color MapColor => new Color(134, 59, 49); }
	public class WeatheredYellowMoss : WeatheredMossTile { internal override Color MapColor => new Color(126, 134, 49); }
}