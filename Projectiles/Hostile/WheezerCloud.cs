using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Hostile
{
	public class WheezerCloud : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gas Cloud");
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
			projectile.light = 0;
			projectile.timeLeft = 255;
			aiType = ProjectileID.Bullet;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.scale *= .8f;
		}

		public override bool PreAI()
		{
            projectile.velocity *= .98f;
			projectile.alpha --;
			if (projectile.alpha < 0)
				projectile.alpha = 0;

			projectile.spriteDirection = projectile.direction;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 8)
					projectile.frame = 0;
			}

			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 0);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 1)
				target.AddBuff(BuffID.Poisoned, 300);
		}
	}
}