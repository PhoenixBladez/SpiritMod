using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class MiniorBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Meteor");
			Description.SetDefault("A mini meteor fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Minior")] > 0)
			{
				modPlayer.minior = true;
			}
			if (!modPlayer.minior)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
