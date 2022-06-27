using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class SepulchreBrick : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(87, 85, 81));
			ItemDrop = ModContent.ItemType<Items.Placeable.Tiles.SepulchreBrickItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sepulchre Roofing");
			DustType = DustID.Wraith;
		}
	}
}

