using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CobaltStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Shard");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 240;
			projectile.penetrate = 2;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation += .1f;
			float speed = (projectile.timeLeft < 120) ? 12 : 6;
			float lerpspeed = (projectile.timeLeft < 180) ? 0.1f : 0.05f;
			float mindist = 25f;

			++projectile.ai[1];


			if (Main.player[projectile.owner].active && !Main.player[projectile.owner].dead) {
				if (projectile.Hitbox.Intersects(Main.player[projectile.owner].Hitbox))
					projectile.Kill();
				if (projectile.Distance(Main.player[projectile.owner].Center) <= mindist)
					return;
				Vector2 unitY = projectile.DirectionTo(Main.player[projectile.owner].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				projectile.velocity = Vector2.Lerp(projectile.velocity, unitY * speed, lerpspeed);
			}
			else {
				if (projectile.timeLeft > 30)
					projectile.timeLeft = 30;
				if (projectile.ai[0] == -1f)
					return;
				projectile.ai[0] = -1f;
				projectile.netUpdate = true;
			}
			int num = 3;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.Cobalt, 0.0f, 0.0f, 0, new Color(), .5f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .6f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override void Kill(int timeLeft)
		{

			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			for (int num623 = 0; num623 < 15; num623++) {
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Cobalt, 0f, 0f, 100, default(Color), .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			{
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for (int k = 0; k < projectile.oldPos.Length; k++) {
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}