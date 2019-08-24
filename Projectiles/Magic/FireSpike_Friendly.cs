using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class FireSpike_Friendly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Spike");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 12;

			projectile.hostile = false;
			projectile.friendly = true;

			projectile.penetrate = -1;
		}

		public override bool PreAI()
		{
			if (projectile.ai[0] == 0)
			{
				projectile.frame = Main.rand.Next(5);
				projectile.ai[0] = 1;
			}

			projectile.velocity.Y = projectile.velocity.Y + 0.1f;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;

			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Vector2 dir = Vector2.Zero;
			if (projectile.velocity.X != oldVelocity.X)
				dir.X = oldVelocity.X > 0 ? -1 : 1;
			else if (projectile.velocity.Y != oldVelocity.Y)
				dir.Y = oldVelocity.Y > 0 ? -1 : 1;

			for (int i = 0; i < 2; ++i)
			{
				Vector2 randSpeed = new Vector2(Main.rand.Next(3, 8), Main.rand.Next(3, 8)) * dir;
				if (dir.X != 0)
					randSpeed.Y *= Main.rand.Next(2) == 0 ? 0.5F : -0.5F;
				if (dir.Y != 0)
					randSpeed.X *= Main.rand.Next(2) == 0 ? 0.5F : -0.5F;

				int randFire = Main.rand.Next(3);
				int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					randSpeed.X, randSpeed.Y,
					ProjectileID.GreekFire1 + randFire, projectile.damage, 0, projectile.owner);
				Main.projectile[newProj].hostile = false;
				Main.projectile[newProj].friendly = true;
			}
			return true;
		}

	}
}
