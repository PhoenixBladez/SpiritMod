using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class Death : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Death");


			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.boss == false)
			{
				npc.GetGlobalNPC<GNPC>(mod).Death = true;
			}
		}
	}
}
