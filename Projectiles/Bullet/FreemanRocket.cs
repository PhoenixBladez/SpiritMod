using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SpiritMod.Projectiles.BaseProj;

namespace SpiritMod.Projectiles.Bullet
{
	public class FreemanRocket : BaseRocketProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Rocket");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;       //projectile width
			Projectile.height = 24;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Ranged;        // 
			Projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 1;      //how many npc will penetrate
			Projectile.timeLeft = 300;   //how many time projectile projectile has before disepire
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 3) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 4)
					Projectile.frame = 0;
			}
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && proj.type == ModContent.ProjectileType<FreemanRocket>() && proj.active) {
					Projectile.penetrate = -1;
					proj.penetrate = -1;
					proj.Kill();
					Projectile.Kill();
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] == 16f) {
				Projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			if (Main.myPlayer == Projectile.owner && Projectile.ai[0] <= 0f) {
				if (Main.player[Projectile.owner].channel) {
					float num2353 = 8f;
					Vector2 vector329 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num2352 = (float)Main.mouseX + Main.screenPosition.X - vector329.X;
					float num2351 = (float)Main.mouseY + Main.screenPosition.Y - vector329.Y;
					if (Main.player[Projectile.owner].gravDir == -1f) {
						num2351 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector329.Y;
					}
					float num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					num2350 = (float)Math.Sqrt((double)(num2352 * num2352 + num2351 * num2351));
					if (Projectile.ai[0] < 0f) {
						Projectile.ai[0] += 1f;
					}
					if (Projectile.type == 491 && num2350 < 100f) {
						if (Projectile.velocity.Length() < num2353) {
							Projectile.velocity *= 1.1f;
							if (Projectile.velocity.Length() > num2353) {
								Projectile.velocity.Normalize();
								Projectile.velocity *= num2353;
							}
						}
						if (Projectile.ai[0] == 0f) {
							Projectile.ai[0] = -10f;
						}
					}
					else if (num2350 > num2353) {
						num2350 = num2353 / num2350;
						num2352 *= num2350;
						num2351 *= num2350;
						int num2345 = (int)(num2352 * 1000f);
						int num2344 = (int)(Projectile.velocity.X * 1000f);
						int num2343 = (int)(num2351 * 1000f);
						int num2342 = (int)(Projectile.velocity.Y * 1000f);
						if (num2345 != num2344 || num2343 != num2342) {
							Projectile.netUpdate = true;
						}
						if (Projectile.type == 491) {
							Vector2 value167 = new Vector2(num2352, num2351);
							Projectile.velocity = (Projectile.velocity * 4f + value167) / 5f;
						}
						else {
							Projectile.velocity.X = num2352;
							Projectile.velocity.Y = num2351;
						}
					}
					else {
						int num2341 = (int)(num2352 * 1000f);
						int num2340 = (int)(Projectile.velocity.X * 1000f);
						int num2339 = (int)(num2351 * 1000f);
						int num2338 = (int)(Projectile.velocity.Y * 1000f);
						if (num2341 != num2340 || num2339 != num2338) {
							Projectile.netUpdate = true;
						}
						Projectile.velocity.X = num2352;
						Projectile.velocity.Y = num2351;
					}
				}
				else if (Projectile.ai[0] <= 0f) {
					Projectile.netUpdate = true;
					if (Projectile.type != 491) {
						float num2337 = 12f;
						Vector2 vector328 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
						float num2336 = (float)Main.mouseX + Main.screenPosition.X - vector328.X;
						float num2335 = (float)Main.mouseY + Main.screenPosition.Y - vector328.Y;
						if (Main.player[Projectile.owner].gravDir == -1f) {
							num2335 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector328.Y;
						}
						float num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
						if (num2334 == 0f || Projectile.ai[0] < 0f) {
							vector328 = new Vector2(Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2), Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2));
							num2336 = Projectile.position.X + (float)Projectile.width * 0.5f - vector328.X;
							num2335 = Projectile.position.Y + (float)Projectile.height * 0.5f - vector328.Y;
							num2334 = (float)Math.Sqrt((double)(num2336 * num2336 + num2335 * num2335));
						}
						num2334 = num2337 / num2334;
						num2336 *= num2334;
						num2335 *= num2334;
						Projectile.velocity.X = num2336;
						Projectile.velocity.Y = num2335;
						if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f) {
							Projectile.Kill();
						}
					}
					Projectile.ai[0] = 1f;
				}
			}
			int num1222 = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Torch, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num1222 * (float)k;
				Main.dust[index2].scale = .95f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
		}

		public override void AbstractHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.OnFire, 480);

		public override void ExplodeEffect()
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != Projectile.Center)
				{
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
	}
}
