using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.NPCs.Hemophora
{
	public class HemophoraProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Blood Clump");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.timeLeft = 255;
            projectile.light = 0;
            projectile.width = 18;
            projectile.height = 24;
			aiType = ProjectileID.Bullet;
			projectile.friendly = false;
			projectile.hostile = true;
		}

		public override void AI()
        {
            projectile.ai[0] += .1135f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(1))
            {
                float x = projectile.Center.X - projectile.velocity.X / 10f;
                float y = projectile.Center.Y - projectile.velocity.Y / 10f;
                int num623 = Dust.NewDust(new Vector2(x, y), 4, 4,
                    5, 0f, 0f, 0, default(Color), .7f);
                Main.dust[num623].velocity *= .12f;
                Main.dust[num623].noGravity = true;
                Main.dust[num623].fadeIn = .8f;
                Main.dust[num623].alpha = 100;
            }

            projectile.frameCounter++;
			if (projectile.frameCounter >= 6) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 3)
					projectile.frame = 0;
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 1)
				target.AddBuff(BuffID.Poisoned, 300);
            if (Main.rand.Next(2) == 1)
                target.AddBuff(BuffID.Darkness, 420);
            if (Main.rand.Next(2) == 1)
                target.AddBuff(BuffID.Confused, 120);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(projectile.Center, 20, 20, 5, projectile.velocity.X * 1.15f, projectile.velocity.Y * 1f, 0, default(Color), Main.rand.NextFloat(.45f, 1.15f));
                Main.dust[d].noGravity = true;
            }
        }
	}
}