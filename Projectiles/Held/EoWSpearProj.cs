using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Held
{
	public class EoWSpearProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Scourge");
		}

		int timer = 10;
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Trident);

			aiType = ProjectileID.Trident;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 1)
			{
				int n = Main.rand.Next(1, 3);
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++)
				{
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 5.5f;
					perturbedSpeed.Y *= 5.5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.TinyEater, projectile.damage / 2, projectile.knockBack, projectile.owner);
				}
			}
		}

	}
}
