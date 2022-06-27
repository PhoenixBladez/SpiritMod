using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class BarkTileTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(133, 104, 70));
			ItemDrop = ModContent.ItemType<AncientBark>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

