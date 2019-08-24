using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class FelShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reaper's Soul");
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.width = 6;
			projectile.height = 6;
			projectile.penetrate = 6;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.timeLeft = 400;
		}

		public override bool PreAI()
		{
			projectile.ai[1] += 1f;
			bool chasing = false;
			if (projectile.ai[1] >= 30f)
			{
				chasing = true;

				projectile.friendly = true;
				NPC target = null;
				if (projectile.ai[0] == -1f)
				{
					target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
				}
				else
				{
					target = Main.npc[(int)projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy())
						target = ProjectileExtras.FindRandomNPC(projectile.Center, 960f, false);
				}

				if (target == null)
				{
					chasing = false;
					projectile.ai[0] = -1f;
				}
				else
				{
					projectile.ai[0] = (float)target.whoAmI;
					ProjectileExtras.HomingAI(this, target, 10f, 5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing)
			{
				Vector2 dir = projectile.velocity;
				float vel = projectile.velocity.Length();
				if (vel != 0f)
				{
					if (vel < 4f)
					{
						dir *= 1 / vel;
						projectile.velocity += dir * 0.0625f;
					}
				}
				else
				{
					//Stops the projectiles from spazzing out
					projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}
			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = 1.2f;
			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
				vector *= 6f / magnitude;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("FelBrand"), 280);
		}


	}
}
