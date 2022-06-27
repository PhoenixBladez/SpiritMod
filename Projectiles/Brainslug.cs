using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Brainslug : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brainslug");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(190);
			Projectile.damage = 16;
			Projectile.extraUpdates = 1;
			AIType = 190;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			float rotationSpeed = (float)Math.PI / 15;
			Projectile.rotation += rotationSpeed;
		}

	}
}
