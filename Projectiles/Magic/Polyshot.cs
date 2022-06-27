using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Polyshot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polyshot");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 32;
			Projectile.light = .25f;
			Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.damage = 35;
		}

		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.3f;
			Projectile.scale = num395 + 0.95f;
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(89, 255, 161, 100);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life <= target.lifeMax / 2 && !target.boss) {
				target.life = 0;
				NPC.NewNPC((int)target.position.X, (int)target.position.Y, NPCID.Bunny);
			}
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(target.position, target.width, target.height, DustID.TerraBlade, 0f, -2f, 0, Color.White, .9f);
				Main.dust[num].noLight = true;
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-10, 11) / 20) - 1.5f);
				if (Main.dust[num].position != target.Center) {
					Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 4f;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, -2f, 0, Color.White, .9f);
				Main.dust[num].noLight = true;
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X += (Main.rand.Next(-10, 11) / 20) - 1.5f;
				dust.position.Y += (Main.rand.Next(-10, 11) / 20) - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
				}
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

	}
}
