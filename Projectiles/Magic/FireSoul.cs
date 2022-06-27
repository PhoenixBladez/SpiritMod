﻿using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class FireSoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Soul");
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;       //projectile width
			Projectile.height = 46;  //projectile height
			Projectile.friendly = true;      //make that the projectile will not damage you
			Projectile.DamageType = DamageClass.Magic;         // 
			Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			Projectile.penetrate = 1;      //how many npc will penetrate
			Projectile.timeLeft = 300;   //how many time projectile projectile has before disepire // projectile light
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57F;

			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++) {
				if (Main.npc[index1].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1)) {
					float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
					float num25 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num23) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num24);
					if (num25 < 300f) {
						flag25 = true;
						jim = index1;
					}

				}
			}

			if (flag25) {
				float num1 = 4f;
				Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 30;
				Projectile.velocity.X = (Projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
				Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
			}

			for (int index1 = 0; index1 < 5; ++index1) {
				float num1 = Projectile.velocity.X * 0.2f * (float)index1;
				float num2 = (float)-((double)Projectile.velocity.Y * 0.2) * (float)index1;
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 100, default, 1.3f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].position.X -= num1;
				Main.dust[index2].position.Y -= num2;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 180);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			ProjectileExtras.Explode(Projectile.whoAmI, 120, 120,
				delegate {
					SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
					Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
					Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
					Projectile.width = 50;
					Projectile.height = 50;
					Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
					Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

					for (int num621 = 0; num621 < 20; num621++) {
						int num622 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
							DustID.CopperCoin, 0f, 0f, 100, default, 2f);
						Main.dust[num622].velocity *= 3f;
						if (Main.rand.Next(2) == 0) {
							Main.dust[num622].scale = 0.5f;
							Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						}
					}

					for (int num623 = 0; num623 < 35; num623++) {
						int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 3f);
						Main.dust[num624].noGravity = true;
						Main.dust[num624].velocity *= 5f;
						num624 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
							DustID.CopperCoin, 0f, 0f, 100, default, 2f);
						Main.dust[num624].velocity *= 2f;
					}

					for (int num625 = 0; num625 < 3; num625++) {
						float scaleFactor10 = 0.33f;
						if (num625 == 1)
							scaleFactor10 = 0.66f;
						else if (num625 == 2)
							scaleFactor10 = 1f;

						int num626 = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
						Main.gore[num626].velocity *= scaleFactor10;
						Gore expr_13AB6_cp_0 = Main.gore[num626];
						expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
						Gore expr_13AD6_cp_0 = Main.gore[num626];
						expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
						num626 = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
						Main.gore[num626].velocity *= scaleFactor10;
						Gore expr_13B79_cp_0 = Main.gore[num626];
						expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
						Gore expr_13B99_cp_0 = Main.gore[num626];
						expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
						num626 = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
						Main.gore[num626].velocity *= scaleFactor10;
						Gore expr_13C3C_cp_0 = Main.gore[num626];
						expr_13C3C_cp_0.velocity.X = expr_13C3C_cp_0.velocity.X + 1f;
						Gore expr_13C5C_cp_0 = Main.gore[num626];
						expr_13C5C_cp_0.velocity.Y = expr_13C5C_cp_0.velocity.Y - 1f;
						num626 = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default, Main.rand.Next(61, 64), 1f);
						Main.gore[num626].velocity *= scaleFactor10;
						Gore expr_13CFF_cp_0 = Main.gore[num626];
						expr_13CFF_cp_0.velocity.X = expr_13CFF_cp_0.velocity.X - 1f;
						Gore expr_13D1F_cp_0 = Main.gore[num626];
						expr_13D1F_cp_0.velocity.Y = expr_13D1F_cp_0.velocity.Y - 1f;
					}

					Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
					Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
					Projectile.width = 10;
					Projectile.height = 10;
					Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
					Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
				});
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

	}
}
