using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class StardropStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardrop");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hide = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 2; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
			}

			if (Main.rand.Next(3) == 1)
			{
				Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);

				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, -5, mod.ProjectileType("StardropStaffProj2"), projectile.damage, projectile.knockBack, Main.myPlayer);

				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 6, -2, mod.ProjectileType("StardropStaffProj2"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -6, -2, mod.ProjectileType("StardropStaffProj2"), projectile.damage, projectile.knockBack, Main.myPlayer);

				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 3, 5, mod.ProjectileType("StardropStaffProj2"), projectile.damage, projectile.knockBack, Main.myPlayer);
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, -3, 5, mod.ProjectileType("StardropStaffProj2"), projectile.damage, projectile.knockBack, Main.myPlayer);
			}
		}

		public override void AI()
		{
			int timer = 0;
			projectile.velocity *= 0.95f;

			for (int i = 1; i <= 3; i++)
			{
				int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 2f);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(mod.BuffType("TidalEbb"), 240);
		}

	}
}
