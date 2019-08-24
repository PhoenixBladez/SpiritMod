using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class BlizzardWrath : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Blizzard Wrath");
			Description.SetDefault("The Blizzard surrounds you... \n Increases magic damage and reduces mana consumption");

			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.magicDamage += 0.09f;
			player.manaCost -= 0.11f;
			{

				Dust.NewDust(player.position, player.width, player.height, 135);
				Dust.NewDust(player.position, player.width, player.height, 135);
			}
		}
	}
}
