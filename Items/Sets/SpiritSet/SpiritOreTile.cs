using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	public class SpiritOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			TileID.Sets.Ore[Type] = true;

			ItemDrop = ModContent.ItemType<SpiritOre>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Spirit Ore");
			AddMapEntry(new Color(30, 144, 255), name);
			HitSound = SoundID.Tink;
			MinPick = 180;

		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.2f;
			g = 0.4f;
			b = 1.4f;
		}
	}
}