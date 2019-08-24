using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class MagalaShield : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magala Shield");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 15;
			projectile.height = 18;
			projectile.width = 12;
			aiType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			projectile.spriteDirection = projectile.direction;

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 0.9f;
			Main.dust[dust].scale = 0.9f;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("FrenzyVirus"), 580);
		}
	}
}
