using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class EarthshakerProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthshaker");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SpikyBall);
			projectile.width = 54;
			projectile.penetrate = 3;
			projectile.height = 54;
			projectile.timeLeft = 180;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();

			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = -oldVelocity.X;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = -oldVelocity.Y * 1.3f;

			for (int i = 0; i < 2; ++i)
			{
				Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
				targetDir.Normalize();
				targetDir *= 3;
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, targetDir.X, targetDir.Y, mod.ProjectileType("Earthdust"), 30, projectile.knockBack, projectile.owner);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 8);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

	}
}