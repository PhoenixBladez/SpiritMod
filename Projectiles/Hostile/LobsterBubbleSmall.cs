using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
namespace SpiritMod.Projectiles.Hostile
{
	public class LobsterBubbleSmall : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lobster Bubble");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
			projectile.alpha = 75;
		}

		public override void AI()
		{
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y -= 0.015f;
		}
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 54);
            for (int i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 165, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .2825f;
                if (Main.dust[num].position != projectile.Center)
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 5f;
            }
        }
    }
}
