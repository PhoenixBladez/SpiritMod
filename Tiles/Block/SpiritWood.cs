using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SpiritWood : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(128, 128, 128));
			drop = ModContent.ItemType<SpiritWoodItem>();
		}

		public override bool CanExplode(int i, int j)
		{
			return true;
		}
	}
}

