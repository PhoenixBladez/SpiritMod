using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Firespike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fire Spike");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.alpha = 255;

			projectile.hostile = false;
			projectile.friendly = true;

			projectile.penetrate = -1;

			Main.projFrames[projectile.type] = 1;
		}

		public override bool PreAI()
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
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

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 6, 0f, 0f); //to make some with gravity to fly all over the place :P

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 120, true);
		}

	}
}
