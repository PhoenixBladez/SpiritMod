using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class TeslaTurret : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Turret");
			Main.projFrames[base.projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 56;
			projectile.timeLeft = 3000;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.sentry = true;
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
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}

			projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 30;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 20;

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(projectile))
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
																		 //firing
			projectile.ai[0]++;
			if (projectile.ai[0] % shootSpeed == 4 && target.active && projectile.Distance(target.Center) / 16 < range)
			{
				Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				direction.X *= shootVelocity;
				direction.Y *= shootVelocity;
				int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 25, direction.X, direction.Y, mod.ProjectileType("TeslaSpikeProjectile"), projectile.damage, 0, Main.myPlayer);
				Main.PlaySound(2, projectile.Center, 12);  //make bow shooty sound
			}
		}

	}
}