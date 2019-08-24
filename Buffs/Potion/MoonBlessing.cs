using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Potion
{
	public class MoonBlessing : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Moon Jelly");
			Description.SetDefault("Extreme regeneration");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 14;
		}
	}
}
