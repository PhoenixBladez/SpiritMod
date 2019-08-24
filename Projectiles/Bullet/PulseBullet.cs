using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class PulseBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulse Bullet");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.height = 20;
			projectile.width = 8;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 1;

		}

		int timer = 1;
		public override void AI()
		{
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3F, 0.06F, 0.01F);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 2; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 235);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(projectile.position, projectile.width, projectile.height,
				235, 0f, 0f, 100, default(Color), 3f);
			Main.dust[num624].velocity = Vector2.Zero;
			Main.dust[num624].scale *= 0.3f;
			Main.dust[num624].noGravity = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}
