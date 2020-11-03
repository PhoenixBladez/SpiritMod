using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SpiritMod.Projectiles.Hostile
{
	public class ScreechOwlNote : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cacophonous Note");
        }

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 18;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 60;
			projectile.alpha = 75;
            projectile.extraUpdates = 2;
            projectile.hide = true;
		}
		public override void AI()
        {
            projectile.ai[1]++;
            if (projectile.ai[1] == 15)
            {
                projectile.height += 10;
                for (int j = 0; j < 30; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(16f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2)) * 1.3f;
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 180, 0f, 0f, 0, new Color(), .6f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = 1.46f;
                }
            }
            if (projectile.ai[1] == 30)
            {
                projectile.height += 10;
                for (int j = 0; j < 40; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(20f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2)) * 1.3f;
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 180, 0f, 0f, 0, new Color(), .7f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = 1.36f;
                }
            }
            if (projectile.ai[1] == 45)
            {
                projectile.height += 10;
                for (int j = 0; j < 50; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(24f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2)) * 1.3f;
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 180, 0f, 0f, 0, new Color(), .8f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = 1.16f;
                }
            }
            if (projectile.ai[1] == 60)
            {
                projectile.height += 10;
                for (int j = 0; j < 60; j++)
                {
                    Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
                    vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(30f, 8f);
                    vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2)) * 1.3f;
                    int num8 = Dust.NewDust(projectile.Center, 0, 0, 180, 0f, 0f, 0, new Color(), 1.1f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].position = projectile.Center + vector2;
                    Main.dust[num8].velocity = projectile.velocity * 0.1f;
                    Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    Main.dust[num8].fadeIn = .96f;
                }
            }
        }
    }
}
