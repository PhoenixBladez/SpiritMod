using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class Typhoon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Typhoon");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(555);
			projectile.extraUpdates = 1;
			aiType = 555;
		}

		public override void AI()
		{
			projectile.rotation -= 10f;
		}

		public override bool PreAI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 60)
			{
				projectile.frameCounter = 0;
				float num = 2000f;
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
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
						{
							num5 = num4 / num5;
						}
						value *= num5;
						Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, 408, projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
					}
				}
			}
			return true;
		}

	}
}
