using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Yoyo
{
	public class Typhoon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Typhoon");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(555);
			Projectile.extraUpdates = 1;
			AIType = 555;
		}

		public override void AI()
		{
			Projectile.rotation -= 10f;
		}

		public override bool PreAI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 60) {
				Projectile.frameCounter = 0;
				float num = 2000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++) {
					float num3 = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 640f && Main.npc[i].CanBeChasedBy(Projectile, false)) {
						num2 = i;
						num = num3;
					}
				}

				if (num2 != -1) {
					bool flag = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag) {
						Vector2 value = Main.npc[num2].Center - Projectile.Center;
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4) {
							num5 = num4 / num5;
						}
						value *= num5;
						Terraria.Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ProjectileID.MiniSharkron, Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
					}
				}
			}
			return true;
		}

	}
}
