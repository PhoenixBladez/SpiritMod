using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class ShamanBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Healing Bolt");
		}
		int counter = -180;
		float distance = 2;
		int rotationalSpeed = 10;
		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.magic = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 200;
			projectile.height = 16;
			projectile.width = 16;
			projectile.alpha = 255;
			aiType = ProjectileID.Bullet;
			projectile.extraUpdates = 4;
		}
		bool summoned = false;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			distance += 0.015f;
			counter += rotationalSpeed;
			Vector2 initialSpeed = new Vector2(projectile.ai[0], projectile.ai[1]);
			Vector2 offset = initialSpeed.RotatedBy(Math.PI / 2);
			offset.Normalize();
			offset *= (float)(Math.Cos(counter * (Math.PI / 180)) * (distance / 3));
			projectile.velocity = initialSpeed + offset;

			if (!summoned) {
				for (int j = 0; j < 12; j++) {
					Vector2 vector2 = Vector2.UnitX * -projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default(Vector2)) * new Vector2(8f, 16f);
					vector2 = Utils.RotatedBy(vector2, (projectile.rotation - 1.57079637f), default(Vector2));
					int num8 = Dust.NewDust(projectile.Center, 0, 0, 272, 0f, 0f, 160, new Color(), 1f);
					// Main.dust[num8].scale = .48f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = projectile.Center + vector2;
					Main.dust[num8].velocity = projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(projectile.Center - projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
				summoned = true;
			}
			for (int i = 0; i < 6; i++) {
				Dust dust;
				// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
				Vector2 position = projectile.Center;
				dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, 272, 0f, 0f, 0, new Color(255, 255, 255), 0.3947368f)];
				dust.noLight = true;
				dust.velocity = Vector2.Zero;
			}

		}
	}
}
