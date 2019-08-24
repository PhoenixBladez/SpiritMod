using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
	public class ExplodingFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Exploding Feather");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 34;

			projectile.hostile = true;
			projectile.friendly = false;

			projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			projectile.spriteDirection = projectile.direction;

			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;

			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 65, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 4);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 4);
			projectile.width = 20;
			projectile.height = 20;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 4);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 4);

			for (int num625 = 0; num625 < 3; num625++)
			{
				float scaleFactor10 = 0.2f;
				if (num625 == 1)
				{
					scaleFactor10 = 0.5f;
				}
				if (num625 == 2)
				{
					scaleFactor10 = 1f;
				}
				int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13AB6_cp_0 = Main.gore[num626];
				expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
				Gore expr_13AD6_cp_0 = Main.gore[num626];
				expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13B79_cp_0 = Main.gore[num626];
				expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
				Gore expr_13B99_cp_0 = Main.gore[num626];
				expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
			}

			projectile.position.X = projectile.position.X + (float)(projectile.width / 4);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 4);
			projectile.width = 10;
			projectile.height = 10;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 4);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 4);

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
			for (int num273 = 0; num273 < 3; num273++)
			{
				int num274 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 187, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num274].noGravity = true;
				Main.dust[num274].noLight = true;
				Main.dust[num274].scale = 0.7f;

				int num278 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 65, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num278].noGravity = true;
				Main.dust[num278].noLight = true;
				Main.dust[num278].scale = 0.7f;
			}
		}
	}
}
