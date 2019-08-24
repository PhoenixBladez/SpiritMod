using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class TankMinion : ModProjectile
	{
		string phase = "";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Tank");
			Main.projFrames[base.projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 54;
			projectile.height = 30;
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
			if (oldVelocity.X != projectile.velocity.X && phase == "moving")
				projectile.velocity.Y = -6.5f;

			return false;
		}

		public override void AI()
		{
			if (projectile.velocity.Y < 5)
				projectile.velocity.Y += 0.3f;
			
			if (projectile.velocity.Y > 5)
				projectile.velocity.Y = 5;
			
			bool flag64 = projectile.type == mod.ProjectileType("TankMinion");
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
			if (flag64)
			{
				if (player.dead)
					modPlayer.tankMinion = false;
				
				if (modPlayer.tankMinion)
					projectile.timeLeft =2;
				
			}

			#region moving
			if (phase == "moving")
			{
				projectile.spriteDirection = projectile.direction;
				projectile.frameCounter++;
				if (projectile.frameCounter >= 5)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
					if (projectile.frame >= 5)
					{
						projectile.frame = 0;
					}
				}
			}
			#endregion

			#region shooting
			if (phase == "shooting")
			{

				projectile.frameCounter++;
				if (projectile.frameCounter >= 8)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
					if (projectile.frame >= 8)
					{
						projectile.frame = 5;
					}
				}
			}
			#endregion

			int range = 100;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 18f;

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
			if (target.active && projectile.Distance(target.Center) / 16 < range)
			{
				if (Math.Abs(projectile.position.Y - target.position.Y) < 50 && projectile.Distance(target.Center) / 16 < (range / 2))
				{
					phase = "shooting";
					if (projectile.position.X - target.position.X > 0)
						projectile.velocity.X = -0.02f;
					else
						projectile.velocity.X = 0.02f;
					
					projectile.spriteDirection = projectile.direction;

					if (projectile.frame == 6 && projectile.frameCounter == 4)
					{
						projectile.frameCounter = 0;
						projectile.frame = 7;
						if (projectile.position.X - target.position.X > 0)
						{
							Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
							Vector2 direction = target.Center - ShootArea;
							direction.Normalize();
							direction.X *= shootVelocity;
							direction.Y *= shootVelocity;
							Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

							int proj2 = Projectile.NewProjectile(projectile.Center.X - 10, projectile.Center.Y - 8, direction.X, direction.Y, 134, projectile.damage, 0, Main.myPlayer);
						}
						else
						{
							Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
							Vector2 direction = target.Center - ShootArea;
							direction.Normalize();
							direction.X *= shootVelocity;
							direction.Y *= shootVelocity;
							Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

							int proj2 = Projectile.NewProjectile(projectile.Center.X + 10, projectile.Center.Y - 8, direction.X, direction.Y, 134, projectile.damage, 0, Main.myPlayer);
						}
					}
				}
				else
				{
					phase = "moving";
					
					if (projectile.position.X - target.position.X > 0)
						projectile.velocity.X = -2;
					else
						projectile.velocity.X = 2;
				}
			}
			else
			{
				phase = "idle";
				projectile.velocity.X = 0;
				projectile.frame = 1;
			}
		}

	}
}