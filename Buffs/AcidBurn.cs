using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class AcidBurn : ModBuff
	{
		public override void SetDefaults() => DisplayName.SetDefault("Toxic Grasp");

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if (info.acidBurnStacks < 4)
				info.acidBurnStacks++;

			npc.buffTime[buffIndex] = time;

			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if (info.acidBurnStacks <= 0)
				info.acidBurnStacks = 1;

			if (Main.rand.NextBool(2)) {
				int a = Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 0, 1);
                int ab = Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 0, 1);
                int ac = Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 0, 1);
                Main.dust[a].alpha = 100;
                Main.dust[ab].alpha = 100;
                Main.dust[ac].alpha = 100;
                Main.dust[a].scale = Main.rand.NextFloat(.3f, .9f);
                Main.dust[ab].scale = Main.rand.NextFloat(.3f, .9f);
                Main.dust[ac].scale = Main.rand.NextFloat(.3f, .9f);
            }
		}
	}
}
