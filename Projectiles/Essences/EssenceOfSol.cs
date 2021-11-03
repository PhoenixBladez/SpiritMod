using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Essences
{
	class EssenceOfSol : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Essence of Sol");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 500;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;

			aiType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<SolarExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			projectile.position = projectile.Center;
			projectile.height = 50;
			projectile.position.X = projectile.position.X - (projectile.width / 2f);
			projectile.position.Y = projectile.position.Y - (projectile.height / 2f);

			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
				Main.dust[dust].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[dust].scale = 0.5f;
					Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}
			for (int i = 0; i < 35; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 3f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 5f;
				dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
				Main.dust[dust].velocity *= 2f;
			}

			for (int i = 0; i < 3; i++)
			{
				float scaleFactor10 = 0.33f;
				if (i == 1)
					scaleFactor10 = 0.66f;
				else if (i == 2)
					scaleFactor10 = 1f;

				for (int j = 0; j < 4; ++j)
				{
					int gore = Gore.NewGore(new Vector2(projectile.position.X + (projectile.width / 2f) - 24f, projectile.position.Y + (projectile.height / 2f) - 24f), default, Main.rand.Next(61, 64), 1f);
					Main.gore[gore].velocity *= scaleFactor10;
					Main.gore[gore].velocity.X += 1f;
					Main.gore[gore].velocity.Y += 1f;
				}
			}

			projectile.position = projectile.Center;
			projectile.width = 10;
			projectile.height = 10;
			projectile.position.X = projectile.position.X - (projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (projectile.height / 2);
		}

		public override void AI()
		{
			for (int i = 0; i < 3; ++i)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.CopperCoin);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 0.6f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => projectile.Kill();
	}
}