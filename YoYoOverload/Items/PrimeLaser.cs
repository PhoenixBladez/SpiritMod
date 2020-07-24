using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items
{
	public class PrimeLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prime Laser");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.melee = true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter % 120 == 0) {
				Main.rand.Next(0, 361);
				Vector2 vector = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
				int num = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector.X, vector.Y, ProjectileID.DeathLaser, projectile.damage, projectile.owner, 0, 0f, 0f);
				Main.projectile[num].friendly = true;
				Main.projectile[num].hostile = false;
				Main.projectile[num].velocity *= 18f;
			}
			if (!Main.projectile[(int)projectile.localAI[0]].active) {
				projectile.Kill();
			}
			int num2 = (int)projectile.velocity.X * 10;
			int num3 = (int)projectile.velocity.Y + 1;
			int num4 = 80 - num2;
			int num5 = 12 - num3;
			int num6 = 16;
			projectile.localAI[1] += 0.0104719754f * num5;
			projectile.localAI[1] %= 6.28318548f;
			Vector2 center = Main.projectile[(int)projectile.localAI[0]].Center;
			center.X -= num6;
			projectile.rotation = (float)Math.Atan2(center.Y, center.X) - 2f;
			projectile.Center = center + num4 * new Vector2((float)Math.Cos(projectile.localAI[1]), (float)Math.Sin(projectile.localAI[1]));
		}

		private static Vector2 GetVelocity(Projectile projectile)
		{
			float num = 400f;
			Vector2 velocity = projectile.velocity;
			Vector2 vector = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			vector.Normalize();
			Vector2 vector2 = vector * ((float)Main.rand.Next(10, 41) * 0.1f);
			if (Main.rand.Next(3) == 0) {
				vector2 *= 2f;
			}
			Vector2 vector3 = velocity * 0.25f + vector2;
			for (int i = 0; i < 200; i++) {
				if (Main.npc[i].CanBeChasedBy(projectile, false)) {
					float num2 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
					float num3 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
					float num4 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num2) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num3);
					if ((double)num4 < (double)num && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height)) {
						num = num4;
						vector3.X = num2;
						vector3.Y = num3;
						Vector2 vector4 = vector3 - projectile.Center;
						vector4.Normalize();
						vector3 = vector4 * 8f;
					}
				}
			}
			return vector3 * 0.8f;
		}
	}
}
