using Microsoft.Xna.Framework;
using SpiritMod.Dusts;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Projectiles
{
	class DarkAnima : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Anima");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 500;
			projectile.height = 24;
			projectile.width = 10;
			projectile.scale = .8f;
            projectile.extraUpdates = 5;
        }
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<AnimaExplosion>(), 55, 0, Main.myPlayer);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] == 16f) {
				projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, ModContent.DustType<NightmareDust>(), 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
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


                float num1 = 10f;
                Vector2 vector2 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num2 = Main.npc[jim].Center.X - vector2.X;
                float num3 = Main.npc[jim].Center.Y - vector2.Y;
                float num4 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
                float num5 = num1 / num4;
                float num6 = num2 * num5;
                float num7 = num3 * num5;
                int num8 = 10;
                projectile.velocity.X = (projectile.velocity.X * (float)(num8 - 1) + num6) / (float)num8;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num8 - 1) + num7) / (float)num8;
            }
        }
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(60, 60, 60, 100);
		}
	}
}
