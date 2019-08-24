using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Projectiles
{
	public class ChaosB : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(15);
			base.projectile.damage = 40;
			base.projectile.extraUpdates = 1;
			this.aiType = 15;
			base.projectile.timeLeft = 10;
			projectile.width = 30;
			projectile.alpha = 255;
			projectile.height = 30;
			Main.projFrames[base.projectile.type] = 1;
		}
	}
}
