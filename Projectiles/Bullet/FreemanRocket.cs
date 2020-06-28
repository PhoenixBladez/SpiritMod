using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class FreemanRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Rocket");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;       //projectile width
			projectile.height = 24;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.ranged = true;        // 
			projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.localAI[0] += 1f;
			if(projectile.localAI[0] == 16f) {
				projectile.localAI[0] = 0f;
				for(int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, 226, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			if(Main.myPlayer == projectile.owner && projectile.ai[0] <= 0f) {
				if(Main.player[projectile.owner].channel) {
					float num2353 = 8f;
					Vector2 vector329 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
					float num2352 = (float)Main.mouseX + Main.screenPosition.X - vector329.X;
					float num2351 = (float)Main.mouseY + Main.screenPosition.Y - vector329.Y;
					if(Main.player[projectile.owner].gravDir == -1f) {
						num2351 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector329.Y;
					}
					float num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					if(projectile.ai[0] < 0f) {
						projectile.ai[0] += 1f;
					}
					if(projectile.type == 491 && num2350 < 100f) {
						if(projectile.velocity.Length() < num2353) {
							projectile.velocity *= 1.1f;
							if(projectile.velocity.Length() > num2353) {
								projectile.velocity.Normalize();
								projectile.velocity *= num2353;
							}
						}
						if(projectile.ai[0] == 0f) {
							projectile.ai[0] = -10f;
						}
					} else if(num2350 > num2353) {
						num2350 = num2353 / num2350;
						num2352 *= num2350;
						num2351 *= num2350;
						int num2345 = (int)(num2352 * 1000f);
						int num2344 = (int)(projectile.velocity.X * 1000f);
						int num2343 = (int)(num2351 * 1000f);
						int num2342 = (int)(projectile.velocity.Y * 1000f);
						if(num2345 != num2344 || num2343 != num2342) {
							projectile.netUpdate = true;
						}
						if(projectile.type == 491) {
							Vector2 value167 = new Vector2(num2352, num2351);
							projectile.velocity = (projectile.velocity * 4f + value167) / 5f;
						} else {
							projectile.velocity.X = num2352;
							projectile.velocity.Y = num2351;
						}
					} else {
						int num2341 = (int)(num2352 * 1000f);
						int num2340 = (int)(projectile.velocity.X * 1000f);
						int num2339 = (int)(num2351 * 1000f);
						int num2338 = (int)(projectile.velocity.Y * 1000f);
						if(num2341 != num2340 || num2339 != num2338) {
							projectile.netUpdate = true;
						}
						projectile.velocity.X = num2352;
						projectile.velocity.Y = num2351;
					}
				} else if(projectile.ai[0] <= 0f) {
					projectile.netUpdate = true;
					if(projectile.type != 491) {
						float num2337 = 12f;
						Vector2 vector328 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
						float num2336 = (float)Main.mouseX + Main.screenPosition.X - vector328.X;
						float num2335 = (float)Main.mouseY + Main.screenPosition.Y - vector328.Y;
						if(Main.player[projectile.owner].gravDir == -1f) {
							num2335 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector328.Y;
						}
						float num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
						if(num2334 == 0f || projectile.ai[0] < 0f) {
							vector328 = new Vector2(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2), Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2));
							num2336 = projectile.position.X + (float)projectile.width * 0.5f - vector328.X;
							num2335 = projectile.position.Y + (float)projectile.height * 0.5f - vector328.Y;
							num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
						}
						num2334 = num2337 / num2334;
						num2336 *= num2334;
						num2335 *= num2334;
						projectile.velocity.X = num2336;
						projectile.velocity.Y = num2335;
						if(projectile.velocity.X == 0f && projectile.velocity.Y == 0f) {
							projectile.Kill();
						}
					}
					projectile.ai[0] = 1f;
				}
			}
			int num1222 = 5;
			for(int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 6, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num1222 * (float)k;
				Main.dust[index2].scale = .95f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 480);
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			{
				for(int i = 0; i < 40; i++) {
					int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if(Main.dust[num].position != projectile.Center) {
						Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
		}
	}
}
