using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HeartilleryMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aching Heart");
            Main.projFrames[base.projectile.type] = 9;
        }

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 28;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.sentry = true;
			projectile.ignoreWater = true;
			projectile.sentry = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
        bool spawnedHooks;
        public override void AI()
		{
            projectile.scale = 1f;
			//CONFIG INFO
			int range = 22;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 13.5f; //magnitude of the shoot vector (speed of arrows shot)
			int shootSpeed = 20;
            //TARGET NEAREST NPC WITHIN RANGE
            float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly) {
					//if npc is within 50 blocks
					float dist = projectile.Distance(npc.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
							projectile.netUpdate = true;
						}
					}
				}
			}

			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
																		 //firing
			projectile.ai[0]++;
			if (projectile.ai[0] >= 30 && projectile.ai[0] <= 60)
            {
                float num395 = Main.mouseTextColor / 200f - 0.35f;
                num395 *= 0.3f;
                projectile.scale = num395 + 0.85f;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 6f)
                {
                    projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                    projectile.frameCounter = 0;
                    if (projectile.frame >= 8 || projectile.frame < 4)
                    {
                        projectile.frame = 4;
                    }
                }
            }
			else
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 10f)
                {
                    projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
                    projectile.frameCounter = 0;
                    if (projectile.frame >= 3)
                    {
                        projectile.frame = 0;
                    }
                }
            }
			if (projectile.ai[0] >= 60 && target.active && !target.friendly && projectile.Distance(target.Center) / 16 < range) {
				projectile.ai[0] = 0;
				Vector2 ShootArea = new Vector2(projectile.Center.X, projectile.Center.Y - 5);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				direction.X *= shootVelocity/2.5f;
				direction.Y *= shootVelocity*3;
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 5, 0f, -2f, 0, default(Color), .85f);
                    Main.dust[num].noGravity = false;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    if (Main.dust[num].position != projectile.Center)
                    {
                        Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 5f;
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int z = 0; z < Main.rand.Next(2, 4); z++)
                    {
                        int proj2 = Projectile.NewProjectile(projectile.Center.X + 6, projectile.Center.Y + 3, direction.X, Main.rand.NextFloat(-10.75f, -9.5f), mod.ProjectileType("HeartilleryMinionClump"), projectile.damage, 0, Main.myPlayer);
                        Main.projectile[proj2].hostile = false;
                        Main.projectile[proj2].minion = true;
                        Main.projectile[proj2].friendly = true;
                    }
                }

                Main.PlaySound(SoundID.Item, projectile.Center, 95);  //make bow shooty sound
            }
            for (int index1 = 0; index1 < 3; ++index1)
            {
                float num1 = projectile.velocity.X * 0.2f * (float)index1;
                float num2 = (float)-((double)projectile.velocity.Y * 0.200000002980232) * (float)index1;
                int index2 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 5, 0.0f, 0.0f, 100, new Color(), 1.3f);
                Main.dust[index2].noGravity = false;
                Main.dust[index2].velocity.X *= 0.0f;
                Main.dust[index2].velocity.Y *= 0.5f;
                Main.dust[index2].scale *= 0.7f;
                Main.dust[index2].alpha = 100;
                Main.dust[index2].position.X -= num1;
                Main.dust[index2].position.Y -= num2;
            }
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 22);
			for (int i = 0; i < 10; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 5, 0f, -2f, 0, default(Color), .85f);
				Main.dust[num].noGravity = false;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center) {
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 1f;
				}
			}
		}
	}
}