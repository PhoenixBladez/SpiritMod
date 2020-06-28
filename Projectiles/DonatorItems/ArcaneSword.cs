using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class ArcaneSword : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Sword");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 2;
			projectile.timeLeft = 300;
			projectile.tileCollide = true;
			projectile.magic = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}
		int timer;
		int colortimer;
		public override bool PreAI()
		{
			timer++;
			if(timer <= 50) {
				colortimer++;
			}
			if(timer > 50) {
				colortimer--;
			}
			if(timer >= 100) {
				timer = 0;
			}
			float num = 1f - (float)projectile.alpha / 255f;
			num *= projectile.scale;
			float num395 = Main.mouseTextColor / 155f - 0.35f;
			num395 *= 0.34f;
			projectile.scale = num395 + 0.55f;
			Lighting.AddLight(projectile.Center, 0.1f * num, 0.2f * num, 0.4f * num);
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			return true;
		}
		public override void Kill(int timeLeft)
		{

			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for(int num257 = 0; num257 < 24; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 187, 0f, 0f, 0, default(Color), 1.2f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for(int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(60 + colortimer * 2, 60 + colortimer * 2, 60 + colortimer * 2, 100);
		}
	}
}
