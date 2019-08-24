using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class OverDrive : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Overdrive");
			Description.SetDefault("Your movement speed and throwing damage are charged up!");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += 0.2f;
			player.thrownDamage += 0.09f;
			int dust = Dust.NewDust(player.position, player.width, player.height, 226);
		}
	}
}
