using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteCrystalOre : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			ItemDrop = ModContent.ItemType<BismiteCrystal>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bismite Crystal");
			AddMapEntry(new Color(30, 100, 25), name);
			HitSound = SoundID.Tink;
			DustType = DustID.Plantera_Green;
			Main.tileBlendAll[Type] = true;
		}
	}
}