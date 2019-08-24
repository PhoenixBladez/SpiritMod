using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class Terror4SummonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Terror Fiend");
			Description.SetDefault("It's taken a liking to you");

			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.minionDamage += .07f;
			player.minionKB += 0.05f;
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Terror4Summon")] > 0)
			{
				modPlayer.terror4Summon = true;
			}
			if (!modPlayer.terror4Summon)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
