using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Items.Sets.GraniteSet
{
	public class GraniteOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = false;  //true for block to emit light
			Main.tileLighted[Type] = false;
			drop = ModContent.ItemType<GraniteChunk>();   //put your CustomBlock name
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Enchanted Granite Chunk");
			AddMapEntry(new Color(30, 144, 255), name);
			soundType = SoundID.Tink;
			minPick = 65;
			dustType = 226;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.155f / 2;
			g = 0.215f / 2;
			b = .4375f / 2;
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
			if (Main.rand.Next(255) == 1) {
				Dust.NewDustPerfect(new Vector2(i * 16, j * 16), 226, new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1.5f, 1.5f)), 0, default, 0.8f).noGravity = false;
			}
		}
	}
}