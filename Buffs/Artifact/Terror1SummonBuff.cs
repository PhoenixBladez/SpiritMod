using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class Terror1SummonBuff : ModBuff
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
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Terror1Summon")] > 0)
			{
				modPlayer.terror1Summon = true;
			}
			if (!modPlayer.terror1Summon)
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
