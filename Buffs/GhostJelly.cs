﻿using SpiritMod.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Buffs
{
	public class GhostJelly : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghostly Wrath");
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();
			if (info.GhostJellyStacks < 5) {
				info.GhostJellyStacks++;
			}

			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			GNPC info = npc.GetGlobalNPC<GNPC>();

			if (info.GhostJellyStacks == 0) {
				info.GhostJellyStacks = 1;
			}

			for (int i = 0; i < info.GhostJellyStacks; ++i) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.UnusedWhiteBluePurple);
			}
		}
	}
}
