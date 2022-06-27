using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class SaucerPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Support Saucer");
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
			Projectile.width = 40;
			Projectile.height = 30;
		}

		public override bool PreAI()
		{
			int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, -1f, 0, default, 1f);
			Main.dust[d].scale *= 0.5f;

			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.saucerPet = false;

			if (modPlayer.saucerPet)
				Projectile.timeLeft = 2;
		}

	}
}