using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class HowlingBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Howling Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
			for (int num621 = 0; num621 < 40; num621++) {
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.5f;
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
			delegate {
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 1.1f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += (Main.rand.Next(-30, 31) / 20) - 1.5f;
					dust.position.Y += (Main.rand.Next(-30, 31) / 20) - 1.5f;
					if (dust.position != Projectile.Center)
						dust.velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			});
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(220, 220, 220, 100);
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
				Projectile.tileCollide = false;
				Projectile.ai[1] = 0f;
				Projectile.alpha = 255;
				Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
				Projectile.width = 30;
				Projectile.height = 30;
				Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
				Projectile.knockBack = 4f;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust].scale *= Main.rand.NextFloat(.4f, .6f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Frostburn, 180);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}