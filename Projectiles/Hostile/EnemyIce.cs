using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Hostile
{
	public class EnemyIce : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Spike");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
			projectile.light = 0;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 360;
			projectile.friendly = false;
			projectile.hostile = true;
		}

		public override bool PreAI()
		{
			projectile.alpha -= 40;
			if (projectile.alpha < 0)
				projectile.alpha = 0;

			projectile.spriteDirection = projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 6)
					projectile.frame = 0;
			}

			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(projectile.Center, projectile.width, projectile.height, 68, (float)(Main.rand.Next(8) - 4), (float)(Main.rand.Next(8) - 4), 68);
			}
		}

	}
}