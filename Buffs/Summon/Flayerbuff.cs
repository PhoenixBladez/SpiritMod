using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class Flayerbuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Flayer Minion");
			Description.SetDefault("Look at all the pretty colors!");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Flayer")] > 0)
			{
				modPlayer.Flayer = true;
			}
			if (!modPlayer.Flayer)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
