using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class Primavore : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primavore");
			Main.projFrames[base.projectile.type] = 7;
		}

		public override void SetDefaults()
		{
			projectile.width = 84;
			projectile.height = 76;
			projectile.timeLeft = 3000;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.minion = true;
			projectile.sentry = true;
			projectile.minionSlots = 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{


			/*projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }*/

			projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 50;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 18f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 50;

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
			#region frame
			Vector2 direction2 = target.Center - projectile.Center;
			float actualangle = ((float)Math.Atan(direction2.X / direction2.Y) * (float)(180.0 / Math.PI)) + 90f;
			// string SlopeText = actualangle.ToString();
			//Main.NewText(SlopeText, Color.Orange.R, Color.Orange.G, Color.Orange.B);
			if (actualangle < 11.5f)
				projectile.frame = 0;
			else if (actualangle < 22.5f)
				projectile.frame = 1;
			else if (actualangle < 67.5f)
				projectile.frame = 2;
			else if (actualangle < 112.5f)
				projectile.frame = 3;
			else if (actualangle < 157.5f)
				projectile.frame = 4;
			else if (actualangle < 168.5f)
				projectile.frame = 5;
			else
				projectile.frame = 6;
			#endregion

			if (projectile.ai[0] % shootSpeed == 4 && target.active && projectile.Distance(target.Center) / 16 < range)
			{
				Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 25);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				direction.X *= shootVelocity;
				direction.Y *= shootVelocity;

				int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, 483, projectile.damage, 0, Main.myPlayer);
				Main.PlaySound(2, projectile.Center, 5);
			}
		}

	}
}