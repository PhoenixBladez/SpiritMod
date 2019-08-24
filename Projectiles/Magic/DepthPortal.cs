using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class DepthPortal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seawater Portal");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 50;
			projectile.height = 50;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.timeLeft = 150;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = false;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 15, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		int timer = 30;

		public override void AI()
		{
			timer--;

			if (timer == 0)
			{
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + Main.rand.Next(10, -10), projectile.velocity.Y, mod.ProjectileType("DepthBall"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 30;
			}

			projectile.frameCounter++;
			if (projectile.frameCounter > 8)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame > 5)
					projectile.frame = 0;
			}

			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 7200f)
			{
				projectile.alpha += 5;
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] >= 10f)
			{
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 2)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}

	}
}
