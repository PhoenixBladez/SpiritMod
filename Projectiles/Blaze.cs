using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class Blaze : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Blaze");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 500;
			projectile.height = 18;
			projectile.width = 10;
			projectile.alpha = 50;
			projectile.CloneDefaults(ProjectileID.BoneArrow);
			projectile.extraUpdates = 1;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void AI()
		{
			Lighting.AddLight(projectile.position, 0.4f, .12f, .036f);
			Dust dust = Dust.NewDustDirect(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Flare, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Dust dust2 = Dust.NewDustDirect(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Flare, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
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

			target.immune[projectile.owner] = 10;
		}

		public override bool? CanHitNPC(NPC target) => target.immune[projectile.owner] == 0;

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 74);
			ProjectileExtras.Explode(projectile.whoAmI, 80, 80,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, -2f, 0, default, 1.2f);
						Main.dust[num].noGravity = true;
						Dust dust = Main.dust[num];
						dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
						if (dust.position != projectile.Center) 
							dust.velocity = projectile.DirectionTo(dust.position) * 6f;
					}
				});

			for (int i = 0; i < 2; i++) {
				int gore = Gore.NewGore(new Vector2(projectile.Center.X - 24f, projectile.Center.Y - 24f), default, Main.rand.Next(61, 64), 1f);
				Main.gore[gore].velocity *= 1 / 3f * (i + 1);
				Main.gore[gore].velocity.X += 1f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
