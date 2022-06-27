﻿using SpiritMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class NebulaFlame : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nebula Flame");
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if (info.nebulaFlameStacks < 5) {
				info.nebulaFlameStacks++;
			}

			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();

			if (info.nebulaFlameStacks == 0) {
				info.nebulaFlameStacks = 1;
			}

			for (int i = 0; i < info.nebulaFlameStacks; ++i) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.PinkFlame);
			}
		}
	}
}
