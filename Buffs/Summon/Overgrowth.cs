using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class Overgrowth : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Overgrowth Spirit");
			Description.SetDefault("The overgrowth spirit will protect you!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.Overgrowth>()] > 0) {
				modPlayer.OG = true;
				player.buffTime[buffIndex] = 18000;
			}

			if (!modPlayer.OG) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
		}
	}
}
