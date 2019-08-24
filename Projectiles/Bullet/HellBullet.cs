using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class HellBullet : ModProjectile
	{
		private int Counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 16;
			projectile.aiStyle = 1;
			projectile.extraUpdates = 10;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 240;
			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 6);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}

			Counter++;
			if (Counter % 35 == 1)
			{
				for (int i = 0; i < 2; ++i)
				{
					int randFire = Main.rand.Next(3);
					int newProj = Projectile.NewProjectile(projectile.Center, new Vector2(0, -4),
						ProjectileID.GreekFire1 + randFire, projectile.damage / 2, 0, projectile.owner);
					Main.projectile[newProj].hostile = false;
					Main.projectile[newProj].friendly = true;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			int n = 2;
			int deviation = Main.rand.Next(0, 300);
			for (int z = 0; z < n; z++)
			{
				float rotation = MathHelper.ToRadians(270 / n * z + deviation);
				Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
				perturbedSpeed.Normalize();
				perturbedSpeed *= 5.5f;
				int newProj = Projectile.NewProjectile(projectile.Center, perturbedSpeed,
					ProjectileID.GreekFire1, 30, 2, projectile.owner);

				Main.projectile[newProj].hostile = false;
				Main.projectile[newProj].friendly = true;
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
					Main.dust[dust].noGravity = true;
				}
				Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
			}
		}


	}
}
