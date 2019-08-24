using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	class SpiritScythe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Scythe");
			Main.projFrames[base.projectile.type] = 6;
		}

		int timer = 0;
		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 2;
			projectile.tileCollide = true;
			projectile.timeLeft = 300;
			projectile.height = 28;
			projectile.width = 28;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation += 0.2f;

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X, projectile.velocity.Y);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X, projectile.velocity.Y);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = 1.2f;

			timer++;
			if (timer == 10)
				projectile.velocity *= 0.1f;
			else if (timer == 20)
				projectile.velocity *= 5f;
			else if (timer == 40)
				projectile.velocity *= 5f;
			else if (timer == 300)
				timer = 0;

		}

		public override void Kill(int timeLeft)
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = 1.2f;
		}

	}
}
