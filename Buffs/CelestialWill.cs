using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class CelestialWill : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Will of the Celestials");
			Description.SetDefault("The Celestials smile upon you...");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance += 0.1f;
			player.lifeRegen += 5;
		}
	}
}
