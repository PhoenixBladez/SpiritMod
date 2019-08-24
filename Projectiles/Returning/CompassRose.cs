using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class CompassRose : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Compass Rose");
		}

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.magic = false;
			projectile.penetrate = 4;
			projectile.timeLeft = 500;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			{
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = 1.2f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(20) == 1)
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
