using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class SoulSpirit : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Soul");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 30;
			Projectile.alpha = 255;
			Projectile.maxPenetrate = -1;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}

		public override bool PreAI()
		{
			Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0.5f, 0.5f, 0.9f);

			for (int i = 0; i < 10; i++) {
				float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
				float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.BlueCrystalShard, 0f, 0f, 0, default, 1f);
				Main.dust[num].alpha = Projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}

			float num2 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
			float num3 = Projectile.localAI[0];
			if (num3 == 0f) {
				Projectile.localAI[0] = num2;
				num3 = num2;
			}
			float num4 = Projectile.position.X;
			float num5 = Projectile.position.Y;
			float num6 = 300f;
			bool flag = false;
			int num7 = 0;
			if (Projectile.ai[1] == 0f) {
				for (int j = 0; j < 200; j++) {
					if (Main.npc[j].CanBeChasedBy(this, false) && (Projectile.ai[1] == 0f || Projectile.ai[1] == (float)(j + 1))) {
						float num8 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
						float num9 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
						float num10 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num8) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num9);
						if (num10 < num6 && Collision.CanHit(new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)(Projectile.height / 2)), 1, 1, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height)) {
							num6 = num10;
							num4 = num8;
							num5 = num9;
							flag = true;
							num7 = j;
						}
					}
				}
				if (flag) {
					Projectile.ai[1] = (float)(num7 + 1);
				}
				flag = false;
			}
			if (Projectile.ai[1] > 0f) {
				int num11 = (int)(Projectile.ai[1] - 1f);
				if (Main.npc[num11].active && Main.npc[num11].CanBeChasedBy(this, true) && !Main.npc[num11].dontTakeDamage) {
					float num12 = Main.npc[num11].position.X + (float)(Main.npc[num11].width / 2);
					float num13 = Main.npc[num11].position.Y + (float)(Main.npc[num11].height / 2);
					float num14 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num12) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num13);
					if (num14 < 1000f) {
						flag = true;
						num4 = Main.npc[num11].position.X + (float)(Main.npc[num11].width / 2);
						num5 = Main.npc[num11].position.Y + (float)(Main.npc[num11].height / 2);
					}
				}
				else {
					Projectile.ai[1] = 0f;
				}
			}
			if (!Projectile.friendly)
				flag = false;

			if (flag) {
				float num15 = num3;
				Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num16 = num4 - vector.X;
				float num17 = num5 - vector.Y;
				float num18 = (float)Math.Sqrt((double)(num16 * num16 + num17 * num17));
				num18 = num15 / num18;
				num16 *= num18;
				num17 *= num18;
				int num19 = 8;
				Projectile.velocity.X = (Projectile.velocity.X * (float)(num19 - 1) + num16) / (float)num19;
				Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num19 - 1) + num17) / (float)num19;
			}
			Projectile.rotation = 0f;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					for (int i = 0; i < 40; i++) {
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, -2f, 0, default, 2f);
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

		public override bool PreDraw(ref Color lightColor)
		{
			ProjectileExtras.DrawAroundOrigin(Projectile.whoAmI, lightColor);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<SoulBurn>(), 180, true);
		}

	}
}
