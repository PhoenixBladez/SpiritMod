using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SpiritMod.Projectiles.Yoyo
{
	public class MesopelagicProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Mesopelagic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(546);
			projectile.extraUpdates = 1;
			aiType = 546;
		}

		public override void PostAI()
		{
			projectile.rotation -= 10f;
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.2f;
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
