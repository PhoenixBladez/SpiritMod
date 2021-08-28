using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class GiantFeather : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Giant Feather");
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		//Warning : it's not my code. It's SpiritMod code. so i donnt fully understand it
		public override void SetDefaults()
		{
			projectile.width = 13;
			projectile.height = 18;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 240;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreAI()
		{
			projectile.ai[1] += 1f;
			bool chasing = false;
			if (projectile.ai[1] >= 30f) {
				chasing = true;

				projectile.friendly = true;
				NPC target = null;
				if (projectile.ai[0] == -1f) {
					target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
				}
				else {
					target = Main.npc[(int)projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy()) {
						target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
					}
				}

				if (target == null) {
					chasing = false;
					projectile.ai[0] = -1f;
				}
				else {
					projectile.ai[0] = (float)target.whoAmI;
					ProjectileExtras.HomingAI(this, target, 10f, 5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing) {
				Vector2 dir = projectile.velocity;
				float vel = projectile.velocity.Length();
				if (vel != 0f) {
					if (vel < 4f) {
						dir *= 1 / vel;
						projectile.velocity += dir * 0.0625f;
					}
				}
				else {
					//Stops the projectiles from spazzing out
					projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}

			int num = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, DustID.DungeonWater, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .5f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}

			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
				vector *= 6f / magnitude;
		}


	}
}
