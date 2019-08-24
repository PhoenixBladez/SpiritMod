using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BloodRain : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Rain");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 120;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 6);
			for (int I = 0; I < 8; I++)
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 5, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
			for (int i = 0; i < 3; ++i)
			{
				int randFire = Main.rand.Next(3);
				int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
					Main.rand.Next(-400, 400) / 100, Main.rand.Next(-4, 4),
					mod.ProjectileType("Terrorflame"), projectile.damage, 0, projectile.owner);
			}
		}

		public override bool PreAI()
		{
			if (Main.rand.Next(4) == 1)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("Wither"), 180);
		}

	}
}
