using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
			projectile.penetrate = 1;
			projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			projectile.tileCollide = false;
			projectile.timeLeft = 100;
		}

		public override void AI()
		{
			if (++projectile.ai[0] > 20f) {
				bool canaccelerate = projectile.velocity.Length() < 18;
				projectile.tileCollide = !canaccelerate;
				if (canaccelerate)
					projectile.velocity = projectile.velocity * 1.02f;
			}

			projectile.rotation += 0.1f;

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

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, Main.projectileTexture[projectile.type].Bounds, projectile.GetAlpha(Color.White), projectile.rotation, 
				Main.projectileTexture[projectile.type].Size()/2, projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}