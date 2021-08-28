using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword
{
	public class BloodWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Wave");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 500;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool PreAI()
		{
			float num = 1f - (float)projectile.alpha / 255f;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			num *= projectile.scale;
			Lighting.AddLight(projectile.Center, 0.3f * num, 0.2f * num, 0.1f * num);
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

			int num12 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.LifeDrain, 0f, -2f, 0, default(Color), 2f);
			Main.dust[num12].noGravity = true;
			Main.dust[num12].velocity *= 0f;
			Main.dust[num12].scale *= 0.5f;
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 104, 102, 100);
		}
		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.LifeDrain, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}

	}
}