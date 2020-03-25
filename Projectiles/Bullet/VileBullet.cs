using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class VileBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vile Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 22;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
            projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}
        int dustType;
		public override void AI()
		{
            projectile.alpha = 255;
            bool flag25 = false;
            int jim = 1;
            for (int index1 = 0; index1 < 200; index1++)
            {
                if (Main.npc[index1].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1) && Main.npc[index1].HasBuff(mod.BuffType("Tracked")))
                {
                    float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
                    float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
                    float num25 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num23) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num24);
                    if (num25 < 500f)
                    {
                        flag25 = true;
                        jim = index1;
                    }
                }
            }
           
            if (flag25)
            {
                float num1 = 12f;
                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num2 = Main.npc[jim].Center.X - vector2.X;
                float num3 = Main.npc[jim].Center.Y - vector2.Y;
                float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
                float num5 = num1 / num4;
                float num6 = num2 * num5;
                float num7 = num3 * num5;
                int num8 = 10;
                dustType = 173;
                projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
            }
            if (!flag25)
            {
                dustType = 27;
            }
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            {
                for (int i = 0; i < 10; i++)
                {
                    float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                    float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
                    int num = Dust.NewDust(new Vector2(x, y), 2, 2, dustType);
                    Main.dust[num].alpha = projectile.alpha;
                    Main.dust[num].velocity = Vector2.Zero;
                    Main.dust[num].noGravity = true;
                }
            }
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("ShadowCurse"), 120, true);
		}
	}
}
