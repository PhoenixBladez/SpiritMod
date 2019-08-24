using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Projectiles
{
	public class ShellHammerProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shell");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.timeLeft = 300;
			projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			projectile.velocity.Y += 0.4F;
			projectile.velocity.X *= 1.005F;
			projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -10, 10);

			projectile.frameCounter++;
			if (projectile.frameCounter > projectile.velocity.X * 1.5F)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != projectile.velocity.X)
				projectile.velocity.X = -oldVelocity.X;

			return false;
		}
	}
}
