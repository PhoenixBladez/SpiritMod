using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class SteamBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steam Beam");
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 4;
			projectile.timeLeft = 500;
			projectile.height = 6;
			projectile.width = 6;

			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 100;
		}

		public override void AI()
		{
			for (int num447 = 0; num447 < 2; num447++)
			{
				Vector2 vector33 = projectile.position;
				vector33 -= projectile.velocity * ((float)num447 * 0.25f);
				projectile.alpha = 255;
				int num448 = Dust.NewDust(vector33, 1, 1, 187, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.25f);
				Main.dust[num448].noGravity = true;
				Main.dust[num448].position = vector33;
				Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num448].velocity *= 0.2f;
			}
			return;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				projectile.ai[0] += 0.1f;
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.velocity *= 0.75f;
			}
			return false;
		}

	}
}
