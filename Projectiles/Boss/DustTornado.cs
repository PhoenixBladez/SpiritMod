
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Boss
{
	public class DustTornado : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Wave");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 120;
			projectile.tileCollide = true;
		}
		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 8) {
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 8;
			}
			if (projectile.timeLeft < 120) {
				projectile.alpha++;
			}
			projectile.spriteDirection = -projectile.direction;

			Dust newDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 0, 0f, 0f, 0, default(Color), 1f)];
			newDust.noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 40; num621++) {
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 0, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].velocity *= 3f;
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.85f;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle rectangle = new Rectangle(0, projectile.frame * tex.Height / Main.projFrames[projectile.type], tex.Width, tex.Height / Main.projFrames[projectile.type]);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, lightColor, projectile.rotation, rectangle.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}