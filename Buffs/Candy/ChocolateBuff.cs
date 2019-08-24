using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Candy
{
	public class ChocolateBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Chocolate");
			Description.SetDefault("10% Increased Speed");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			 player.moveSpeed += 0.1f; 
		}
	}
}
