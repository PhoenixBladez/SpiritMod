using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Candy
{
	public class HealthBuffC : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Healing Candy");
			Description.SetDefault("+25 health.");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			 player.statLifeMax2 += 25;
		}
	}
}
