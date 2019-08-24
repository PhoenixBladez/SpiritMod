using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SummonPortal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiritguard Portal");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.sentry = true;
			projectile.alpha = 255;
			projectile.timeLeft = 3600;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = false;
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 173, 0f, 0f);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;

			int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, 0f, 0f);
			Main.dust[dust1].scale = 1.5f;
			Main.dust[dust1].noGravity = true;
			return true;
		}

		int timer = 50;
		int timer2 = 0;
		public override void AI()
		{
			timer--;

			if (timer == 0)
			{

				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++)
				{
					float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(projectile, false))
					{
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1)
				{
					bool flag = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag)
					{
						Vector2 value = Main.npc[num2].Center - projectile.Center;
						float num4 = 5f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
						{
							num5 = num4 / num5;
						}
						value *= num5;
						int p = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, mod.ProjectileType("StarSoul"), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
				timer = 25;
			}
			timer2++;
			if (timer2 == 100)
			{
				for (int i = 0; i < 6; ++i)
				{
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 92);
					Vector2 targetDir = ((((float)Math.PI * 2) / 6) * i).ToRotationVector2();
					targetDir.Normalize();
					targetDir *= 15;
					int y = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, targetDir.X, targetDir.Y, mod.ProjectileType("ShadowPulse1"), 55, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
					Main.projectile[y].friendly = true;
					Main.projectile[y].hostile = false;
					timer2 = 0;
				}
			}
		}

	}
}
