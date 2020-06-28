using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class VeridianDirt : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileMerge[Type][ModContent.TileType<VeridianStone>()] = true;
			Main.tileMergeDirt[Type] = true;
			AddMapEntry(new Color(0, 191, 255));
			drop = ModContent.ItemType<SpiritDirtItem>();
		}



		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

