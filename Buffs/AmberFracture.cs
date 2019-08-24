using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class AmberFracture : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Amber Fracture");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>(mod).amberFracture = true;
		}
	}
}
