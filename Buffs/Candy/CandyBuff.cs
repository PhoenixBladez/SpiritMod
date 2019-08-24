using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Candy
{
	public class CandyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sugar Rush");
			Description.SetDefault("Increased stats");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.5f;
			player.statDefense += 2;
			player.magicCrit += 2;
			player.meleeCrit += 2;
			player.thrownCrit += 2;
			player.rangedCrit += 2;
			player.magicDamage += 0.04f;
			player.meleeDamage += 0.04f;
			player.rangedDamage += 0.04f;
			player.minionDamage += 0.04f;
			player.thrownDamage += 0.04f;
			player.lifeRegen += 1;
			player.jumpSpeedBoost += 0.4f;
		}
	}
}
