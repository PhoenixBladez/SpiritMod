using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SpiritWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Wave");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 55;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 60;
			projectile.penetrate = -1;
			projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 8);
			}

			else if (projectile.ai[0] == 1f)
			{
				projectile.scale -= 0.01f;
				projectile.alpha += 50;
				if (projectile.alpha >= 255)
				{
					projectile.ai[0] = 2f;
					projectile.alpha = 255;
				}
			}

			projectile.rotation = projectile.velocity.ToRotation() + 4.71F;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 206, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;

			Main.dust[dust2].scale = 2f;
			Main.dust[dust].scale = 2f;
			return false;
		}

	}
}