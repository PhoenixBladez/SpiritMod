using System;
using Terraria;
using Terraria.ID;
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
			projectile.width = 160;
			projectile.timeLeft = 2;
			projectile.height = 160;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
		}
	}
}
