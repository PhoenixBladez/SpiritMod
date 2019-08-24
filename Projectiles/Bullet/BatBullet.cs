using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class BatBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 26;

			projectile.ranged = true;
			projectile.friendly = true;

			projectile.penetrate = 10;
			projectile.tileCollide = false;
			projectile.timeLeft = 200;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
				for (int num623 = 0; num623 < 25; num623++)
				{
					int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 191, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].noGravity = true;
					Main.dust[num622].velocity *= 1.5f;
					Main.dust[num622].scale = 0.8f;
				}
		}
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
			int num622 = Dust.NewDust(new Vector2(projectile.position.X - projectile.velocity.X, projectile.position.Y - projectile.velocity.Y), projectile.width, projectile.height, 191, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num622].noGravity = true;
				Main.dust[num622].scale = 0.5f;
				
				projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 5)
					projectile.frame = 0;
			}
		}


	}
}
