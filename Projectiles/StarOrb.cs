using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class StarOrb : ModProjectile
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starry Electricity");

		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 280;
			projectile.height = 16;
			projectile.width = 16;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 5f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 5f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 26, 26, 187, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}

			timer++;
			if (timer == 50)
			{
				projectile.velocity *= 0.01f;
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 14f, 0f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -14f, 0f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 14f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -14f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -9.8f, -9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 9.8f, -9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -9.8f, 9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 9.8f, 9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
			}
			else if (timer == 100)
			{
				projectile.velocity *= 80f;
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 14f, 0f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -14f, 0f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, 14f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0f, -14f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -9.8f, -9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 9.8f, -9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -9.8f, 9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 9.8f, 9.8f, mod.ProjectileType("StarTrail2"), 22, 0f, projectile.owner, 0f, 0f);
			}
			else if (timer >= 110)
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
