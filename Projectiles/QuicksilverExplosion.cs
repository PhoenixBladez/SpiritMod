using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class QuicksilverExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Explosion");
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 30;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void Kill(int timeLeft)
		{
			for (int h = 0; h < 2; h++)
			{
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 8f;
				Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, mod.ProjectileType("QuicksilverBolt"), 45, 1, projectile.owner, -20, 0f);

			}
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("Wrath"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);

			for (int i = 0; i < 5; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 20, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;

			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}
	}
}
