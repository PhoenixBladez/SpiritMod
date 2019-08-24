using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class HostileWrath : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun's Wrath");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.timeLeft = 10;
			projectile.alpha = 255;
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
					projectile.Kill();
			}
			return false;
		}
	}
}
