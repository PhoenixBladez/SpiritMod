using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SpiritMod.Projectiles.Magic
{
	public class GunBubble3 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.tileCollide = true;
            projectile.magic = true;
            projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 150;
			projectile.alpha = 110;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 150) {
				projectile.scale = Main.rand.NextFloat(0.6f, 1.1f);
			}
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y -= 0.015f;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 165, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .3f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 7f;
			}
		}
	}
}
