using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class LivingBriarWood : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			Main.tileMerge[TileID.LivingMahoganyLeaves][Type] = true;
			Main.tileMerge[Type][TileID.LivingMahoganyLeaves] = true;

			AddMapEntry(new Color(133, 104, 70));
			ItemDrop = ModContent.ItemType<AncientBark>();
		}
	}
}

