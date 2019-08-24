using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class DepthBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Ball");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 2;

			projectile.magic = true;
			projectile.friendly = true;
			projectile.timeLeft = 150;
			projectile.alpha = 255;
			projectile.penetrate = 4;
		}

		public override bool PreAI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].scale = 1.6f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;
			int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust1].scale = 1.6f;
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].noLight = true;

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();

			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y * 1.0f;

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(15) == 1)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
				ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
					delegate
				{
					for (int i = 0; i < 40; i++)
					{
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 15, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Dust expr_62_cp_0 = Main.dust[num];
						expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						if (Main.dust[num].position != projectile.Center)
						{
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
				});
			}
		}

	}
}
