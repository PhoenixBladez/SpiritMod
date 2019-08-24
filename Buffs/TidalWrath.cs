using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class TidalWrath : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tidal Wrath");
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.wet = true;
		}
	}
}
