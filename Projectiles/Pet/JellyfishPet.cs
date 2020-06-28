using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class JellyfishPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Support Saucer");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ZephyrFish);
			aiType = ProjectileID.ZephyrFish;
			projectile.width = 40;
			projectile.height = 30;
			projectile.scale = 0.9f;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(player.dead)
				modPlayer.jellyfishPet = false;

			if(modPlayer.jellyfishPet)
				projectile.timeLeft = 2;
		}

	}
}