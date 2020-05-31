using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class StarTrail : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Trail");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;

			projectile.timeLeft = 30;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
		}
	}
}
