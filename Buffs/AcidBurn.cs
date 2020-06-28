using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class AcidBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Acid Burn");
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if(info.acidBurnStacks < 2) {
				info.acidBurnStacks++;
			}

			npc.buffTime[buffIndex] = time;

			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if(info.acidBurnStacks <= 0) {
				info.acidBurnStacks = 1;
			}

			npc.defense -= 2;

			if(Main.rand.NextBool(2)) {
				Dust.NewDust(npc.position, npc.width, npc.height, 107, 0, 1);
			}
		}
	}
}
