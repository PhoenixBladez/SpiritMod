using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BlizzardSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icicle");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 30;
			projectile.timeLeft = 80;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			if (Main.rand.Next(6) == 0)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
			}

			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 5)
				{
					projectile.frame = 0;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}
