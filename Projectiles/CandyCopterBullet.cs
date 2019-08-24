using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class CandyCopterBullet : ModProjectile
	{
		public const int maxTravelTime = 600;
		private static readonly Vector3 lightColor = new Vector3(0.6400f, 0.1839f, 0.3286f);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Candy Copter");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = maxTravelTime;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			if (projectile.timeLeft == maxTravelTime)
			{
				Main.PlaySound(2, projectile.position, 11); //41 /12
			}
			Point point = projectile.Center.ToTileCoordinates();
			Lighting.AddLight(point.X, point.Y, lightColor.X, lightColor.Y, lightColor.Z);
		}

	}
}
