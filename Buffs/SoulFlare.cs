using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class SoulFlare : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Soul Flare");
			Description.SetDefault("Your soul is fluctuating...");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0)
				player.lifeRegen = 0;
			player.lifeRegen -= 17;
			player.statDefense -= 4;

			if (Main.rand.Next(4) == 1)
			{

				Dust.NewDust(player.position, player.width, player.height, 187);
			}
		}
	}
}
