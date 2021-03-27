using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class LavaRockSummonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slagtern");
			Description.SetDefault("The power of magma flows through this lantern");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.LavaRockSummon>()] > 0) {
				modPlayer.lavaRock = true;
				player.buffTime[buffIndex] = 18000;
			}

			if (!modPlayer.lavaRock) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
		}
	}
}
