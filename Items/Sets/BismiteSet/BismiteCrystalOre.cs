using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteCrystalOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			drop = ModContent.ItemType<BismiteCrystal>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Bismite Crystal");
			AddMapEntry(new Color(30, 100, 25), name);
			soundType = SoundID.Tink;
			dustType = DustID.Plantera_Green;
			Main.tileBlendAll[this.Type] = true;

		}
	}
}