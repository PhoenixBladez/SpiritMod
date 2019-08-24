using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class TwilightArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Arrow");
		}

		public override void SetDefaults()
		{
			projectile.width = 9;
			projectile.height = 17;

			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;

			projectile.ranged = true;
			projectile.friendly = true;
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int d1 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					6, 0f, -2f, 0, default(Color), 2f);
				Main.dust[d1].scale *= 0.25f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(BuffID.ShadowFlame, 180, false);
				target.AddBuff(mod.BuffType("StackingFireBuff"), 180, false);
				target.AddBuff(mod.BuffType("HolyLight"), 180, false);
			}
		}

		public override void Kill(int timeLeft)
		{
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 50;
			projectile.height = 50;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			for (int num621 = 0; num621 < 20; num621++)
			{
				int num622 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					244, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
				}
			}

			for (int num623 = 0; num623 < 35; num623++)
			{
				int num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					244, 0f, 0f, 100, default(Color), 3f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
				num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					244, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num624].velocity *= 2f;
			}

			for (int num625 = 0; num625 < 3; num625++)
			{
				float scaleFactor10 = 0.33f;
				if (num625 == 1)
					scaleFactor10 = 0.66f;
				else if (num625 == 2)
					scaleFactor10 = 1f;

				int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Main.gore[num626].velocity.X += 1f;
				Main.gore[num626].velocity.Y += 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Main.gore[num626].velocity.X -= 1f;
				Main.gore[num626].velocity.Y += 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Main.gore[num626].velocity.X += 1f;
				Main.gore[num626].velocity.Y -= 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Main.gore[num626].velocity.X -= 1f;
				Main.gore[num626].velocity.Y -= 1f;
			}

			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 10;
			projectile.height = 10;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					DustID.Shadowflame, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
			}
		}

	}
}
