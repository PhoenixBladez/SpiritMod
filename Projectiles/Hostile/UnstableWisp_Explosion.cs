using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class UnstableWisp_Explosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Wisp");
			Main.projFrames[base.projectile.type] = 9;
		}

		public override void SetDefaults()
		{
			projectile.width = 124;
			projectile.height = 106;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
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
				{
					projectile.Kill();
				}
			}
			return false;
		}

	}
}
