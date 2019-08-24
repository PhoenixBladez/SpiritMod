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
	public class HappySoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Happiness");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 20;
			projectile.timeLeft = 3600;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 20;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 30)
			{
				projectile.frameCounter = 0;
				float num = 8000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++)
				{
					float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 1640f && Main.npc[i].CanBeChasedBy(projectile, false))
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
							num5 = num4 / num5;

						value *= num5;
						int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, mod.ProjectileType("NovaBeam1"), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}

	}
}
