using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class EelOrb : ModProjectile
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Orb");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 500;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
					int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
					int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.GoldCoin, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].velocity *= 0f;
					Main.dust[dust2].velocity *= 0f;
					Main.dust[dust2].scale = 0.9f;
					Main.dust[dust].scale = 0.9f;

			timer++;
			if (timer == 50)
			{
				projectile.velocity *= 0.01f;
			}
			if (timer == 100)
			{
				projectile.velocity *= 80f;
			}
			if (timer >= 110)
			{
				timer = 0;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) == 0)
				target.AddBuff(mod.BuffType("ElectrifiedV2"), 240, true);
		}

	}
}
