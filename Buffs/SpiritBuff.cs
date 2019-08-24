using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class SpiritBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spirit Aura");
			Description.SetDefault("Increases damage and critical strike chance by 5 % \n Getting hurt occasionally spawns a damaging bolt to chase enemies");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			player.magicDamage *= 1.05f;
			player.meleeDamage *= 1.05f;
			player.rangedDamage *= 1.05f;
			player.minionDamage *= 1.05f;
			player.thrownDamage *= 1.05f;
			player.magicCrit += 5;
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.thrownCrit += 5;
			modPlayer.spiritBuff = true;
		}
	}
}
