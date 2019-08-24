using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Blacksludge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black Sludge");
		}

		public override void SetDefaults()
		{
			projectile.width = 5;
			projectile.height = 9;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
		}

		public override void Kill(int timeLeft)
		{
			for (int I = 0; I < 8; I++)
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 93, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);

			for (int i = 0; i < 3; ++i)
			{
				int randFire = Main.rand.Next(1);
				int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					Main.rand.Next(0) / 100, Main.rand.Next(0, 0),
					mod.ProjectileType("AbyssalSludge"), projectile.damage, 0, projectile.owner);
			}
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 93, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("Brine"), 180);

			for (int i = 0; i < 3; ++i)
			{
				int randFire = Main.rand.Next(1);
				int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					Main.rand.Next(0) / 100, Main.rand.Next(0, 0),
					mod.ProjectileType("AbyssalSludge"), projectile.damage, 0, projectile.owner);
			}
		}

	}
}