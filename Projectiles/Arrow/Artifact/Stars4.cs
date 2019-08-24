using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow.Artifact
{
	class Stars4 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Burning Star");
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.ranged = true;
			projectile.timeLeft = 500;
			projectile.height = 30;
			projectile.width = 10;
			projectile.scale = 1.15f;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-80, 80),
				projectile.Center.Y - 1000 + Main.rand.Next(-50, 50),
				0, Main.rand.Next(18, 28),
				mod.ProjectileType("Stars3"), projectile.damage / 4 * 3, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f,
				mod.ProjectileType("Fire"), projectile.damage / 2 * 3, projectile.knockBack, projectile.owner);
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

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
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244);
			Main.dust[dust1].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust1].velocity = Vector2.Zero;
			Main.dust[dust2].velocity = Vector2.Zero;
			Main.dust[dust2].scale = 0.6f;
			Main.dust[dust1].scale = 0.6f;
			Lighting.AddLight(projectile.position, 0.224f, 0.139f, 0.29f);

			projectile.frameCounter++;
			if ((float)projectile.frameCounter >= 3f)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
			target.AddBuff(BuffID.OnFire, 180);

			MyPlayer mp = Main.player[projectile.owner].GetModPlayer<MyPlayer>(mod);
			if (mp.MoonSongBlossom && Main.rand.Next(10) == 0)
			{
				Projectile.NewProjectile(target.Center.X, target.Center.Y - 100, 0f, 0f,
					mod.ProjectileType("Moon"), projectile.damage / 3 * 2, 4, projectile.owner);
			}
		}

	}
}
