using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ShadowflameStoneBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Wisp");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 300;
			projectile.height = 12;
			projectile.width = 12;
			projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff, 0f, -2f, 0, default, .9f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-10, 11) / 20) - 1.5f;
				dust.position.Y += (Main.rand.Next(-10, 11) / 20) - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

		public override void AI() => projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.tileCollide == true) {
				Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 6);
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.ShadowFlame, 85);
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
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(190, 79, 252, 100);
		}
	}
}
