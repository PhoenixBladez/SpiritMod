using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class ReachKnife : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bramble Daggers");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 5;
			projectile.height = 20;
			projectile.aiStyle = 113;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.extraUpdates = 2;
			projectile.light = 0;
			aiType = ProjectileID.ThrowingKnife;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(5) == 1)
			{
				int n = Main.rand.Next(2, 3);
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++)
				{
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 5.5f;
					perturbedSpeed.Y *= 5.5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.SporeCloud, 13, projectile.knockBack, projectile.owner);
				}
			}

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 110);
			}
		}

	}
}



