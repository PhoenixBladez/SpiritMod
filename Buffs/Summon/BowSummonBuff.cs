using SpiritMod.Projectiles.Summon.BowSummon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class BowSummonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Jinxbow");
			Description.SetDefault("Who's pulling the strings?");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<BowSummon>()] > 0) {
				modPlayer.bowSummon = true;
			}

			if (!modPlayer.bowSummon) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
