using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class StompExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shockwave");
		}

		public override void SetDefaults()
		{
			Projectile.width = 160;
			Projectile.timeLeft = 2;
			Projectile.height = 160;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
		}
	}
}
