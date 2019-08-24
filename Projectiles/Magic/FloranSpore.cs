using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class FloranSpore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Spore");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1000;
			projectile.alpha = 0;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.ai[0]++;
			if (projectile.velocity.Length() >= 0.1)
			{
				if (projectile.velocity.X > 0)
					projectile.velocity.X -= 0.2f;
				else if (projectile.velocity.X < 0)
					projectile.velocity.X += 0.2f;

				if (projectile.velocity.Y > 0)
					projectile.velocity.Y -= 0.2f;
				else if (projectile.velocity.Y < 0)
					projectile.velocity.Y += 0.2f;

				if (Main.rand.Next(5) == 0)
				{
					int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44);
					Main.dust[d].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[d].scale *= 0.2f;
				}

				if (projectile.velocity.Length() <= 0.1f)
					projectile.velocity = Vector2.Zero;
				if (projectile.ai[0] % 2 == 0)
					projectile.alpha += 3;
				if (projectile.alpha >= 200)
					projectile.Kill();
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(projectile.Center, projectile.width, projectile.height, 44, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 133);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("VineTrap"), 180);
		}

	}
}