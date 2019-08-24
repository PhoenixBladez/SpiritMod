using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class TwinklePopperMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twinkle Popper");
			Main.projFrames[base.projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.sentry = true;
			projectile.timeLeft = 3000;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.minion = true;
			projectile.minionSlots = 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{
			projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 50;   //How many tiles away the projectile targets NPCs
			int animSpeed = 2;  //how many game frames per frame :P note: firing anims are twice as fast currently
			int targetingMax = 15; //how many frames allowed to target nearest instead of shooting
			float shootVelocity = 6f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (NPC npc in Main.npc)
			{
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && !npc.friendly && npc.catchItem == 0)
				{
					//if npc is within 50 blocks
					float dist = projectile.Distance(npc.Center);
					if (dist / 16 < range)
					{
						//if npc is closer than closest found npc
						if (dist < lowestDist)
						{
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
						}
					}
				}
			}

			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
			if (projectile.frame < 5)
			{
				//do nuffin... until target in range
				if (target.active && projectile.Distance(target.Center) / 16 < range)
				{
					projectile.frameCounter++;
					//proceed if rotated in the right direction
					if (projectile.rotation == projectile.DirectionTo(target.position).ToRotation() && projectile.frameCounter % 2 == 1)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					//proceed if still haven't locked on (targets change too quickly, etc)
					else if (projectile.frameCounter >= targetingMax)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
				}
				else
					projectile.frameCounter = 0;
			}
			//firing
			else if (projectile.frame == 5)
			{
				projectile.frameCounter++;
				//fire!!
				if (projectile.frameCounter % animSpeed == 0)
				{
					//spawn the arrow centered on the bow (this code aligns the centers :3)
					Vector2 vel = projectile.DirectionTo(target.Center);
					Vector2 vel7 = new Vector2(-1, 0);
					vel7 *= shootVelocity;
					vel7 = vel7.RotatedBy(System.Math.PI / 13);
					for (int K = 0; K < 18; K++)
					{
						vel7 = vel7.RotatedBy(System.Math.PI / 13);
						int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel7.X, vel7.Y, 538, projectile.damage, 0, projectile.owner);
						Projectile newProj2 = Main.projectile[proj2];
						newProj2.friendly = true;
						newProj2.hostile = false;
						projectile.frame = 1;
					}

					Main.PlaySound(2, projectile.Center, 5);  //make bow shooty sound

					projectile.frame++;
				}
			}

			//finish firing anim, revert back to 0
			if (projectile.frame == 6)
			{
				projectile.frame = 1;
				projectile.frameCounter = 0;
			}
		}

	}
}