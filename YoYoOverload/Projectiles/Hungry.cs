using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Projectiles
{
	public class Hungry : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;
            Main.projFrames[base.projectile.type] = 4;
        }
        public override void SetDefaults()
		{
			base.projectile.CloneDefaults(575);
			base.projectile.damage = 25;
			base.projectile.extraUpdates = 1;
			this.aiType = 575;
			base.projectile.timeLeft = 360;
		}
	}
}
