using Microsoft.Xna.Framework;
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
			Main.projFrames[projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(190);
			projectile.damage = 16;
			projectile.extraUpdates = 1;
			aiType = 190;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			float rotationSpeed = (float)Math.PI / 15;
			projectile.rotation += rotationSpeed;
		}

	}
}
