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
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.alpha = 255;

			projectile.hostile = false;
			projectile.friendly = true;

			projectile.penetrate = 4;
		}

		public override bool PreAI()
		{
			int side = Math.Sign(projectile.velocity.Y);
			int bitSide = (side != -1) ? 1 : 0;

			if (projectile.ai[0] == 0f)
			{
				if (!Collision.SolidCollision(projectile.position + new Vector2(0f, (side == -1) ? (projectile.height - 48) : 0), projectile.width, 48) && !Collision.WetCollision(projectile.position + new Vector2(0f, ((side == -1) ? (projectile.height - 20) : 0)), projectile.width, 20))
				{
					projectile.velocity = new Vector2(0f, Math.Sign(projectile.velocity.Y) * 0.001f);
					projectile.ai[0] = 1f;
					projectile.ai[1] = 0f;
					projectile.timeLeft = 60;
				}

				projectile.ai[1] += 1f;

				if (projectile.ai[1] >= 60f)
					projectile.Kill();

				for (int num1200 = 0; num1200 < 3; num1200++)
				{
					Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1f)];
					dust.scale = 0.1f + Main.rand.Next(5) * 0.1f;
					dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
					dust.noGravity = true;
					dust.position = projectile.Center + new Vector2(0f, -projectile.height / 2f).RotatedBy(projectile.rotation, default) * 1.1f;
				}
			}

			if (projectile.ai[0] == 1f)
			{
				projectile.velocity = new Vector2(0f, Math.Sign(projectile.velocity.Y) * 0.001f);
				if (side != 0)
				{
					int num1198 = 16;

					for (; num1198 < 320 && !Collision.SolidCollision(projectile.position + new Vector2(0f, (side == -1) ? (projectile.height - num1198 - 16) : 0), projectile.width, num1198 + 16); num1198 += 16)
					{
					}

					if (side == -1)
						projectile.position.Y = (projectile.position.Y + projectile.height) - (projectile.height = num1198);
					else
						projectile.height = num1198;
				}

				projectile.ai[1] += 1f;

				if (projectile.ai[1] >= 60f)
					projectile.Kill();

				if (projectile.localAI[0] == 0f)
				{
					projectile.localAI[0] = 1f;
					for (int i = 0; i < 60; ++i)
						SpawnDust(side);
					Main.PlaySound(SoundID.Item34, projectile.position);
				}

				if (side == 1)
					for (int i = 0; i < 9; ++i)
						SpawnDust(side);

				int offsetY = (int)(projectile.ai[1] / 60f * projectile.height) * 3;
				if (offsetY > projectile.height)
					offsetY = projectile.height;

				Vector2 position9 = projectile.position + ((side == -1) ? new Vector2(0f, projectile.height - offsetY) : Vector2.Zero);
				Vector2 vector154 = projectile.position + ((side == -1) ? new Vector2(0f, projectile.height) : Vector2.Zero);

				for (int i = 0; i < 6; ++i)
				{
					if (Main.rand.Next(3) < 2)
					{
						Dust dust81 = Main.dust[Dust.NewDust(position9, projectile.width, offsetY, DustID.Fire, 0f, 0f, 90, default, 2.5f)];
						dust81.noGravity = true;
						dust81.fadeIn = 1f;
						if (dust81.velocity.Y > 0f)
							dust81.velocity.Y *= -1f;

						if (Main.rand.NextBool(2))
						{
							dust81.position.Y = MathHelper.Lerp(dust81.position.Y, vector154.Y, 0.5f);
							dust81.velocity *= 5f;
							dust81.velocity.Y -= 3f;
							dust81.position.X = projectile.Center.X;
							dust81.noGravity = false;
							dust81.noLight = true;
							dust81.fadeIn = 0.4f;
							dust81.scale *= 0.3f;
						}
						else
							dust81.velocity = projectile.DirectionFrom(dust81.position) * dust81.velocity.Length() * 0.25f;

						dust81.velocity.Y *= (0f - side);
						dust81.customData = bitSide;
					}
				}

				for (int i = 0; i < 6; ++i)
				{
					if (Main.rand.NextFloat() >= 0.5f)
					{
						Dust dust = Main.dust[Dust.NewDust(position9, projectile.width, offsetY, Utils.SelectRandom(Main.rand, 6, 259, 127), 0f, -2.5f * (0f - side), 0, default, .8f)];
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
			Dust dust81 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, Utils.SelectRandom(Main.rand, 6, 259, 158), 0f, speedY, 0, default, 1f)];
			dust81.alpha = 200;
			dust81.velocity *= new Vector2(0.3f, 2f);
			dust81.velocity.Y += 2 * side;
			dust81.scale += Main.rand.NextFloat();
			dust81.position = new Vector2(projectile.Center.X, projectile.Center.Y + projectile.height * 0.5f * (0f - side));

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