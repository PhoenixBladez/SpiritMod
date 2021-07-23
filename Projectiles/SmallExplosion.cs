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
			projectile.width = 36;
			projectile.timeLeft = 10;
			projectile.height = 36;
			projectile.magic = true;
			projectile.penetrate = 3;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
		}
	}
}
