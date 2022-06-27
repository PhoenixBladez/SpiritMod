using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class FireBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 340;
			Projectile.height = 8;
			Projectile.width = 8;
			Projectile.alpha = 255;
			AIType = ProjectileID.Bullet;
			Projectile.extraUpdates = 1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			if (!Main.projectile[(int)Projectile.localAI[0]].active)
				Projectile.Kill();

			int num = (int)Projectile.velocity.X * 16;
			int num2 = (int)Projectile.velocity.Y;
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 120) {
				Projectile.frameCounter = 0;
				if (Projectile.frame == 5)
					Projectile.frame = 0;
				else
					Projectile.frame++;
			}

			int num3 = 80 - num;
			int num4 = 12 - num2;
			int num5 = 16;
			Projectile.localAI[1] += 0.0104719754f * (float)num4;
			Projectile.localAI[1] %= 6.28318548f;
			Vector2 center = Main.projectile[(int)Projectile.localAI[0]].Center;
			center.X -= (float)num5;
			Projectile.rotation = (float)Math.Atan2((double)center.Y, (double)center.X) - 2f;
			Projectile.Center = center + (float)num3 * new Vector2((float)Math.Cos((double)Projectile.localAI[1]), (float)Math.Sin((double)Projectile.localAI[1]));
		}

		private static Vector2 GetVelocity(Projectile projectile)
		{
			float num = 400f;
			Vector2 velocity = projectile.velocity;
			Vector2 vector = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
			vector.Normalize();
			Vector2 vector2 = vector * ((float)Main.rand.Next(10, 41) * 0.1f);
			if (Main.rand.Next(3) == 0)
				vector2 *= 2f;

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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(10) == 2)
				target.AddBuff(BuffID.OnFire, 180);
		}
	}
}
