using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod;
using SpiritMod.NPCs;

namespace SpiritMod.Buffs
{
	public class UnstableAffliction : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Unstable Affliction");
			Description.SetDefault("Reduces movement speed by 10%");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.wingTimeMax <= 0)
			{
				player.wingTimeMax = 0;
			}
			player.maxRunSpeed -= .10f;
			player.moveSpeed -= .10f;

		}
	}
}