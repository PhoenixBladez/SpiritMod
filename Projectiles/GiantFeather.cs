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
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		//Warning : it's not my code. It's SpiritMod code. so i donnt fully understand it
		public override void SetDefaults()
		{
			Projectile.width = 13;
			Projectile.height = 18;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 240;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreAI()
		{
			Projectile.ai[1] += 1f;
			bool chasing = false;
			if (Projectile.ai[1] >= 30f) {
				chasing = true;

				Projectile.friendly = true;
				NPC target = null;
				if (Projectile.ai[0] == -1f) {
					target = ProjectileExtras.FindRandomNPC(Projectile.Center, 960f, false);
				}
				else {
					target = Main.npc[(int)Projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy()) {
						target = ProjectileExtras.FindRandomNPC(Projectile.Center, 960f, false);
					}
				}

				if (target == null) {
					chasing = false;
					Projectile.ai[0] = -1f;
				}
				else {
					Projectile.ai[0] = (float)target.whoAmI;
					ProjectileExtras.HomingAI(this, target, 10f, 5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing) {
				Vector2 dir = Projectile.velocity;
				float vel = Projectile.velocity.Length();
				if (vel != 0f) {
					if (vel < 4f) {
						dir *= 1 / vel;
						Projectile.velocity += dir * 0.0625f;
					}
				}
				else {
					//Stops the projectiles from spazzing out
					Projectile.velocity.X += Main.rand.NextBool(2) ? 0.1f : -0.1f;
				}
			}

			int num = 5;
			for (int k = 0; k < 3; k++) {
				int index2 = Dust.NewDust(Projectile.position, 1, 1, DustID.DungeonWater, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = Projectile.Center - Projectile.velocity / num * (float)k;
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
