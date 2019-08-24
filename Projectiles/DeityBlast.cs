using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class DeityBlast : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deity's Soul");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 90;
			projectile.ranged = true;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 244, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 110, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.9f;
			Main.dust[dust].scale = 1.9f;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3, -5, mod.ProjectileType("DeitySoul1"), 45, 0.4f, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -3, -5, mod.ProjectileType("DeitySoul1"), 45, 0.4f, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 2, -5, mod.ProjectileType("DeitySoul1"), 45, 0.4f, Main.myPlayer);
		}

	}
}