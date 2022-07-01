using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.World.Sepulchre
{
	public class SepulchreBrickTwo : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[Type] = true;
			HitSound = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.SepulchreBrickTwoItem>();
			AddMapEntry(new Color(87, 85, 81));
			DustType = DustID.Wraith;
		}
	}
}
