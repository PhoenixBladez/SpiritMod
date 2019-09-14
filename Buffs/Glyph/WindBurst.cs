using SpiritMod.Dusts;
using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class WindBurst : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wind Burst");
			Description.SetDefault("Knockback is amplified");
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().stormBurst = true;

			if (Main.rand.NextDouble() < 0.1)
			{
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, mod.DustType("Wind"));
				dust.customData = new WindAnchor(npc.Center, dust.position);
			}
		}
	}
}
