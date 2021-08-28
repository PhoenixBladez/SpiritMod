
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class AngelLightStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.timeLeft = 300;
			projectile.height = 28;
			projectile.alpha = 30;
			projectile.width = 18;
			projectile.penetrate = 5;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Clentaminator_Purple, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor * 2) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void AI()
		{
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.2f, 0.06f, 0.14f);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			for (int i = 0; i < 8; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.Clentaminator_Purple);
				Main.dust[num].scale *= .8f;
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
