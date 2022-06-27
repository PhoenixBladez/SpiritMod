using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class RunicRune : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override bool PreAI()
		{
			float num = 1f - (float)Projectile.alpha / 255f;
			num *= Projectile.scale;
			Lighting.AddLight(Projectile.Center, 0.5f * num, 0.5f * num, 0.9f * num);
			Projectile.frameCounter++;
			if ((float)Projectile.frameCounter >= 8f) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}

			Projectile.localAI[0]++;
			if (Projectile.localAI[0] >= 90f) {
				Projectile.localAI[0] *= -1f;
			}
			if (Projectile.localAI[0] >= 0f) {
				Projectile.scale += 0.003f;
			}
			else {
				Projectile.scale -= 0.003f;
			}

			float num2 = 1f;
			float num3 = 1f;
			int identity = Projectile.identity % 6;
			if (identity == 0) {
				num3 *= -1f;
			}
			else if (identity == 1) {
				num2 *= -1f;
			}
			else if (identity == 2) {
				num3 *= -1f;
				num2 *= -1f;
			}
			else if (identity == 3) {
				num3 = 0f;
			}
			else if (identity == 4) {
				num2 = 0f;
			}

			Projectile.localAI[1]++;
			if (Projectile.localAI[1] > 60f) {
				Projectile.localAI[1] = -180f;
			}
			if (Projectile.localAI[1] >= -60f) {
				Projectile.velocity.X = Projectile.velocity.X + 0.002f * num3;
				Projectile.velocity.Y = Projectile.velocity.Y + 0.002f * num2;
			}
			else {
				Projectile.velocity.X = Projectile.velocity.X - 0.002f * num3;
				Projectile.velocity.Y = Projectile.velocity.Y - 0.002f * num2;
			}

			Projectile.ai[0]++;
			if (Projectile.ai[0] > 5400f) {
				Projectile.damage = 0;
				Projectile.ai[1] = 1f;
				if (Projectile.alpha < 255) {
					Projectile.alpha += 5;
					if (Projectile.alpha > 255)
						Projectile.alpha = 255;
				}
				else if (Projectile.owner == Main.myPlayer)
					Projectile.Kill();
			}
			else {
				float num4 = (Projectile.Center - Main.player[Projectile.owner].Center).Length() / 100f;
				if (num4 > 4f) {
					num4 *= 1.1f;
				}
				if (num4 > 5f) {
					num4 *= 1.2f;
				}
				if (num4 > 6f) {
					num4 *= 1.3f;
				}
				if (num4 > 7f) {
					num4 *= 1.4f;
				}
				if (num4 > 8f) {
					num4 *= 1.5f;
				}
				if (num4 > 9f) {
					num4 *= 1.6f;
				}
				if (num4 > 10f) {
					num4 *= 1.7f;
				}
				if (!Main.player[Projectile.owner].GetSpiritPlayer().runicSet) {
					num4 += 100f;
				}
				Projectile.ai[0] += num4;

				if (Projectile.alpha > 50) {
					Projectile.alpha -= 10;
					if (Projectile.alpha < 50) {
						Projectile.alpha = 50;
					}
				}
			}

			bool flag = false;
			Vector2 value = Vector2.Zero;
			float num5 = 280f;
			for (int i = 0; i < 200; i++) {
				if (Main.npc[i].CanBeChasedBy(this, false)) {
					float num6 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
					float num7 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
					float num8 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num6) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num7);
					if (num8 < num5) {
						num5 = num8;
						value = Main.npc[i].Center;
						flag = true;
					}
				}
			}
			if (flag) {
				Vector2 vector = value - Projectile.Center;
				vector.Normalize();
				vector *= 0.75f;
				Projectile.velocity = (Projectile.velocity * 10f + vector) / 11f;
				return false;
			}

			if (Projectile.velocity.Length() > 0.2f) {
				Projectile.velocity *= 0.98f;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.velocity *= 0f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 3;
		}

		public override void Kill(int timeLeft)
		{
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Dust dust = Main.dust[num];
						dust.position.X += (Main.rand.Next(-30, 31) / 20) - 1.5f;
						dust.position.Y += (Main.rand.Next(-30, 31) / 20) - 1.5f;
						if (dust.position != Projectile.Center)
							dust.velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				});
		}

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}
	}
}
