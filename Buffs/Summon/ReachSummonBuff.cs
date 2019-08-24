using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class ReachSummonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Briar Spirit");
			Description.SetDefault("A Briar Spirit fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("ReachSummon")] > 0)
			{
				modPlayer.ReachSummon = true;
			}
			if (!modPlayer.ReachSummon)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
