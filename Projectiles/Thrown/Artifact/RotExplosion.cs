using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Artifact
{
	public class RotExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Explosion");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 52;
			projectile.height = 52;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.thrown = true;
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
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame > Main.projFrames[projectile.type])
					projectile.Kill();
			}
			return false;
		}

		public override void AI()
		{
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Pestilence"), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
		}

	}
}
