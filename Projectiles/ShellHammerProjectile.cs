
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShellHammerProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shell");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			Projectile.velocity.Y += 0.4F;
			Projectile.velocity.X *= 1.00F;
			Projectile.velocity.X = MathHelper.Clamp(Projectile.velocity.X, -10, 10);

			Projectile.frameCounter++;
			if (Projectile.frameCounter > Projectile.velocity.X * 1.5F) {
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
				Projectile.frameCounter = 0;
			}
			if (Projectile.timeLeft < 65) {
				Projectile.alpha += 3;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.X != Projectile.velocity.X)
				Projectile.velocity.X = -oldVelocity.X;

			return false;
		}
	}
}
