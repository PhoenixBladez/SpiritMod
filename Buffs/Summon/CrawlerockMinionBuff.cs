using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class CrawlerockMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crawlerock Minion");
			Description.SetDefault("A baby Cavern Crawler fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Crawlerock>()] > 0) {
				modPlayer.crawlerockMinion = true;
				player.buffTime[buffIndex] = 18000;
			}

			if (!modPlayer.crawlerockMinion) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
		}
	}
}
