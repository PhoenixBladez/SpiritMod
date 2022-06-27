﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class BaneBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 6;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 10000;
			Projectile.extraUpdates = 1;
		}

		bool summoned = false;
		public override void AI()
		{
			Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.3F, 0.0F, 0.5F);
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (!summoned) {
				for (int j = 0; j < 24; j++) {
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation - 1.57079637f), default) * 1.3f;
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 160, new Color(), 1f);
					Main.dust[num8].scale = 1.5f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
				summoned = true;
			}
            for (int i = 0; i < 10; i++)
            {
                float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)i;
                float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)i;
                int num = Dust.NewDust(new Vector2(x, y), 2, 2, DustID.Shadowflame);
                Main.dust[num].alpha = Projectile.alpha;
                Main.dust[num].velocity = Vector2.Zero;
                Main.dust[num].noGravity = true;
            }
        }

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y, 1);

			for (int num383 = 0; num383 < 5; num383++) {
				int num384 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch);
				Main.dust[num384].noGravity = true;
				Main.dust[num384].velocity *= 1.5f;
				Main.dust[num384].scale *= 0.9f;
			}
		}

	}
}
