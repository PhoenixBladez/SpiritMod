using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Candy
{
	public class ManaBuffC : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mana Candy");
			Description.SetDefault("+40 Mana");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statManaMax2 += 40;
		}
	}
}
