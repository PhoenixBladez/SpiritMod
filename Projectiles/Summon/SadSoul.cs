using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class SadSoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Sadness");
		}

		int timer = 25;

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 24;
			projectile.timeLeft = 3600;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 24;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			timer--;

			if (timer <= 0)
			{
				Projectile.NewProjectile(projectile.Center.X + 5, projectile.Center.Y + 4, 0, Main.rand.Next(8, 18), mod.ProjectileType("SadBeam"), 18, projectile.knockBack, projectile.owner, 0f, 0f);
				Projectile.NewProjectile(projectile.Center.X - 5, projectile.Center.Y + 4, 0, Main.rand.Next(8, 18), mod.ProjectileType("SadBeam"), 18, projectile.knockBack, projectile.owner, 0f, 0f);
				timer = 25;
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
