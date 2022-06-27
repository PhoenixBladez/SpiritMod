using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class ShadowBall_Friendly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Ball");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Wrath>(), (int)(Projectile.damage), 0, Main.myPlayer);

			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			{
				for (int num621 = 0; num621 < 40; num621++) {
					int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override void AI()
		{
			if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3) {
				Projectile.tileCollide = false;
				Projectile.ai[1] = 0f;
				Projectile.alpha = 255;
				Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
				Projectile.width = 14;
				Projectile.height = 14;
				Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
				Projectile.knockBack = 4f;
			}
		}

		public override bool PreAI()
		{
			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++) {
				if (Main.npc[index1].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
					float num25 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num23) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num24);
					if (num25 < 500f) {
						flag25 = true;
						jim = index1;
					}

				}
			}

			if (flag25) {
				float num1 = 10f;
				Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 10;
				Projectile.velocity.X = (Projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
				Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
			}

			if (Projectile.ai[1] >= 1f && Projectile.ai[1] < 20f) {
				Projectile.ai[1] += 1f;
				if (Projectile.ai[1] == 20f)
					Projectile.ai[1] = 1f;
			}

			Projectile.alpha -= 40;
			if (Projectile.alpha < 0) {
				Projectile.alpha = 0;
			}
			Projectile.spriteDirection = Projectile.direction;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 3) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 4)
					Projectile.frame = 0;
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] == 12f) {
				Projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -(float)Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, (double)((float)j * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (double)(Projectile.rotation - 1.57079637f), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 160, default, 1f);
					Main.dust[num8].scale = 1.1f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}

			if (Main.rand.Next(4) == 0) {
				for (int k = 0; k < 1; k++) {
					Vector2 value = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.19634954631328583), (double)Utils.ToRotation(Projectile.velocity), default);
					int num9 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, default, 1f);
					Main.dust[num9].velocity *= 0.1f;
					Main.dust[num9].position = Projectile.Center + value * (float)Projectile.width / 2f;
					Main.dust[num9].fadeIn = 0.9f;
				}
			}

			if (Main.rand.Next(32) == 0) {
				for (int l = 0; l < 1; l++) {
					Vector2 value2 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.39269909262657166), (double)Utils.ToRotation(Projectile.velocity), default);
					int num10 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 155, default, 0.8f);
					Main.dust[num10].velocity *= 0.3f;
					Main.dust[num10].position = Projectile.Center + value2 * (float)Projectile.width / 2f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num10].fadeIn = 1.4f;
					}
				}
			}

			if (Main.rand.Next(2) == 0) {
				for (int m = 0; m < 2; m++) {
					Vector2 value3 = -Utils.RotatedBy(Utils.RotatedByRandom(Vector2.UnitX, 0.78539818525314331), (double)Utils.ToRotation(Projectile.velocity), default);
					int num11 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 0, default, 1.2f);
					Main.dust[num11].velocity *= 0.3f;
					Main.dust[num11].noGravity = true;
					Main.dust[num11].position = Projectile.Center + value3 * (float)Projectile.width / 2f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num11].fadeIn = 1.4f;
					}
				}
			}
			return false;
		}

	}
}
