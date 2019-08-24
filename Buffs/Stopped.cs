using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs

{
	public class Stopped : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Stopped");
			Description.SetDefault("You are locked in place");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).Stopped = true;
			if (!npc.boss)
			{
				npc.velocity *= 0;

				npc.frame.Y = 0;
			}
			if (Main.rand.Next(4) == 0)
			{
				int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, 206);

				Main.dust[dust1].scale = 1f;
			}
		}
	}
}