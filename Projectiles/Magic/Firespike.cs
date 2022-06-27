using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Firespike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Spike");
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.alpha = 255;

			Projectile.hostile = false;
			Projectile.friendly = true;

			Projectile.penetrate = 4;
		}

		public override bool PreAI()
		{
			int side = Math.Sign(Projectile.velocity.Y);
			int bitSide = (side != -1) ? 1 : 0;

			if (Projectile.ai[0] == 0f)
			{
				if (!Collision.SolidCollision(Projectile.position + new Vector2(0f, (side == -1) ? (Projectile.height - 48) : 0), Projectile.width, 48) && !Collision.WetCollision(Projectile.position + new Vector2(0f, ((side == -1) ? (Projectile.height - 20) : 0)), Projectile.width, 20))
				{
					Projectile.velocity = new Vector2(0f, Math.Sign(Projectile.velocity.Y) * 0.001f);
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.timeLeft = 60;
				}

				Projectile.ai[1] += 1f;

				if (Projectile.ai[1] >= 60f)
					Projectile.Kill();

				for (int num1200 = 0; num1200 < 3; num1200++)
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f)];
					dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
					dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
					dust.noGravity = true;
					dust.position = Projectile.Center + new Vector2(0f, -Projectile.height / 2f).RotatedBy(Projectile.rotation, default) * 1.1f;
				}
			}

			if (Projectile.ai[0] == 1f)
			{
				Projectile.velocity = new Vector2(0f, Math.Sign(Projectile.velocity.Y) * 0.001f);
				if (side != 0)
				{
					int num1198 = 16;

					for (; num1198 < 320 && !Collision.SolidCollision(Projectile.position + new Vector2(0f, (side == -1) ? (Projectile.height - num1198 - 16) : 0), Projectile.width, num1198 + 16); num1198 += 16)
					{
					}

					if (side == -1)
						Projectile.position.Y = (Projectile.position.Y + Projectile.height) - (Projectile.height = num1198);
					else
						Projectile.height = num1198;
				}

				Projectile.ai[1] += 1f;

				if (Projectile.ai[1] >= 60f)
					Projectile.Kill();

				if (Projectile.localAI[0] == 0f)
				{
					Projectile.localAI[0] = 1f;
					for (int i = 0; i < 60; ++i)
						SpawnDust(side);
					SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
				}

				if (side == 1)
					for (int i = 0; i < 9; ++i)
						SpawnDust(side);

				int offsetY = (int)(Projectile.ai[1] / 60f * Projectile.height) * 3;
				if (offsetY > Projectile.height)
					offsetY = Projectile.height;

				Vector2 position9 = Projectile.position + ((side == -1) ? new Vector2(0f, Projectile.height - offsetY) : Vector2.Zero);
				Vector2 vector154 = Projectile.position + ((side == -1) ? new Vector2(0f, Projectile.height) : Vector2.Zero);

				for (int i = 0; i < 6; ++i)
				{
					if (Main.rand.Next(3) < 2)
					{
						Dust dust81 = Main.dust[Dust.NewDust(position9, Projectile.width, offsetY, DustID.Torch, 0f, 0f, 90, default, 2.5f)];
						dust81.noGravity = true;
						dust81.fadeIn = 1f;
						if (dust81.velocity.Y > 0f)
							dust81.velocity.Y *= -1f;

						if (Main.rand.NextBool(2))
						{
							dust81.position.Y = MathHelper.Lerp(dust81.position.Y, vector154.Y, 0.5f);
							dust81.velocity *= 5f;
							dust81.velocity.Y -= 3f;
							dust81.position.X = Projectile.Center.X;
							dust81.noGravity = false;
							dust81.noLight = true;
							dust81.fadeIn = 0.4f;
							dust81.scale *= 0.3f;
						}
						else
							dust81.velocity = Projectile.DirectionFrom(dust81.position) * dust81.velocity.Length() * 0.25f;

						dust81.velocity.Y *= (0f - side);
						dust81.customData = bitSide;
					}
				}

				for (int i = 0; i < 6; ++i)
				{
					if (Main.rand.NextFloat() >= 0.5f)
					{
						Dust dust = Main.dust[Dust.NewDust(position9, Projectile.width, offsetY, Utils.SelectRandom(Main.rand, 6, 259, 127), 0f, -2.5f * (0f - side), 0, default, .8f)];
						dust.alpha = 200;
						dust.velocity *= new Vector2(0.6f, 1.5f);
						dust.scale += Main.rand.NextFloat();
						dust.customData = bitSide;

						if (side == -1 && Main.rand.Next(4) != 0)
							dust.velocity.Y -= 0.2f;
					}
				}
			}
			return false;
		}

		private void SpawnDust(int side)
		{
			float speedY = -2.5f * (0f - side);
			Dust dust81 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Utils.SelectRandom(Main.rand, 6, 259, 158), 0f, speedY, 0, default, 1f)];
			dust81.alpha = 200;
			dust81.velocity *= new Vector2(0.3f, 2f);
			dust81.velocity.Y += 2 * side;
			dust81.scale += Main.rand.NextFloat();
			dust81.position = new Vector2(Projectile.Center.X, Projectile.Center.Y + Projectile.height * 0.5f * (0f - side));

			if (side == -1 && Main.rand.Next(4) != 0)
				dust81.velocity.Y -= 0.2f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 120, true);
		}
	}
}