using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword.Artifact
{
	public class SoulBurst : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(" Explosion");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 70;
			projectile.height = 86;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
		}

		public override bool PreAI()
		{
			if (projectile.ai[0] == 0f)
			{
				projectile.Damage();
				projectile.ai[0] = 1f;
			}

			projectile.frameCounter++;
			if (projectile.frameCounter > 3)
			{
				;
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame > Main.projFrames[projectile.type])
				{
					projectile.Kill();
				}
			}
			return false;
		}

		public override void AI()
		{
			int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 110, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[d].noGravity = true;
		}

	}
}
