using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Projectiles
{
	public class FierySetExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Wrath");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.timeLeft = 2;
			projectile.height = 2;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
		}

		public override void AI()
		{
            projectile.Kill();
        }

		public override void Kill(int timeLeft)
		{
            int n = 8;
            int deviation = Main.rand.Next(0, 300);
            for (int i = 0; i < n; i++)
            {
                float rotation = MathHelper.ToRadians(270 / n * i + deviation);
                Vector2 perturbedSpeed = new Vector2(projectile.velocity.X + 1, projectile.velocity.Y).RotatedBy(rotation);
                perturbedSpeed.Normalize();
                perturbedSpeed.X *= 2.5f;
                perturbedSpeed.Y *= 2.5f;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Blaze"), projectile.damage, projectile.knockBack, projectile.owner);
            }
        }
	}
}
