using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class GraniteSpike1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grant Spike");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.penetrate = 3;
			projectile.timeLeft = 120;
			projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			for (int index1 = 0; index1 < 9; ++index1) {
				float num1 = projectile.velocity.X * 0.2f * (float)index1;
				float num2 = projectile.velocity.Y * -0.200000002980232f * index1;
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0.0f, 0.0f, 100, new Color(), 1.3f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].scale = .425f;
				Main.dust[index2].position.X -= num1;
				Main.dust[index2].position.Y -= num2;
			}
			projectile.velocity.Y += projectile.ai[0];
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.PiOver2;

			projectile.frameCounter++;
			if (projectile.frameCounter >= 4) {
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 2;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
			return false;
		}
		public override void Kill(int timeLeft)
		{
			Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				187, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);

			for (int i = 0; i < 4; i++) {
				float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					velocity.X, velocity.Y, mod.ProjectileType("GraniteShard1"), 13, projectile.knockBack, projectile.owner);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].velocity *= 4f;
			}
		}

	}
}