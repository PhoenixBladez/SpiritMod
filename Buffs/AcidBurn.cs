using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.NPCs;

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
			GNPC info = npc.GetGlobalNPC<GNPC>(mod);
			if (info.acidBurnStacks < 2)
				info.acidBurnStacks++;
			npc.buffTime[buffIndex] = time;
			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>(mod);
			if (info.acidBurnStacks <= 0)
				info.acidBurnStacks = 1;
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 107);
			}
			npc.defense -= 2;
		}
	}
}
