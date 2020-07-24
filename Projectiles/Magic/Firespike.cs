using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Firespike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Spike");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.alpha = 255;

			projectile.hostile = false;
			projectile.friendly = true;

			projectile.penetrate = 4;

			Main.projFrames[projectile.type] = 1;
		}
		int num2475;
		Vector2 vector333;
		Dust dust81;
		public override bool PreAI()
		{
			int num1202 = Math.Sign(projectile.velocity.Y);
			int num1201 = (num1202 != -1) ? 1 : 0;
			if (projectile.ai[0] == 0f) {
				if (!Collision.SolidCollision(projectile.position + new Vector2(0f, (float)((num1202 == -1) ? (projectile.height - 48) : 0)), projectile.width, 48) && !Collision.WetCollision(projectile.position + new Vector2(0f, (float)((num1202 == -1) ? (projectile.height - 20) : 0)), projectile.width, 20)) {
					projectile.velocity = new Vector2(0f, (float)Math.Sign(projectile.velocity.Y) * 0.001f);
					projectile.ai[0] = 1f;
					projectile.ai[1] = 0f;
					projectile.timeLeft = 60;
				}
				projectile.ai[1] += 1f;
				if (projectile.ai[1] >= 60f) {
					projectile.Kill();
				}
				for (int num1200 = 0; num1200 < 3; num1200 = num2475 + 1) {
					Vector2 position216 = projectile.position;
					int width155 = projectile.width;
					int height155 = projectile.height;
					Color newColor5 = default(Color);
					int num1199 = Dust.NewDust(position216, width155, height155, 31, 0f, 0f, 100, newColor5, 1f);
					Main.dust[num1199].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num1199].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num1199].noGravity = true;
					Dust obj11 = Main.dust[num1199];
					Vector2 center51 = projectile.Center;
					Vector2 spinningpoint65 = new Vector2(0f, (0f - (float)projectile.height) / 2f);
					double radians61 = (double)projectile.rotation;
					vector333 = default(Vector2);
					obj11.position = center51 + spinningpoint65.RotatedBy(radians61, vector333) * 1.1f;
					num2475 = num1200;
				}
			}
			if (projectile.ai[0] == 1f) {
				projectile.velocity = new Vector2(0f, (float)Math.Sign(projectile.velocity.Y) * 0.001f);
				if (num1202 != 0) {
					int num1198 = 16;
					int num1197 = 320;
					if (projectile.type == 670) {
						num1197 -= (int)Math.Abs(projectile.localAI[1]) * 64;
					}
					for (; num1198 < num1197 && !Collision.SolidCollision(projectile.position + new Vector2(0f, (float)((num1202 == -1) ? (projectile.height - num1198 - 16) : 0)), projectile.width, num1198 + 16); num1198 += 16) {
					}
					if (num1202 == -1) {
						projectile.position.Y = projectile.position.Y + (float)projectile.height;
						projectile.height = num1198;
						projectile.position.Y = projectile.position.Y - (float)num1198;
					}
					else {
						projectile.height = num1198;
					}
				}
				projectile.ai[1] += 1f;
				if (projectile.type == 670 && projectile.owner == Main.myPlayer && projectile.ai[1] == 12f && projectile.localAI[1] < 3f && projectile.localAI[1] > -3f) {
					if (projectile.localAI[1] == 0f) {
						int num1196 = Projectile.NewProjectile(projectile.Bottom + new Vector2(-50f, -10f), -Vector2.UnitY, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
						Main.projectile[num1196].localAI[1] = projectile.localAI[1] - 1f;
						num1196 = Projectile.NewProjectile(projectile.Bottom + new Vector2(50f, -10f), -Vector2.UnitY, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
						Main.projectile[num1196].localAI[1] = projectile.localAI[1] + 1f;
					}
					else {
						int num1194 = Math.Sign(projectile.localAI[1]);
						int num1193 = Projectile.NewProjectile(projectile.Bottom + new Vector2((float)(50 * num1194), -10f), -Vector2.UnitY, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
						Main.projectile[num1193].localAI[1] = projectile.localAI[1] + (float)num1194;
					}
				}
				if (projectile.ai[1] >= 60f) {
					projectile.Kill();
				}
				if (projectile.localAI[0] == 0f) {
					projectile.localAI[0] = 1f;
					int num1192 = 0;
					while ((float)num1192 < 60f) {
						int num1191 = Utils.SelectRandom(Main.rand, 6, 259, 158);
						Vector2 position217 = projectile.position;
						int width156 = projectile.width;
						int height156 = projectile.height;
						int num2507 = num1191;
						float speedY34 = -2.5f * (0f - (float)num1202);
						Color newColor5 = default(Color);
						int num1190 = Dust.NewDust(position217, width156, height156, num2507, 0f, speedY34, 0, newColor5, 1f);
						Main.dust[num1190].alpha = 200;
						dust81 = Main.dust[num1190];
						dust81.velocity *= new Vector2(0.3f, 2f);
						Dust expr_2FFB5_cp_0 = Main.dust[num1190];
						expr_2FFB5_cp_0.velocity.Y = expr_2FFB5_cp_0.velocity.Y + (float)(2 * num1202);
						dust81 = Main.dust[num1190];
						dust81.scale += Main.rand.NextFloat();
						Main.dust[num1190].position = new Vector2(projectile.Center.X, projectile.Center.Y + (float)projectile.height * 0.5f * (0f - (float)num1202));
						Main.dust[num1190].customData = num1201;
						if (num1202 == -1 && Main.rand.Next(4) != 0) {
							Dust expr_30062_cp_0 = Main.dust[num1190];
							expr_30062_cp_0.velocity.Y = expr_30062_cp_0.velocity.Y - 0.2f;
						}
						num2475 = num1192;
						num1192 = num2475 + 1;
					}
					Main.PlaySound(SoundID.Item34, projectile.position);
				}
				if (num1202 == 1) {
					int num1189 = 0;
					while ((float)num1189 < 9f) {
						int num1188 = Utils.SelectRandom(Main.rand, 6, 259, 158);
						Vector2 position218 = projectile.position;
						int width157 = projectile.width;
						int height157 = projectile.height;
						int num2508 = num1188;
						float speedY35 = -2.5f * (0f - (float)num1202);
						Color newColor5 = default(Color);
						int num1187 = Dust.NewDust(position218, width157, height157, num2508, 0f, speedY35, 0, newColor5, .6f);
						Main.dust[num1187].alpha = 200;
						dust81 = Main.dust[num1187];
						dust81.velocity *= new Vector2(0.3f, 2f);
						Dust expr_3017A_cp_0 = Main.dust[num1187];
						expr_3017A_cp_0.velocity.Y = expr_3017A_cp_0.velocity.Y + (float)(2 * num1202);
						dust81 = Main.dust[num1187];
						dust81.scale += Main.rand.NextFloat();
						Main.dust[num1187].position = new Vector2(projectile.Center.X, projectile.Center.Y + (float)projectile.height * 0.5f * (0f - (float)num1202));
						Main.dust[num1187].customData = num1201;
						if (num1202 == -1 && Main.rand.Next(4) != 0) {
							Dust expr_30227_cp_0 = Main.dust[num1187];
							expr_30227_cp_0.velocity.Y = expr_30227_cp_0.velocity.Y - 0.2f;
						}
						num2475 = num1189;
						num1189 = num2475 + 1;
					}
				}
				int num1186 = (int)(projectile.ai[1] / 60f * (float)projectile.height) * 3;
				if (num1186 > projectile.height) {
					num1186 = projectile.height;
				}
				Vector2 position9 = projectile.position + ((num1202 == -1) ? new Vector2(0f, (float)(projectile.height - num1186)) : Vector2.Zero);
				Vector2 vector154 = projectile.position + ((num1202 == -1) ? new Vector2(0f, (float)projectile.height) : Vector2.Zero);
				int num1185 = 0;
				while ((float)num1185 < 6f) {
					if (Main.rand.Next(3) < 2) {
						Vector2 position219 = position9;
						int width158 = projectile.width;
						int height158 = num1186;
						Color newColor5 = default(Color);
						int num1179 = Dust.NewDust(position219, width158, height158, 6, 0f, 0f, 90, newColor5, 2.5f);
						Main.dust[num1179].noGravity = true;
						Main.dust[num1179].fadeIn = 1f;
						if (Main.dust[num1179].velocity.Y > 0f) {
							Dust expr_30385_cp_0 = Main.dust[num1179];
							expr_30385_cp_0.velocity.Y = expr_30385_cp_0.velocity.Y * -1f;
						}
						if (Main.rand.Next(6) < 3) {
							Main.dust[num1179].position.Y = MathHelper.Lerp(Main.dust[num1179].position.Y, vector154.Y, 0.5f);
							dust81 = Main.dust[num1179];
							dust81.velocity *= 5f;
							Dust expr_30410_cp_0 = Main.dust[num1179];
							expr_30410_cp_0.velocity.Y = expr_30410_cp_0.velocity.Y - 3f;
							Main.dust[num1179].position.X = projectile.Center.X;
							Main.dust[num1179].noGravity = false;
							Main.dust[num1179].noLight = true;
							Main.dust[num1179].fadeIn = 0.4f;
							dust81 = Main.dust[num1179];
							dust81.scale *= 0.3f;
						}
						else {
							Main.dust[num1179].velocity = projectile.DirectionFrom(Main.dust[num1179].position) * Main.dust[num1179].velocity.Length() * 0.25f;
						}
						Dust expr_304E7_cp_0 = Main.dust[num1179];
						expr_304E7_cp_0.velocity.Y = expr_304E7_cp_0.velocity.Y * (0f - (float)num1202);
						Main.dust[num1179].customData = num1201;
					}
					num2475 = num1185;
					num1185 = num2475 + 1;
				}
				int num1184 = 0;
				while ((float)num1184 < 6f) {
					if (Main.rand.NextFloat() >= 0.5f) {
						int num1183 = Utils.SelectRandom(Main.rand, 6, 259, 127);
						Vector2 position220 = position9;
						int width159 = projectile.width;
						int height159 = num1186;
						int num2509 = num1183;
						float speedY36 = -2.5f * (0f - (float)num1202);
						Color newColor5 = default(Color);
						int num1182 = Dust.NewDust(position220, width159, height159, num2509, 0f, speedY36, 0, newColor5, .8f);
						Main.dust[num1182].alpha = 200;
						dust81 = Main.dust[num1182];
						dust81.velocity *= new Vector2(0.6f, 1.5f);
						dust81 = Main.dust[num1182];
						dust81.scale += Main.rand.NextFloat();
						if (num1202 == -1 && Main.rand.Next(4) != 0) {
							Dust expr_30641_cp_0 = Main.dust[num1182];
							expr_30641_cp_0.velocity.Y = expr_30641_cp_0.velocity.Y - 0.2f;
						}
						Main.dust[num1182].customData = num1201;
					}
					num2475 = num1184;
					num1184 = num2475 + 1;
				}
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 120, true);
		}

	}
}
