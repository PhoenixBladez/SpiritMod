using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class ProbeP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;

		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(549);
			Projectile.damage = 52;
			Projectile.extraUpdates = 3;
			AIType = 549;
		}

		public override void PostAI()
		{
			Projectile.rotation -= 10f;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 30) {
				Projectile.frameCounter = 0;
				float num = 8000f;
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
						int p = Terraria.Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ProjectileID.DeathLaser, 30, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
						Main.projectile[p].friendly = true;
						Main.projectile[p].hostile = false;
					}
				}
			}
		}
	}
}
