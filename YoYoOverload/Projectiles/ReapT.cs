using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Projectiles
{
	public class ReapT : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(131);
			base.projectile.damage = 57;
			base.projectile.extraUpdates = 1;
			this.aiType = 131;
			base.projectile.timeLeft = 360;
		}
	}
}
