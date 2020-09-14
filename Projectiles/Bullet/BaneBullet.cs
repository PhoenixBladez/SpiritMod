using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class BaneBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 6;
			projectile.ranged = true;
            projectile.hide = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 10000;
			projectile.extraUpdates = 1;
		}

		bool summoned = false;
		public override void AI()
		{
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3F, 0.0F, 0.5F);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (!summoned) {
				for (int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2)) * 1.3f;
					int num8 = Dust.NewDust(projectile.Center, 0, 0, 27, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = 1.5f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
				summoned = true;
			}
            for (int i = 0; i < 10; i++)
            {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 2, 2, 27);
                Main.dust[num].alpha = projectile.alpha;
                Main.dust[num].velocity = Vector2.Zero;
                Main.dust[num].noGravity = true;
            }
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
