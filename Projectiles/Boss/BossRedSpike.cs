using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class BossRedSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Needle Spike");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 54;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.tileCollide = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			if (Main.rand.Next(15) == 1)
			{
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 235, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0f;
			}
		}

		public override void Kill(int timeLeft)
		{
			int num622 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
				235, 0f, 0f, 100, default(Color), 2f);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(15) == 1)
				target.AddBuff(BuffID.Bleeding, 200);
		}

	}
}