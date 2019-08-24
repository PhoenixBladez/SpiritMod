using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	public class SpiritBall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Shatter");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 14;
			projectile.height = 14;
			projectile.timeLeft = 300;
			projectile.penetrate = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(mod.BuffType("SoulBurn"), 200, true);

			projectile.Kill();
		}

		public override bool PreAI()
		{
			projectile.rotation += 0.2f;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 187, 0f, 0f, 100, default(Color), 3f);
			Main.dust[num624].noGravity = true;
			Main.dust[num624].velocity *= 5f;
			num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 187, 0f, 0f, 100, default(Color), 2f);
			Main.dust[num624].velocity *= 2f;

			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 107);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SpiritLinger"), projectile.damage, projectile.knockBack, projectile.owner);
		}

	}
}



