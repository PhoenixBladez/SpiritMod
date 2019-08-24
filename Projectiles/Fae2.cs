using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Fae2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fae Blast");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.alpha = 255;
			projectile.timeLeft = 150;
			projectile.penetrate = 1;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 242, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + 10, projectile.velocity.Y + 10, mod.ProjectileType("Fae"), 25, projectile.knockBack, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X - 10, projectile.velocity.Y - 10, mod.ProjectileType("Fae"), 25, projectile.knockBack, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X - 10, projectile.velocity.Y + 10, mod.ProjectileType("Fae"), 25, projectile.knockBack, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + 10, projectile.velocity.Y - 10, mod.ProjectileType("Fae"), 25, projectile.knockBack, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y + 10, mod.ProjectileType("Fae"), 25, projectile.knockBack, projectile.owner, 0f, 0f);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y - 10, mod.ProjectileType("Fae"), 25, projectile.knockBack, projectile.owner, 0f, 0f);
		}
	}
}