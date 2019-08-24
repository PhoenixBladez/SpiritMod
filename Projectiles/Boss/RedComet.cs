using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Boss
{
	public class RedComet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Comet");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.timeLeft = 1000;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;
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
				if (projectile.frame >= 4)
					projectile.frame = 0;

			}
			return true;
		}

		public override void AI()
		{
			if (Main.rand.Next(3) == 0)
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height,
					60, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

	}
}