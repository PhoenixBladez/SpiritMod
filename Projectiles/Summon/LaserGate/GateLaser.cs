using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class GateLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gate Laser");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 1;
			projectile.height = 1;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.damage = 1;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 1;
			projectile.tileCollide = true;
		}

	}
}