using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class ScarabSandball : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 360;
			projectile.scale = 0.75f;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Sand, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.Center.X, (int)projectile.Center.Y);
		}

		public override void AI()
		{
			projectile.tileCollide = (projectile.position.Y >= projectile.ai[1]);

			projectile.rotation += 0.1f;
			for (int i = -2; i < 2; i++) {
				Dust dust = Dust.NewDustPerfect(projectile.Center + 2 * projectile.velocity, mod.DustType("SandDust"), Vector2.Normalize(projectile.velocity).RotatedBy(Math.Sign(i) * MathHelper.Pi / 4) * Math.Abs(i));
				dust.noGravity = true;
				dust.scale = 0.65f;
			}

			if (projectile.ai[0] == 0 && projectile.velocity.Y < 15f) {
				projectile.velocity.Y += 0.2f;
			}

			if (projectile.ai[0] == 1) {
				if(projectile.velocity.Y < 0) {
					projectile.velocity.Y += 0.01f;
					projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.05f);
				}
				else {
					projectile.velocity.X = 0;
					projectile.ai[1]++;
					if (projectile.ai[1] > 15)
						projectile.velocity.Y += 0.4f;
					else
						projectile.velocity.Y = 0f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, texture2D.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
				return false;
		}
	}
}