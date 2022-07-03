using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ReachPetal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Petal");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 12;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.damage = 10;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain);
			SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.position, 0.4f / 2, .12f / 2, .036f / 2);
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			int num1222 = 5;
			for (int k = 0; k < 2; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.LifeDrain, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num1222 * (float)k;
				Main.dust[index2].scale = .95f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = true;
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
	}
}