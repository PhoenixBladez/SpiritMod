using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class VoidStar : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Star");
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override bool PreAI()
		{
			float num = 1f - (float)Projectile.alpha / 255f;
			num *= Projectile.scale;
			Lighting.AddLight(Projectile.Center, 0.9f * num, 0.3f * num, 0.9f * num);
			Projectile.frameCounter++;
			if ((float)Projectile.frameCounter >= 8f) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(6))
				target.AddBuff(BuffID.ShadowFlame, 120, true);
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			for (int i = 0; i < 10; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}

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
			else {
				Projectile.velocity *= 0.95f;
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Dust dust = Main.dust[num];
						dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
						if (Main.dust[num].position != Projectile.Center) {
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
				});
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
	}
}
