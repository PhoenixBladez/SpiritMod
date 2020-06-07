using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
    public class HungryMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Hungry Minion");
			Description.SetDefault("It's taken a liking to you... and your flesh");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<HungryMinion>()] > 0)
            {
                modPlayer.hungryMinion = true;
            }

            if (!modPlayer.hungryMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
			if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }

            player.lifeRegen -= 4;
		}
	}
}
