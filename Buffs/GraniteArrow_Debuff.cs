using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class GraniteArrow_Debuff : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Granite Arrow Debuff");
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.lifeRegen > 0)
				npc.lifeRegen = 0;

			npc.lifeRegen -= 12;
		}
	}
}
