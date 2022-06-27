using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloranSet
{
	public class FloranOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			TileID.Sets.Ore[Type] = true;

			ItemDrop = ModContent.ItemType<FloranOre>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Floran Ore");
			AddMapEntry(new Color(30, 200, 25), name);
			soundType = SoundID.Tink;
			DustType = DustID.GrassBlades;
			MinPick = 40;
		}
	}
}