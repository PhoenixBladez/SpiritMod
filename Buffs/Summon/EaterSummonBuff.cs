using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class EaterSummonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tiny Eater");
			Description.SetDefault("A tiny eater fights for you!");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("EaterSummon")] > 0)
			{
				modPlayer.EaterSummon = true;
			}
			if (!modPlayer.EaterSummon)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			player.buffTime[buffIndex] = 18000;
		}
	}
}
