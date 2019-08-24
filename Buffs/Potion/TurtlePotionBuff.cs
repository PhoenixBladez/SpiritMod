using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Potion
{
	public class TurtlePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Steadfast");
			Description.SetDefault("Increases defense as health wanes\nReduces damage taken by 5%");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.statLife < 500)
			{
				player.statDefense += 3;
			}
			if (player.statLife < 300)
			{
				player.statDefense += 4;
			}
			if (player.statLife < 200)
			{
				player.statDefense += 5;
			}
			if (player.statLife < 100)
			{
				player.statDefense += 5;
			}
			player.endurance += 0.05f;
		}
	}
}
