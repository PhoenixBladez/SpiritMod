
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShadowSingeProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Singe");
			Main.projFrames[Projectile.type] = 7;
		}

		public override void SetDefaults()
		{
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.friendly = true;
			Projectile.timeLeft = 28;
			Projectile.penetrate = 5;
		}

		public override bool PreAI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 4) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 6) {
				Projectile.frame = 0;
			}
			Projectile.alpha += 3;
			return false;
		}
	}
}
