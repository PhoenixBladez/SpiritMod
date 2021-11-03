using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Essences
{
	class EssenceOfVortex : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Essence of Vortex");

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.penetrate = 15;
			projectile.timeLeft = 240;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;

			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			float rotationSpeed = (float)Math.PI / 15;
			projectile.rotation += rotationSpeed;
			projectile.velocity *= 0.99f;

			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Clentaminator_Green);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 0.9f;
			}
		}
	}
}