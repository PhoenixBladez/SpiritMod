using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Projectiles
{
	public class CorrT : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(307);
			base.projectile.damage = 17;
			base.projectile.extraUpdates = 1;
			this.aiType = 307;
		}
	}
}
