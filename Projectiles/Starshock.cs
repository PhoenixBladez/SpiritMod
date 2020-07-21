using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Starshock : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starshock");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = 1;
			projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 5f) {
				projectile.velocity.Y = projectile.velocity.Y + 0.01f;
				projectile.velocity.X = projectile.velocity.X * 1.003f;
				projectile.alpha -= 23;
				projectile.scale = 0.8f * (255f - (float)projectile.alpha) / 255f;
				if (projectile.alpha < 0)
					projectile.alpha = 0;
			}
			if (projectile.alpha >= 255 && projectile.ai[0] > 5f) {
				projectile.Kill();
				return;
			}

			int num = 5;
			for (int k = 0; k < 2; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 180, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}

			if (projectile.localAI[1] == 0f) {
				projectile.localAI[1] = 1f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 3; k++) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 180, projectile.oldVelocity.X * 0.1f, projectile.oldVelocity.Y * 0.1f);
				Main.dust[d].noGravity = true;
			}
		}
	}
}