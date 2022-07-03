using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class Blaze : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Blaze");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.minion = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 500;
			Projectile.height = 18;
			Projectile.width = 10;
			Projectile.alpha = 50;
			Projectile.extraUpdates = 1;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool? CanHitNPC(NPC target) => target.immune[Projectile.owner] == 0;

		public override void AI()
		{
			Projectile.velocity.Y += 0.15f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.position, 0.4f, .12f, .036f);
			Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Dust dust2 = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			dust.noGravity = true;
			dust2.noGravity = true;
			dust2.velocity *= 0.6f;
			dust2.velocity *= 0.1f;
			dust2.scale = 1.2f;
			dust.scale = .8f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(6))
				target.AddBuff(BuffID.OnFire, 180);
			target.immune[Projectile.owner] = 10;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
			ProjectileExtras.Explode(Projectile.whoAmI, 80, 80,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -2f, 0, default, 1.2f);
						Main.dust[num].noGravity = true;
						Dust dust = Main.dust[num];
						dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						if (dust.position != Projectile.Center) 
							dust.velocity = Projectile.DirectionTo(dust.position) * 6f;
					}
				});

			for (int i = 0; i < 2; i++) {
				int gore = Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.Center.X - 24f, Projectile.Center.Y - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[gore].velocity *= 1 / 3f * (i + 1);
				Main.gore[gore].velocity.X += 1f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
