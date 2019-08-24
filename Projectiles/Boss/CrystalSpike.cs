using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class CrystalSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Spike");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = 600;
			projectile.hostile = true;
			projectile.damage = 13;
			projectile.friendly = false;
			projectile.light = 0.25f;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.tileCollide = true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 8)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 4;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 187);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(10) == 0)
			{
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height,
					62, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
		}

	}
}