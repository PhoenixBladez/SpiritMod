
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ValkyrieSpearFriendly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Spear");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 40;
			Projectile.width = 10;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.DamageType = DamageClass.Melee;
			AIType = ProjectileID.Bullet;
		}
		float num;
		public override void AI()
		{
			if (Projectile.timeLeft >= 290) {
				Projectile.tileCollide = false;
			}
			else {
				Projectile.tileCollide = true;
			}
			num += .4f;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255 - (int)num * 5, 255 - (int)num * 5, 255 - (int)num * 5, 100 - (int)num * 3);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y, 1);
			// Vector2 vector9 = projectile.position;
			//Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			// vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++) {
				int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TopazBolt, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				// Main.dust[newDust].velocity += value19 * 2f;
				//  Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				//  vector9 -= value19 * 8f;
			}
		}
	}
}
