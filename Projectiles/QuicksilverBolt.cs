using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class QuicksilverBolt : ModProjectile
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Droplet");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 210;
			projectile.extraUpdates = 0;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			if (projectile.ai[1] == 0)
				projectile.ai[0] = -1;

			projectile.ai[1] += 1f;
			bool chasing = false;
			if (projectile.ai[1] >= 30f)
			{
				chasing = true;

				projectile.friendly = true;
				NPC target = null;
				if (projectile.ai[0] < 0 || projectile.ai[0] >= Main.maxNPCs)
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
					ProjectileExtras.HomingAI(this, target, 10f, .5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing)
			{
				Vector2 dir = projectile.velocity;
				float vel = projectile.velocity.Length();
				if (vel != 0f)
				{
					if (vel < 8f)
					{
						dir *= 1 / vel;
						projectile.velocity += dir * 0.0625f;
					}
				}
				else
				{
					//Stops the projectile from spazzing out
					projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}

			for (int i = 0; i < 10; i++)
			{
				Vector2 pos = projectile.Center - projectile.velocity * ((float)i / 10f);
				int num = Dust.NewDust(pos, 2, 2, DustID.SilverCoin);
				Main.dust[num].alpha = projectile.alpha;
				//Main.dust[num].position = pos;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			return projectile.ai[1] < 30 ? false : (bool?)null;
		}

	}
}
