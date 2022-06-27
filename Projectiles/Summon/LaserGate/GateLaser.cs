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
			Projectile.hostile = false;
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.damage = 1;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = true;
		}

	}
}