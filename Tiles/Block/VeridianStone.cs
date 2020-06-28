using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class VeridianStone : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<VeridianStone>()] = true;
			AddMapEntry(new Color(0, 191, 255));
			soundType = 21;
			drop = ModContent.ItemType<SpiritDirtItem>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}