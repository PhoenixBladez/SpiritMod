using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class FaeBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fae Beam");
		}

		int target;

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.magic = true;
			projectile.width = 4;
			projectile.height = 20;
			projectile.timeLeft = 80;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 5f)
			{
				projectile.velocity.Y = projectile.velocity.Y + 0.01f;
				projectile.velocity.X = projectile.velocity.X * 1.0f;
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

			if (Main.rand.Next(4) == 0)
			{
				int num193 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					62, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num193].position = projectile.Center;
				Main.dust[num193].scale += Main.rand.Next(50) * 0.01f;
				Main.dust[num193].noGravity = true;
				Main.dust[num193].velocity.Y -= 2f;
			}
			if (Main.rand.Next(6) == 0)
			{
				int num194 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					62, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num194].position = projectile.Center;
				Main.dust[num194].scale += 0.3f + Main.rand.Next(50) * 0.01f;
				Main.dust[num194].noGravity = true;
				Main.dust[num194].velocity *= 0.1f;
			}

			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 1f;
				Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7, 1f, 0f);
			}
		}

	}
}