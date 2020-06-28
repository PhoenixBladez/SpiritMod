using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class FleshStickProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Stick");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 8;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			projectile.rotation += 0.1f;
			{
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = .9f;
				Main.dust[dust2].noGravity = true;
			}
		}
	}
}
