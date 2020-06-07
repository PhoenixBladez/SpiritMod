using SpiritMod.Items.Glyphs;
using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class WanderingPlague : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wandering Plague");
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

        public override bool ReApply(NPC npc, int time, int buffIndex) => true;

        public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().unholyPlague = true;

			if (Main.rand.NextDouble() < 0.25f)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<PlagueDust>(), npc.velocity.X, npc.velocity.Y);
            }

            UnholyGlyph.ReleasePoisonClouds(npc, npc.buffTime[buffIndex]);
		}
	}
}
