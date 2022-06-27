using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class ReachSummonBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briar Spirit");
			Description.SetDefault("A Briar Spirit fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ReachSummon>()] > 0) {
				modPlayer.ReachSummon = true;
				player.buffTime[buffIndex] = 18000;
			}

			if (!modPlayer.ReachSummon) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
		}
	}
}
