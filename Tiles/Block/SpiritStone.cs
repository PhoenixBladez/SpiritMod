using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SpiritStone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.Conversion.Stone[Type] = true;
			AddMapEntry(new Color(70, 130, 180));
			soundType = SoundID.Tink;
			ItemDrop = ModContent.ItemType<SpiritStoneItem>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

