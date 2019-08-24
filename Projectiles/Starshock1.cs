using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Starshock1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 5f)
			{
				projectile.velocity.Y = projectile.velocity.Y + 0.01f;
				projectile.velocity.X = projectile.velocity.X * 1.01f;
				projectile.alpha -= 23;
				projectile.scale = 0.8f * (255f - (float)projectile.alpha) / 255f;
				if (projectile.alpha < 0)
					projectile.alpha = 0;
			}
			if (projectile.alpha >= 255 && projectile.ai[0] > 5f)
			{
				projectile.Kill();
				return;
			}

			if (Main.rand.Next(8) == 0)
			{
				int num193 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num193].position = projectile.Center;
				Main.dust[num193].scale += (float)Main.rand.Next(50) * 0.01f;
				Main.dust[num193].noGravity = true;
				Dust expr_835F_cp_0 = Main.dust[num193];
				expr_835F_cp_0.velocity.Y = expr_835F_cp_0.velocity.Y - 2f;
			}
			if (Main.rand.Next(6) == 0)
			{
				int num194 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 176, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num194].position = projectile.Center;
				Main.dust[num194].scale += 0.3f + (float)Main.rand.Next(50) * 0.01f;
				Main.dust[num194].noGravity = true;
				Main.dust[num194].velocity *= 0.1f;
			}

			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7, 1f, 0f);
			}
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("Wrath"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 5;
			projectile.height = 5;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (int num621 = 0; num621 < 20; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for (int num623 = 0; num623 < 35; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
				num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num624].velocity *= 2f;
			}
		}
	}
}