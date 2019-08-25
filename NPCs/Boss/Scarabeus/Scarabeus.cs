using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	[AutoloadBossHead]
	public class Scarabeus : ModNPC
	{
		public static int _type;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 120f;
		private float SpeedMax = 33f;
		private float SpeedDistanceIncrease = 500f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarabeus");
			Main.npcFrameCount[npc.type] = 10;
		    NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 70;
			npc.value = 5000;
			npc.damage = 26;
			npc.defense = 10;
			npc.lifeMax = 1700;
			npc.knockBackResist = 0f;
			npc.boss = true;
			npc.npcSlots = 10f;
			npc.HitSound = SoundID.NPCHit31;
			npc.DeathSound = SoundID.NPCDeath5;
			bossBag = mod.ItemType("BagOScarabs");
		}
		private int Counter;
		float frametimer = .25f;
		bool trailbehind;
		int frame = 0;
		int timer = 0;

		bool wormAI = false;
		bool charge;
		bool jump;
		int npcCounter;		
		int jumpstacks;
		
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			bool rage = (double)npc.life <= (double)npc.lifeMax * 0.2;
			if (rage)
				SpeedMax = 40f;

			if (Main.rand.Next(500) == 0)
				Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 44);
		
			Counter++;
			if (wormAI)
			{
				{
						trailbehind = true;
						npc.netUpdate = true;
						npc.noTileCollide = true;
						npc.behindTiles = true;
						npc.noGravity = true;
						int minTilePosX = (int)(npc.position.X / 16.0) - 1;
						int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 2;
						int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
						int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0) + 2;
						if (minTilePosX < 0)
							minTilePosX = 0;
						if (maxTilePosX > Main.maxTilesX)
							maxTilePosX = Main.maxTilesX;
						if (minTilePosY < 0)
							minTilePosY = 0;
						if (maxTilePosY > Main.maxTilesY)
							maxTilePosY = Main.maxTilesY;

						bool collision = false;
						for (int i = minTilePosX; i < maxTilePosX; ++i)
						{
							for (int j = minTilePosY; j < maxTilePosY; ++j)
							{
								if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64) || Main.tile[i, j].type == TileID.Sand)
								{
									Vector2 vector2;
									vector2.X = (float)(i * 16);
									vector2.Y = (float)(j * 16);
									if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
									{
										collision = true;
										if (Main.rand.Next(100) == 0 && Main.tile[i, j].nactive())
											WorldGen.KillTile(i, j, true, true, false);
									}
								}
							}
						}
						if (!collision)
						{
							Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
							int maxDistance = 1000;
							bool playerCollision = true;
							for (int index = 0; index < 255; ++index)
							{
								if (Main.player[index].active)
								{
									Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
									if (rectangle1.Intersects(rectangle2))
									{
										playerCollision = false;
										break;
									}
								}
							}
							if (playerCollision)
								collision = true;
						}

						float speed = 12f;
						float acceleration = 0.325f;

						Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
						float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

						float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
						float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
						npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
						npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
						float dirX = targetRoundedPosX - npcCenter.X;
						float dirY = targetRoundedPosY - npcCenter.Y;

						float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
						if (!collision)
						{
							npc.TargetClosest(true);
							npc.velocity.Y = npc.velocity.Y + 0.17f;
							if (npc.velocity.Y > speed)
								npc.velocity.Y = speed;
							if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
							{
								if (npc.velocity.X < 0.0)
									npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
								else
									npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
							}
							else if (npc.velocity.Y == speed)
							{
								if (npc.velocity.X < dirX)
									npc.velocity.X = npc.velocity.X + acceleration;
								else if (npc.velocity.X > dirX)
									npc.velocity.X = npc.velocity.X - acceleration;
							}
							else if (npc.velocity.Y > 4.0)
							{
								if (npc.velocity.X < 0.0)
									npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
								else
									npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
							}
						}
						else
						{
							if (npc.soundDelay == 0)
							{
								float num1 = length / 40f;
								if (num1 < 10.0)
									num1 = 10f;
								if (num1 > 20.0)
									num1 = 20f;
								npc.soundDelay = (int)num1;
								Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
							}
							float absDirX = Math.Abs(dirX);
							float absDirY = Math.Abs(dirY);
							float newSpeed = speed / length;
							dirX = dirX * newSpeed;
							dirY = dirY * newSpeed;
							if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || (npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0))
							{
								if (npc.velocity.X < dirX)
									npc.velocity.X = npc.velocity.X + acceleration;
								else if (npc.velocity.X > dirX)
									npc.velocity.X = npc.velocity.X - acceleration;
								if (npc.velocity.Y < dirY)
									npc.velocity.Y = npc.velocity.Y + acceleration;
								else if (npc.velocity.Y > dirY)
									npc.velocity.Y = npc.velocity.Y - acceleration;
								if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
								{
									if (npc.velocity.Y > 0.0)
										npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
									else
										npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
								}
								if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
								{
									if (npc.velocity.X > 0.0)
										npc.velocity.X = npc.velocity.X + acceleration * 2f;
									else
										npc.velocity.X = npc.velocity.X - acceleration * 2f;
								}
							}
							else if (absDirX > absDirY)
							{
								if (npc.velocity.X < dirX)
									npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
								else if (npc.velocity.X > dirX)
									npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
								if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
								{
									if (npc.velocity.Y > 0.0)
										npc.velocity.Y = npc.velocity.Y + acceleration;
									else
										npc.velocity.Y = npc.velocity.Y - acceleration;
								}
							}
							else
							{
								if (npc.velocity.Y < dirY)
									npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
								else if (npc.velocity.Y > dirY)
									npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;
								if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
								{
									if (npc.velocity.X > 0.0)
										npc.velocity.X = npc.velocity.X + acceleration;
									else
										npc.velocity.X = npc.velocity.X - acceleration;
								}
							}
						}
						npc.rotation = npc.velocity.X * .06f;

						if (collision)
						{
							if (npc.localAI[0] != 1)
								npc.netUpdate = true;
							npc.localAI[0] = 1f;
						}
						else
						{
							if (npc.localAI[0] != 0.0)
								npc.netUpdate = true;
							npc.localAI[0] = 0.0f;
						}
						if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || (npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
							npc.netUpdate = true;
					}	
				timer++;
				if(timer >= 4)
				{
					frame++;
					timer = 0;
				}
				if(frame >= 10)
				{
					frame = 5;
				}			
			}
			if (npc.life >= 600)
			{
				{
					npc.TargetClosest(true);
					if (!player.ZoneDesert)
					{
						Counter = 601;
						npc.damage = 100;
						npcCounter = 1010;
					}
					if (player.dead)
					{				
						npc.active = false;
					}
					if (Counter == 60)
					{
						npc.velocity.Y = 0f;
						npc.velocity.X = 0f;
						npc.ai[0] = -180f;
						npc.ai[3] = 0f;
						npc.ai[1] = 1f;
						npc.netUpdate = true;					
					}		
					if (Counter >= 360 && Counter <= 480)
					{
						trailbehind = true;
						wormAI = true;
						Vector2 position = npc.Center + Vector2.Normalize(npc.velocity) * 14;

						Dust newDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 0, 0f, 0f, 0, default(Color), 1f)];
						newDust.position = position;
						newDust.velocity = npc.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + npc.velocity / 4;
						newDust.position += npc.velocity.RotatedBy(Math.PI / 2, default(Vector2));
						newDust.fadeIn = 0.5f;
						newDust.noGravity = true;
						newDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 0, 0f, 0f, 0, default(Color), 1)];
						newDust.position = position;
						newDust.velocity = npc.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + npc.velocity / 4;
						newDust.position += npc.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
						newDust.fadeIn = 0.5F;
						newDust.noGravity = true;
						if (Counter == 600)
						{
							Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
						}
					}		
					if (Counter >= 600 && Counter <= 720)
					{
						npc.rotation *= 0f;
						wormAI = false;
						Vector2 position = npc.Center + Vector2.Normalize(npc.velocity) * 14;

						Dust newDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 0, 0f, 0f, 0, default(Color), 1f)];
						newDust.position = position;
						newDust.velocity = npc.velocity.RotatedBy(Math.PI / 2, default(Vector2)) * 0.33F + npc.velocity / 4;
						newDust.position += npc.velocity.RotatedBy(Math.PI / 2, default(Vector2));
						newDust.fadeIn = 0.5f;
						newDust.noGravity = true;
						newDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 0, 0f, 0f, 0, default(Color), 1)];
						newDust.position = position;
						newDust.velocity = npc.velocity.RotatedBy(-Math.PI / 2, default(Vector2)) * 0.33F + npc.velocity / 4;
						newDust.position += npc.velocity.RotatedBy(-Math.PI / 2, default(Vector2));
						newDust.fadeIn = 0.5F;
						newDust.noGravity = true;				
						if (Counter == 600)
						{
							Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
						}
						trailbehind = true;
						SpeedMax = 65f;
						npc.netUpdate = true;
					}
					if (Counter >= 721 && Counter <= 999)
					{
						npc.noTileCollide = false;
						npc.velocity.Y = npc.ai[0] * .9f;
						trailbehind = true;
						npc.velocity.X = .00000001f;
						for (int i = 0; i < 10; i++)
						{
							int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 10), npc.width, npc.height, 5, npc.velocity.X * 0f, 1.5f);
							Main.dust[dust].scale *= Main.rand.NextFloat(.2f, .8f);
						}
						if (Counter == 724 || Counter == 784 || Counter == 800 || Counter == 823 || Counter == 841 || Counter == 861 || Counter == 881)
						{
							jumpstacks += 2;
							npc.ai[0] = -10;
						}
						if (Counter == 729 || Counter == 790 || Counter == 806 || Counter == 829 || Counter == 846 || Counter == 867 || Counter == 889)
						{
							npc.ai[0] = 60;
						}
						if (Counter == 780)
						{
							NPC.NewNPC((int)npc.Center.X+ Main.rand.Next(-20, 20), (int)npc.Center.Y + Main.rand.Next(-20, 20), mod.NPCType("ChildofScarabeus"));
						}		
					}
					if (Counter >= 1000)
					{
						npc.ai[0] = 0;
						Counter = 0;
					}
					if (Counter < 600)
					{
						trailbehind = false;
						SpeedMax = 33f;
					}
					int distance = (int)Math.Abs((player.Center - npc.Center).X);
					if (distance >= 500 || distance <= -500)
					{
						if (Main.rand.Next(400) == 0)
						{
							NPC.NewNPC((int)npc.Center.X+ Main.rand.Next(-20, 20), (int)npc.Center.Y + Main.rand.Next(-20, 20), mod.NPCType("ChildofScarabeus"));
							Counter = 479;
						}
					}
					if (npc.collideY && jump && npc.velocity.Y > 1)
					{
						jump = false;
						if (jumpstacks >= 3)
						{
							Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 34);
							int damage = expertMode ? 9 : 19;
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 22, 7, 0, mod.ProjectileType("DustTornado"), damage, 1, Main.myPlayer, 0, 0);
							Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 22, -7, 0, mod.ProjectileType("DustTornado"), damage, 1, Main.myPlayer, 0, 0);
							jumpstacks = 0;
						}
						for (int i = 0; i < 40; i++)
						{
							int dust = Dust.NewDust(npc.position, npc.width, npc.height, 0, npc.velocity.X * 0f, npc.velocity.Y * -0.25f);
							Main.dust[dust].scale *= Main.rand.NextFloat(.2f, .8f);
						}
					}
					if (!npc.collideY)
					{
						jump = true;	
					}		
					if (Counter < 720)
					{
						if (npc.ai[1] == 0f && !wormAI)
						{
							if (npc.Center.X >= player.Center.X && npc.ai[2] >= (0f - SpeedMax))
							{
								for (npc.ai[3] = 0f; npc.ai[3] < Math.Abs(npc.Center.X - player.Center.X); npc.ai[3] = npc.ai[3] + SpeedDistanceIncrease)
								{
									npc.ai[2] -= rage ? 4f : 2f;
								}
								npc.ai[2] -= rage ? 4f : 2f;
							}
							if (npc.Center.X <= player.Center.X && npc.ai[2] <= SpeedMax)
							{
								for (npc.ai[3] = 0f; npc.ai[3] < Math.Abs(npc.Center.X - player.Center.X); npc.ai[3] = npc.ai[3] + SpeedDistanceIncrease)
								{
									npc.ai[2] += rage ? 4f : 2f;
								}
								npc.ai[2] += rage ? 4f : 2f;
							}
							if (npc.ai[0] < 100f)
								npc.ai[0] += expertMode ? 3f : 2f;
									npc.noGravity = false;
									npc.noTileCollide = false;
							
							if (Main.rand.Next(2) > 0)
								npc.velocity.X = npc.ai[2] * 0.1f;

							npc.velocity.Y = npc.ai[0] * 0.26f;
							if (npc.velocity.X == 0f && Main.rand.Next(3) == 0 && npc.collideY)
							{
								npc.ai[0] = -40f;
								npc.noTileCollide = true;
							}
						}
						else if (npc.ai[1] == 1f)
						{
							trailbehind = true;
							frame = 0;
							npc.noGravity = true;
							npc.noTileCollide = true;
							npc.ai[0] += 4f;
							if (npc.Center.X >= player.Center.X && npc.ai[2] >= 0f - SpeedMax * 2.63f) // flies to players x position
								npc.ai[2] -= expertMode ? 3f : 2f;

							if (npc.Center.X <= player.Center.X && npc.ai[2] <= SpeedMax * 2.63f)
								npc.ai[2] += expertMode ? 3f : 2f;

							npc.velocity.Y = npc.ai[0] * 0.15f;
							npc.velocity.X = npc.ai[2] * 0.1f;
							if (Math.Abs(npc.Center.X - player.Center.X) < 10)
							{
								trailbehind = true;
								npc.velocity.Y = 0f;
								npc.velocity.X = 0f;
								npc.ai[0] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] = 0f;
								npc.ai[1] = 2f;
								npc.netUpdate = true;
							}
							if (npc.ai[0] > 0f)
							{
								npc.ai[3] = 0f;
								npc.ai[1] = 0f;
								npc.netUpdate = true;
							}
							
						}
						else if (npc.ai[1] == 2f)
						{
							npc.ai[1] = 0f;
							trailbehind = true;
							npc.velocity.X = 0f;
							npc.noGravity = false;
							npc.noTileCollide = false;
							npc.ai[0] += 30f;
							npc.velocity.Y = npc.ai[0] * 3.1f;
							if (Math.Abs(npc.Center.Y - player.Center.Y) < 0 || npc.ai[0] > 240f)
							{
								npc.velocity.X = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] = 0f;
								npc.ai[1] = 0f;
								npc.netUpdate = true;
								if (npc.collideY)
								{
									jumpstacks = 3;
								}
							}
						}
						else if (npc.ai[1] == 3f)
						{
							trailbehind = false;
							npc.damage = 0;
							npc.defense = 9999;
							npc.noTileCollide = true;
							npc.alpha += 7;
							if (npc.alpha > 255)
							{
								npc.alpha = 255;
							}
							npc.velocity.X = npc.velocity.X * 0.98f;
						}
						if (npc.ai[1] == 2f || Counter <= 720 || Counter >= 1000)
						{
							if (!wormAI)
							{
								timer++;
								if(timer >= 5)
								{
									frame++;
									timer = 0;
								}
								if(frame >= 5)
								{
									frame = 0;
								}
							}
						}
					}
				}
			}
			else if (npc.life <= 600)
			{
				npc.noGravity = true;
				npcCounter++;
				if (npcCounter == 300 || npcCounter == 450 ||npcCounter == 600)
				{
					Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
					NPC.NewNPC((int)npc.Center.X+ Main.rand.Next(-20, 20), (int)npc.Center.Y + Main.rand.Next(-20, 20), mod.NPCType("ChildofScarabeus"));	
				}
				if (npcCounter >= 1000 && npcCounter <= 1030 || npcCounter == 1200  && npcCounter <= 1230 || npcCounter == 1400  && npcCounter <= 1430 || npcCounter == 1600  && npcCounter <= 1630 || npcCounter == 1800  && npcCounter <= 1830 || npcCounter == 2000  && npcCounter <= 2030 ||  npcCounter == 2200  && npcCounter <= 2230)
				{
					charge = true;
					Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
					HomeY = -25f;				
					npc.rotation = npc.velocity.X * .05f;
				}
				else
				{
					charge = false;
				}
				if (npcCounter >= 2240)
				{
					npcCounter = 0;
				}
				npc.noTileCollide = true;
				Sandstorm.Happening = true;
				Sandstorm.TimeLeft = 25200;
				Sandstorm.Severity  = .8f;				
				trailbehind = true;
				timer++;
				if(timer >= 4)
				{
					frame++;
					timer = 0;
				}
				if(frame >= 10)
				{
					frame = 5;
				}
				if (!charge)
				{
					if (npc.Center.X >= player.Center.X && moveSpeed >= -40) // flies to players x position
					{
						moveSpeed--;
					}

					if (npc.Center.X <= player.Center.X && moveSpeed <= 40)
					{
						moveSpeed++;
					}

					npc.velocity.X = moveSpeed * 0.1f;

					if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -27) //Flies to players Y position
					{
						moveSpeedY--;
						HomeY = 160f;
					}

					if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 27)
					{
						moveSpeedY++;
					}
					
					npc.velocity.Y = moveSpeedY * 0.12f;
				}
				else
				{
					if (npc.Center.X >= player.Center.X && moveSpeed >= -80) // flies to players x position
					{
						moveSpeed--;
					}

					if (npc.Center.X <= player.Center.X && moveSpeed <= 80)
					{
						moveSpeed++;
					}

					npc.velocity.X = moveSpeed * 0.52f;

					if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -38) //Flies to players Y position
					{
						moveSpeedY--;
						HomeY = 160f;
					}

					if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= -38)
					{
						moveSpeedY++;
					}
					
					npc.velocity.Y = moveSpeedY * 0.40f;					
				}				
			}
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			if (trailbehind)
			{
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height/ Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++)
				{
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
            return false;
        }
       	public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Scarabeus/Scarabeus_Glow"));			
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 100;
				npc.height = 60;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 30; num621++)
				{
					int randomDustType = Main.rand.Next(3);
					if (randomDustType == 0)
						randomDustType = 5;
					else if (randomDustType == 1)
						randomDustType = 36;
					else
						randomDustType = 32;

					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
					}
				}
				for (int num623 = 0; num623 < 50; num623++)
				{
					int randomDustType = Main.rand.Next(3);
					if (randomDustType == 0)
						randomDustType = 5;
					else if (randomDustType == 1)
						randomDustType = 36;
					else
						randomDustType = 32;

					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), .82f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override bool PreNPCLoot()
		{
			MyWorld.downedScarabeus = true;
			return true;
		}

		public override void NPCLoot()
		{
			Sandstorm.Happening = false;
			if (Main.expertMode)
			{
				npc.DropBossBags();
				return;
			}

			npc.DropItem(mod.ItemType("Chitin"), 25, 36);
			npc.DropItem(mod.ItemType("GildedIdol"), 1f / 9);

			string[] lootTable = { "ScarabBow", "OrnateStaff", "ScarabSword" };
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(mod.ItemType(lootTable[loot]));

			npc.DropItem(Items.Armor.Masks.ScarabMask._type, 1f / 7);
			npc.DropItem(Items.Boss.Trophy1._type, 1f / 10);
		}
	}
}