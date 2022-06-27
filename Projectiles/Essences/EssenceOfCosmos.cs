using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Essences
{
	class EssenceOfCosmos : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Essence of Cosmos");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
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

			for (int i = 0; i < 2; ++i)
			{
				int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GoldFlame);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Vector2.Zero;
				Main.dust[dust].scale = 0.9f;
			}
		}
	}
}
