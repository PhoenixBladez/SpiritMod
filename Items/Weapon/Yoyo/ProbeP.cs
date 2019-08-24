using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class ProbeP : ModProjectile
	{
		public override void SetDefaults()
		{
			base.projectile.CloneDefaults(549);
			base.projectile.damage = 52;
			base.projectile.extraUpdates = 3;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[base.projectile.type] = 1;
			this.aiType = 549;
		}

		public override void PostAI()
		{
			base.projectile.rotation -= 10f;
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
                        int p = Terraria.Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, ProjectileID.DeathLaser, 30, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
                        Main.projectile[p].friendly = true;
                        Main.projectile[p].hostile = false;
                    }
                }
            }
        }
    }
}
