using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class ShroomSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override bool PreAI()
		{
			float num = 1f - (float)projectile.alpha / 255f;
			num *= projectile.scale;
			Lighting.AddLight(projectile.Center, 0.5f * num, 0.5f * num, 0.9f * num);

			projectile.localAI[0]++;
			if (projectile.localAI[0] >= 90f) {
				projectile.localAI[0] *= -1f;
			}
			if (projectile.localAI[0] >= 0f) {
				projectile.scale += 0.003f;
			}
			else {
				projectile.scale -= 0.003f;
			}

			float num2 = 1f;
			float num3 = 1f;
			int identity = projectile.identity % 6;
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

			projectile.localAI[1]++;
			if (projectile.localAI[1] > 60f) {
				projectile.localAI[1] = -180f;
			}
			if (projectile.localAI[1] >= -60f) {
				projectile.velocity.X = projectile.velocity.X + 0.002f * num3;
				projectile.velocity.Y = projectile.velocity.Y + 0.002f * num2;
			}
			else {
				projectile.velocity.X = projectile.velocity.X - 0.002f * num3;
				projectile.velocity.Y = projectile.velocity.Y - 0.002f * num2;
			}

			projectile.ai[0]++;
			if (projectile.ai[0] > 5400f) {
				projectile.ai[1] = 1f;
				if (projectile.alpha < 255) {
					projectile.alpha += 5;
					if (projectile.alpha > 255)
						projectile.alpha = 255;
				}
				else if (projectile.owner == Main.myPlayer)
					projectile.Kill();
			}
			else {
				float num4 = (projectile.Center - Main.player[projectile.owner].Center).Length() / 100f;
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
				if (!Main.player[projectile.owner].GetSpiritPlayer().runicSet) {
					num4 += 100f;
				}
				projectile.ai[0] += num4;

				if (projectile.alpha > 50) {
					projectile.alpha -= 10;
					if (projectile.alpha < 50) {
						projectile.alpha = 50;
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
					float num8 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num6) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num7);
					if (num8 < num5) {
						num5 = num8;
						value = Main.npc[i].Center;
						flag = true;
					}
				}
			}
			if (flag) {
				Vector2 vector = value - projectile.Center;
				vector.Normalize();
				vector *= 0.75f;
				projectile.velocity = (projectile.velocity * 11f + vector) / 11f;
				return false;
			}

			if (projectile.velocity.Length() > 0.2f) {
				projectile.velocity *= 0.98f;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.velocity *= 0f;
			projectile.alpha = 255;
			projectile.timeLeft = 3;
		}

		public override void Kill(int timeLeft)
		{
			{
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Harpy, 0f, -2f, 0, default(Color), .6f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-20, 21) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-20, 21) / 20) - 1.5f);
					if (Main.dust[num].position != projectile.Center) {
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 4f;
					}
				}
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
