using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class ShadowflameBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Bullet");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 4;

			projectile.ranged = true;
			projectile.friendly = true;

			projectile.scale = 1.2f;

			projectile.penetrate = 1;
			projectile.timeLeft = 600;

			projectile.extraUpdates = 1;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57F;
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3F, 0.06F, 0.3F);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);

			for (int num383 = 0; num383 < 5; num383++)
			{
				int num384 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Shadowflame);
				Main.dust[num384].noGravity = true;
				Main.dust[num384].velocity *= 1.5f;
				Main.dust[num384].scale *= 0.9f;
			}
		}

	}
}
