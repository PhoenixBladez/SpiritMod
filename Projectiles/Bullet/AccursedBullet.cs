using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class AccursedBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Accursed Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 6;

			projectile.ranged = true;
			projectile.friendly = true;


			projectile.penetrate = 2;
			projectile.timeLeft = 10000;

			projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.1F, 0.3F, 0.1F);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);

			for (int num383 = 0; num383 < 5; num383++) {
				int num384 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 75);
				Main.dust[num384].noGravity = true;
				Main.dust[num384].velocity *= 1.5f;
				Main.dust[num384].scale *= 0.9f;
			}
		}

	}
}
