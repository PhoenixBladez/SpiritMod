using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class MythrilStaffProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Shot");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPC(projectile.damage / 2, 0f, 0, crit);
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

			//Create particles
			int i = Main.rand.Next(10);
			if (i < 5)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 83, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 83, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
				Main.dust[dust1].scale = 1f;
				Main.dust[dust1].noGravity = true;
				Main.dust[dust1].noLight = true;
			}
			return false;
		}

	}
}
