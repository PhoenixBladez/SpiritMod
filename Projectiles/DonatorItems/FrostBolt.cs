using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	class FrostBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Bolt");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 360;
			projectile.height = 28;
			projectile.width = 12;
			projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Vector2 targetPos = projectile.Center;
			float targetDist = 350f;
			bool targetAcquired = false;

			//loop through first 200 NPCs in Main.npc
			//this loop finds the closest valid target NPC within the range of targetDist pixels
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].CanBeChasedBy(projectile) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
				{
					float dist = projectile.Distance(Main.npc[i].Center);
					if (dist < targetDist)
					{
						targetDist = dist;
						targetPos = Main.npc[i].Center;
						targetAcquired = true;
					}
				}
			}

			//change trajectory to home in on target
			if (targetAcquired)
			{
				float homingSpeedFactor = 6f;
				Vector2 homingVect = targetPos - projectile.Center;
				float dist = projectile.Distance(targetPos);
				dist = homingSpeedFactor / dist;
				homingVect *= dist;

				projectile.velocity = (projectile.velocity * 20 + homingVect) / 21f;
			}

			//Spawn the dust
			if (Main.rand.Next(11) == 0)
				Dust.NewDust(projectile.position + projectile.velocity,
					projectile.width, projectile.height,
					187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

			projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.Frostburn, 200, true);
		}

	}
}
