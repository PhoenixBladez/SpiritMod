using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class IchorClotProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Clot");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 25;
			projectile.timeLeft = 3000;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 25;
			projectile.aiStyle = -1;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position + projectile.velocity,
				projectile.width, projectile.height,
				5, Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = 2f;
			projectile.frameCounter++;
			if (projectile.frameCounter >= 40)
			{
				projectile.frameCounter = 0;
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
					bool flag = Collision.CanHit(projectile.position, projectile.width,
						projectile.height, Main.npc[num2].position,
						Main.npc[num2].width, Main.npc[num2].height);
					if (flag)
					{
						Vector2 value = Main.npc[num2].Center - projectile.Center;
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
						{
							num5 = num4 / num5;
						}
						value *= num5;
						int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y,
							value.X, value.Y, ProjectileID.GoldenShowerFriendly,
							projectile.damage, projectile.knockBack * .5f, projectile.owner);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}

	}
}
