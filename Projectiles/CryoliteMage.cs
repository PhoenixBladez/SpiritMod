using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class CryoliteMage : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Blast");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
			projectile.height = 18;
			projectile.width = 18;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;
		}
        int counter;
		public override void AI()
		{
			projectile.velocity *= 0.99f;

            {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 68, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 180, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 0.6f;
                Main.dust[dust2].velocity *= 0.1f;
                Main.dust[dust2].scale = 1.2f;
                Main.dust[dust].scale = .8f;
            }
            counter++;
            if (counter >= 1440)
            {
                counter = -1440;
            }
            for (int i = 0; i < 4; i++)
            {
                float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

                int num2121 = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 180, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num2121].velocity *= 0f;
                Main.dust[num2121].scale *= .75f;
                Main.dust[num2121].noGravity = true;

            }

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(mod.BuffType("CryoCrush"), 300);
		}

	}
}
