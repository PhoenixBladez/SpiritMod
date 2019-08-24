using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown.Artifact
{
	public class RotSeeker : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Seeker");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.thrown = true;
			projectile.tileCollide = false;
			projectile.penetrate = 3;
			projectile.timeLeft = 190;
			projectile.height = 16;
			projectile.width = 36;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
			projectile.penetrate = 3;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
				projectile.frameCounter = 0;
			}

			bool flag25 = false;
			int jim = 1;
			for (int index1 = 0; index1 < 200; index1++)
			{
				if (Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1))
				{
					float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
					float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
					float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
					if (num25 < 300f)
					{
						flag25 = true;
						jim = index1;
					}
				}
			}

			if (flag25)
			{
				float num1 = 4f;
				Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num2 = Main.npc[jim].Center.X - vector2.X;
				float num3 = Main.npc[jim].Center.Y - vector2.Y;
				float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				float num5 = num1 / num4;
				float num6 = num2 * num5;
				float num7 = num3 * num5;
				int num8 = 30;
				projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
				projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
			}
			for (int index1 = 0; index1 < 5; ++index1)
			{
				float num1 = projectile.velocity.X * 0.2f * (float)index1;
				float num2 = (float)-((double)projectile.velocity.Y * 0.200000002980232) * (float)index1;
				int index2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 109, 0.0f, 0.0f, 100, new Color(), 1.3f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.0f;
				Main.dust[index2].scale = 0.3f;
				Main.dust[index2].position.X -= num1;
				Main.dust[index2].position.Y -= num2;
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

					if (num416 > 6)
					{
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("RotExplosion"), projectile.damage / 3 * 4, projectile.knockBack, projectile.owner, 0f, 0f);

						return;
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("RotExplosion"), projectile.damage / 3 * 4, projectile.knockBack, projectile.owner, 0f, 0f);
		}

	}
}