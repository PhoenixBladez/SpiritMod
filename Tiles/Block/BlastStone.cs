using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Block
{
	public class BlastStone : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlendAll[this.Type] = true;
			soundType = SoundID.Tink;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(87, 85, 81));
			drop = ModContent.ItemType<Items.Placeable.Tiles.BlastStoneItem>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bedrock");
			dustType = 54;
		}
	}
}

