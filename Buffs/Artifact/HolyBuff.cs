using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Artifact
{
	public class HolyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cleansed");
			Description.SetDefault("You've been purified\nReduces damage taken by 20% and mana cost by 15%");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance += 0.2f;
			player.manaCost -= 0.15f;
		}
	}
}
