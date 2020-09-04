using SpiritMod.Projectiles.Summon.Snapspore;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class SnapsporeBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Snapspore");
			Description.SetDefault("A bouncy Snapspore fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SnapsporeMinion>()] > 0) {
				modPlayer.snapsporeMinion = true;
			}

			if (!modPlayer.snapsporeMinion) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			player.buffTime[buffIndex] = 18000;
		}
	}
}
