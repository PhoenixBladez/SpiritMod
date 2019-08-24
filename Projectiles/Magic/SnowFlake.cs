using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SnowFlake : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow Crystal");
		}

		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 34;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 68);
			}

			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, -2, ProjectileID.NorthPoleSnowflake, projectile.damage, projectile.knockBack, Main.myPlayer);
			Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, -2, ProjectileID.NorthPoleSnowflake, projectile.damage, projectile.knockBack, Main.myPlayer);
		}

		public override void AI()
		{
			int timer = 0;
			projectile.velocity *= 0.95f;
			projectile.rotation += 0.3f;


			for (int i = 1; i <= 3; i++)
			{
				if (Main.rand.Next(4) == 0)
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 68);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(8) == 0)
				target.AddBuff(BuffID.Frostburn, 300, true);
		}

	}
}