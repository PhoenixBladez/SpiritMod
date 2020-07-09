using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Tiles.Block
{
	public class MarbleOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;  //true for block to emit light
		//	Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<MarbleChunk>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Enchanted Marble Chunk");
			AddMapEntry(new Color(227, 191, 75), name);
			soundType = SoundID.Tink;
			minPick = 65;
			dustType = DustID.GoldCoin;

		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .219f;
			g = .199f;
			b = .132f;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			if(!NPC.downedBoss2) {
				return false;
			}
			return true;
		}
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (Main.rand.Next(1500) == 1)
			{
				int glyphnum = Main.rand.Next(10);
				DustHelper.DrawDustImage(new Vector2(i * 16, j * 16), ModContent.DustType<MarbleDust>(), 0.04f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1f);
			}
		}
	}
}