using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CobaltStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Shard");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 240;
			Projectile.penetrate = 2;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation += .1f;
			float speed = (Projectile.timeLeft < 120) ? 12 : 6;
			float lerpspeed = (Projectile.timeLeft < 180) ? 0.1f : 0.05f;
			float mindist = 25f;

			++Projectile.ai[1];


			if (Main.player[Projectile.owner].active && !Main.player[Projectile.owner].dead) {
				if (Projectile.Hitbox.Intersects(Main.player[Projectile.owner].Hitbox))
					Projectile.Kill();
				if (Projectile.Distance(Main.player[Projectile.owner].Center) <= mindist)
					return;
				Vector2 unitY = Projectile.DirectionTo(Main.player[Projectile.owner].Center);
				if (unitY.HasNaNs())
					unitY = Vector2.UnitY;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, unitY * speed, lerpspeed);
			}
			else {
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				if (Projectile.ai[0] == -1f)
					return;
				Projectile.ai[0] = -1f;
				Projectile.netUpdate = true;
			}
			int num = 3;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Cobalt, 0.0f, 0.0f, 0, new Color(), .5f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .6f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override void Kill(int timeLeft)
		{

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int num623 = 0; num623 < 15; num623++) {
				int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Cobalt, 0f, 0f, 100, default, .31f);
				Main.dust[num624].velocity *= .5f;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++) {
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}