using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ShadowflameStoneBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Wisp");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.tileCollide = true;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, -2f, 0, default, .9f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-10, 11) / 20) - 1.5f;
				dust.position.Y += (Main.rand.Next(-10, 11) / 20) - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}

		public override void AI() => Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.tileCollide == true) {
				SoundEngine.PlaySound(SoundID.NPCDeath6, Projectile.Center);
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(6))
				target.AddBuff(BuffID.ShadowFlame, 85);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(190, 79, 252, 100);
		}
	}
}
