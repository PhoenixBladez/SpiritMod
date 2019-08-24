using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class TidalShard : ModProjectile
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Shard");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 300;
			projectile.height = 16;
			projectile.width = 12;
			projectile.penetrate = 2;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.spriteDirection = projectile.direction;
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height - 10, 172, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height - 10, 172, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust2].scale = 1.2f;
				Main.dust[dust].scale = 1.2f;
			}

			timer++;
			if (timer == 100)
				projectile.velocity *= 1.1f;
			else if (timer == 200)
				projectile.velocity *= 0.50f;
			else if (timer >= 300)
				timer = 0;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(mod.BuffType("TidalEbb"), 240);
		}
	}
}
