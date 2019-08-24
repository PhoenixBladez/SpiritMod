using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword
{
	public class WaterMass : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Mass");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 30;

			projectile.melee = true;
			projectile.friendly = true;

			projectile.penetrate = 6;
		}

		public override bool PreAI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 3)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}
			projectile.rotation = projectile.velocity.ToRotation();

			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3)
			{
				projectile.tileCollide = false;
				projectile.ai[1] = 0f;
				projectile.alpha = 255;

				projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
				projectile.width = 128;
				projectile.height = 128;
				projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
				projectile.knockBack = 8f;

				projectile.ai[0] = 1;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("TidalWrath"), 300);

			if (Main.rand.Next(6) == 0 && projectile.timeLeft > 3)
			{
				projectile.velocity *= 0f;
				projectile.alpha = 255;
				projectile.timeLeft = 3;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.rand.Next(6) == 0)
			{
				projectile.velocity *= 0f;
				projectile.alpha = 255;
				projectile.timeLeft = 3;
				return false;
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			if (!projectile.active)
				return;

			if (projectile.ai[0] != 1)
			{
				for (int i = 0; i < 15; ++i)
				{
					int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 2.5f);
					Main.dust[newDust].noGravity = true;
					Main.dust[newDust].velocity *= 3F;
					newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 1.5f);
					Main.dust[newDust].velocity *= 1.5F;
				}
				return;
			}

			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = 22;
			projectile.height = 22;
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (int num617 = 0; num617 < 20; num617++)
			{
				int num618 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 3.5f);
				Main.dust[num618].noGravity = true;
				Main.dust[num618].velocity *= 7f;
				num618 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 172, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num618].velocity *= 3f;
			}
			projectile.active = false;
		}

	}
}
