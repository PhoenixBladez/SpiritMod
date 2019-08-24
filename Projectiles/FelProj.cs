using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class FelProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Spawner");
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
			projectile.timeLeft = 230;
			projectile.alpha = 255;
			Main.projFrames[projectile.type] = 4;
		}

		int timer = 20;

		public override void AI()
		{
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			Player player = Main.player[projectile.owner];
			projectile.Center = new Vector2(mouse.X, mouse.Y);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)

			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}

			timer--;

			if (timer == 0)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 8);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + Main.rand.Next(-3, 5), projectile.velocity.Y + Main.rand.Next(-3, 5), mod.ProjectileType("FelShot"), 60, 2, projectile.owner, 0f, 0f);

				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + Main.rand.Next(-3, 5), projectile.velocity.Y + Main.rand.Next(-3, 5), mod.ProjectileType("FelShot"), 60, 2, projectile.owner, 0f, 0f);

				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + Main.rand.Next(-3, 5), projectile.velocity.Y + Main.rand.Next(-3, 5), mod.ProjectileType("FelShot"), 60, 2, projectile.owner, 0f, 0f);

				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X + Main.rand.Next(-3, 5), projectile.velocity.Y + Main.rand.Next(-3, 5), mod.ProjectileType("FelShot"), 60, 2, projectile.owner, 0f, 0f);
				timer = 45;
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
				if (num416 > 1)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}

	}
}