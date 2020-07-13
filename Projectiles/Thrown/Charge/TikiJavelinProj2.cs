using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Charge
{
	public class TikiJavelinProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiki Javelin");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 36;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 1;
			projectile.light = 0;
			aiType = ProjectileID.ThrowingKnife;
		}
		public override void AI()
		{
			if(Main.rand.Next(8) == 0) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[d].scale *= 1.85f;
				Main.dust[d].noGravity = true;
			}
		}
		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for(int num257 = 0; num257 < 20; num257++) {
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 6, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}

		}
	}
}