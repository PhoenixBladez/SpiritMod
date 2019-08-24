using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs.Candy
{
	public class TaffyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Taffy");
			Description.SetDefault("+4 Defense");

			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4;
		}
	}
}
