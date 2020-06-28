using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SepulchreBrick : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			soundType = 21;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(87, 85, 81));
			drop = ModContent.ItemType<SepulchreBrickItem>();
			dustType = 54;
		}
	}
}

