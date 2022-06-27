using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class SmallExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Small Explosion");
		}

		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.timeLeft = 10;
			Projectile.height = 36;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
		}
	}
}
