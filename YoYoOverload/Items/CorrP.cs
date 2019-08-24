using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class CorrP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 0;

		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(542);
			base.projectile.damage = 17;
			base.projectile.extraUpdates = 1;
			this.aiType = 542;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
		}
	}
}
