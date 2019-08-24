using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class TerraBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			aiType = ProjectileID.Bullet;
			projectile.alpha = 255;
			projectile.penetrate = 2;
			projectile.friendly = true;
			projectile.ranged = true;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 107);
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Frostburn, 300, true);

			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.CursedInferno, 300, true);

		}

	}
}
