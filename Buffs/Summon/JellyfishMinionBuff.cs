using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class JellyfishMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellyfish");
			Description.SetDefault("A cute, bouncy Jellyfish fights for you!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<JellyfishMinion>()] > 0) {
				modPlayer.jellyfishMinion = true;
				player.buffTime[buffIndex] = 18000;
			}

			if (!modPlayer.jellyfishMinion) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
		}
	}
}
