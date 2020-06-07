using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
    public class LihzahrdMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Minion");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<LihzahrdMinion>()] > 0)
			{
				modPlayer.lihzahrdMinion = true;
			}

			if (!modPlayer.lihzahrdMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
