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
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.timeLeft = 255;
            Projectile.light = 0;
            Projectile.width = 18;
            Projectile.height = 24;
			AIType = ProjectileID.Bullet;
			Projectile.friendly = false;
			Projectile.hostile = true;
		}

		public override void AI()
        {
            Projectile.ai[0] += .1135f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(1))
            {
                float x = Projectile.Center.X - Projectile.velocity.X / 10f;
                float y = Projectile.Center.Y - Projectile.velocity.Y / 10f;
                int num623 = Dust.NewDust(new Vector2(x, y), 4, 4, DustID.Blood, 0f, 0f, 0, default, .7f);
                Main.dust[num623].velocity *= .12f;
                Main.dust[num623].noGravity = true;
                Main.dust[num623].fadeIn = .8f;
                Main.dust[num623].alpha = 100;
            }

            Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 3)
					Projectile.frame = 0;
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
                int d = Dust.NewDust(Projectile.Center, 20, 20, DustID.Blood, Projectile.velocity.X * 1.15f, Projectile.velocity.Y * 1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));
                Main.dust[d].noGravity = true;
            }
        }
	}
}