using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class FrenzyVirus1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Viral Wrath");
			Description.SetDefault("The virus has mutated within you...\nIncreases damage");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.magicDamage += 0.05f;
			player.meleeDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.thrownDamage += 0.05f;
			player.minionDamage += 0.05f;
		}
	}
}
