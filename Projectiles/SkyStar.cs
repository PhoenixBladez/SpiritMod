
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class SkyStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sky Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 300;
			projectile.height = 34;
			projectile.width = 34;
			projectile.penetrate = 5;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int i = 0; i < 16; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, .8f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
				dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
				if (dust.position != projectile.Center) {
					dust.velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
		public override void AI()
		{
			projectile.rotation += .2f;
			Lighting.AddLight(projectile.position, .055f, .054f, .223f);
			if (projectile.ai[0] == 0) {
				int num = 5;
				for (int k = 0; k < 5; k++) {
					int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.BlueCrystalShard, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
					Main.dust[index2].scale = .8f * projectile.scale;
					Main.dust[index2].velocity *= 0f;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].noLight = false;
				}
			}
		}
	}
}
