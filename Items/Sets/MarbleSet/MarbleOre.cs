using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleOre : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;  //true for block to emit light
			Main.tileLighted[Type] = true;
			ItemDrop = ModContent.ItemType<MarbleChunk>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Enchanted Marble Chunk");
			AddMapEntry(new Color(227, 191, 75), name);
			soundType = SoundID.Tink;
			MinPick = 65;
			DustType = DustID.GoldCoin;

		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .219f / 3;
			g = .199f / 3;
			b = .132f / 3;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if (!NPC.downedBoss2) {
				return false;
			}
			return true;
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (Main.rand.Next(1100) == 1) {
				int glyphnum = Main.rand.Next(10);
				DustHelper.DrawDustImage(new Vector2(i * 16, j * 16), ModContent.DustType<MarbleDust>(), 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1f);
			}
		}
	}
}