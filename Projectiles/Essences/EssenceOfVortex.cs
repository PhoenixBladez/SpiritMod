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
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 15;
			Projectile.timeLeft = 240;
			Projectile.height = 6;
			Projectile.width = 6;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;

			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			float rotationSpeed = (float)Math.PI / 15;
			Projectile.rotation += rotationSpeed;
			Projectile.velocity *= 0.99f;

			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Clentaminator_Green);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 0.9f;
			}
		}
	}
}