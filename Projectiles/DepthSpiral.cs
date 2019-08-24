using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class DepthSpiral : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Spiral");
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 150;
			projectile.height = 6;
			projectile.width = 6;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.melee = true;
		}

		public override void AI()
		{
			projectile.localAI[1] += 1f;
			int num = 1;
			int num2 = 1;
			if ((double)projectile.localAI[1] <= 1.0)
			{
				int num3 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("DepthBolt"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num3].localAI[0] = (float)projectile.whoAmI;
				return;
			}
			int num4 = (int)projectile.localAI[1];
			if (num4 <= 30)
			{
				if (num4 != 10)
				{
					if (num4 == 30)
					{
						num2--;
					}
				}
				else
				{
					num2--;
				}
			}
			else if (num4 != 50)
			{
				if (num4 == 70)
				{
					num2--;
				}
			}
			else
			{
				num2--;
			}

			if ((int)projectile.localAI[1] == 20)
			{
				int num5 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("DepthBolt"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num5].localAI[0] = (float)projectile.whoAmI;
			}
			if ((int)projectile.localAI[1] == 30)
			{
				int num6 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("?"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num6].localAI[0] = (float)projectile.whoAmI;
			}
			if ((int)projectile.localAI[1] == 40)
			{
				int num7 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)num, (float)num2, mod.ProjectileType("?"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
				Main.projectile[num7].localAI[0] = (float)projectile.whoAmI;
			}
		}

	}
}
