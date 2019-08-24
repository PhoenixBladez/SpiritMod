using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class Wobbly : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wobbly");
			Description.SetDefault("You feel groggy, becuase your damage is reduced by 3% and your defense is reduced by 2");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense = (player.statDefense / 100) * 98;
			player.magicDamage *= 0.97f;
			player.meleeDamage *= 0.97f;
			player.rangedDamage *= 0.97f;
			player.minionDamage *= 0.97f;
			player.thrownDamage *= 0.97f;

			Dust.NewDust(player.position, player.width, player.height, 62);
		}
	}
}
