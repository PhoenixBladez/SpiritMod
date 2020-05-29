using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class EnergyFieldStarplate : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energizer Field");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 140;
			projectile.height = 140;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 4;
			projectile.alpha = 255;
			projectile.timeLeft = 300;
			projectile.tileCollide = false; //Tells the game whether or not it can collide with a tile
		}

		public override bool PreAI()
		{
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if (projectile != proj && proj.friendly && proj.ranged && !proj.GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon)
                {
                    proj.GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon = true;
					proj.damage += (int)(proj.damage * 0.15);
                }
            }
            for (int k = 0; k < 4; k++)
            {
                int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 226);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 84f;
                Main.dust[dust].position = projectile.Center - vector2_3;
            }
            return false;
		}
		public override void AI()
		{
			int timer = 0;

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

			++projectile.localAI[1];
			int minRadius = 1;
			int minSpeed = 1;

		}
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            player.AddBuff(mod.BuffType("StarCooldown"), 720);
        }
	}
}
