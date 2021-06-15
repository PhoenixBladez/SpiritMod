using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.Mangrove_Defender
{
	public class Mangrove_Defender : ModNPC
	{

		public bool groundSlamming;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Defender");
			Main.npcFrameCount[npc.type] = 14;
			NPCID.Sets.TrailCacheLength[npc.type] = 20; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 240;
			npc.defense = 25;
			npc.value = 890f;
			aiType = 0;
			npc.knockBackResist = 0.1f;
			npc.width = 30;
			npc.height = 62;
			npc.damage = 45;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(6, 1);
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 6);
		}
		/*public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (player.HeldItem.axe > 0)		
				damage*=2;
		}*/
		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
				
			npc.spriteDirection = npc.direction;
			
			if (Vector2.Distance(npc.Center, player.Center) < 600f && !groundSlamming)
			{
				npc.ai[1]++;
			}
			if (npc.ai[1] >= 280)	
			{
				groundSlamming = true;
				slam();
			}
			else
			{
				movement();
				groundSlamming = false;
			}
		}
		public void slam()
		{
			Player player = Main.player[npc.target];
			npc.ai[1]++;
			npc.velocity.X = 0f;
			if (npc.ai[1] == 280)
			{
				npc.frameCounter = 0;				
				npc.netUpdate = true;
			}
			if (npc.ai[1] == 281)
				Main.PlaySound(42, (int)npc.position.X, (int)npc.position.Y, 143, 1f, -0.9f);
			if (npc.ai[1] >= 321)
			{
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 4f * npc.spriteDirection, 0f, mod.ProjectileType("Earth_Slam_Invisible"), 0, 0, player.whoAmI);
				npc.ai[1] = 0;
				npc.netUpdate = true;
			}
		}
		public void movement()
		{
			Player player = Main.player[npc.target];
			int num1 = 30;
			int num2 = 10;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			if ((double)npc.velocity.Y == 0.0 && ((double)npc.velocity.X > 0.0 && npc.direction < 0 || (double)npc.velocity.X < 0.0 && npc.direction > 0))
			{
				flag2 = true;
				++npc.ai[3];
			}
			if ((double)npc.position.X == (double)npc.oldPosition.X || (double)npc.ai[3] >= (double)num1 || flag2)
			{
				++npc.ai[3];
				flag3 = true;
			}
			else if ((double)npc.ai[3] > 0.0)
			{
				--npc.ai[3];
			}

			if ((double)npc.ai[3] > (double)(num1 * num2))
			{
				npc.ai[3] = 0.0f;
			}

			if (npc.justHit)
			{
				npc.ai[3] = 0.0f;
			}

			if ((double)npc.ai[3] == (double)num1)
			{
				npc.netUpdate = true;
			}

			Vector2 vector2_1 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num3 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector2_1.X;
			float num4 = Main.player[npc.target].position.Y - vector2_1.Y;
			float num5 = (float)Math.Sqrt((double)num3 * (double)num3 + (double)num4 * (double)num4);
			if ((double)num5 < 200.0 && !flag3)
			{
				npc.ai[3] = 0.0f;
			}

			if ((double)npc.ai[3] < (double)num1)
			{
				npc.TargetClosest(true);
			}
			else
			{
				if ((double)npc.velocity.X == 0.0)
				{
					if ((double)npc.velocity.Y == 0.0)
					{
						++npc.ai[0];
						if ((double)npc.ai[0] >= 2.0)
						{
							npc.direction *= -1;

							npc.ai[0] = 0.0f;
						}
					}
				}
				else
				{
					npc.ai[0] = 0.0f;
				}

				npc.directionY = -1;
				if (npc.direction == 0)
				{
					npc.direction = 1;
				}
			}
			float num6 = 2.5f; //walking speed
			float num7 = 1.39f; //regular speed (x)
			if (!flag1 && ((double)npc.velocity.Y == 0.0 || npc.wet || (double)npc.velocity.X <= 0.0 && npc.direction < 0 || (double)npc.velocity.X >= 0.0 && npc.direction > 0))
			{
				if ((double)npc.velocity.X < -(double)num6 || (double)npc.velocity.X > (double)num6)
				{
					if ((double)npc.velocity.Y == 0.0)
					{
						Vector2 vector2_2 = npc.velocity * 0.5f; ///////SLIDE SPEED
						npc.velocity = vector2_2;
					}
				}
				else if ((double)npc.velocity.X < (double)num6 && npc.direction == 1)
				{
					npc.velocity.X += num7;
					if ((double)npc.velocity.X > (double)num6)
					{
						npc.velocity.X = num6;
					}
				}
				else if ((double)npc.velocity.X > -(double)num6 && npc.direction == -1)
				{
					npc.velocity.X -= num7;
					if ((double)npc.velocity.X < -(double)num6)
					{
						npc.velocity.X = -num6;
					}
				}
			}
			if ((double)npc.velocity.Y >= 0.0)
			{
				int num8 = 0;
				if ((double)npc.velocity.X < 0.0)
				{
					num8 = -1;
				}

				if ((double)npc.velocity.X > 0.0)
				{
					num8 = 1;
				}

				Vector2 position = npc.position;
				position.X += npc.velocity.X;
				int index1 = (int)(((double)position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 1) * num8)) / 16.0);
				int index2 = (int)(((double)position.Y + (double)npc.height - 1.0) / 16.0);
				if (Main.tile[index1, index2] == null)
				{
					Main.tile[index1, index2] = new Tile();
				}

				if (Main.tile[index1, index2 - 1] == null)
				{
					Main.tile[index1, index2 - 1] = new Tile();
				}

				if (Main.tile[index1, index2 - 2] == null)
				{
					Main.tile[index1, index2 - 2] = new Tile();
				}

				if (Main.tile[index1, index2 - 3] == null)
				{
					Main.tile[index1, index2 - 3] = new Tile();
				}

				if (Main.tile[index1, index2 + 1] == null)
				{
					Main.tile[index1, index2 + 1] = new Tile();
				}

				if ((double)(index1 * 16) < (double)position.X + (double)npc.width && (double)(index1 * 16 + 16) > (double)position.X && (Main.tile[index1, index2].nactive() && !Main.tile[index1, index2].topSlope() && (!Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].type] || Main.tile[index1, index2 - 1].halfBrick() && Main.tile[index1, index2 - 1].nactive()) && ((!Main.tile[index1, index2 - 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].type] || Main.tile[index1, index2 - 1].halfBrick() && (!Main.tile[index1, index2 - 4].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].type])) && ((!Main.tile[index1, index2 - 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].type]) && (!Main.tile[index1, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].type]) && (!Main.tile[index1 - num8, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 - num8, index2 - 3].type]))))
				{
					float num9 = (float)(index2 * 16);
					if (Main.tile[index1, index2].halfBrick())
					{
						num9 += 8f;
					}

					if (Main.tile[index1, index2 - 1].halfBrick())
					{
						num9 -= 8f;
					}

					if ((double)num9 < (double)position.Y + (double)npc.height)
					{
						float num10 = position.Y + (float)npc.height - num9;
						if ((double)num10 <= 16.1)
						{
							npc.gfxOffY += npc.position.Y + (float)npc.height - num9;
							npc.position.Y = num9 - (float)npc.height;
							npc.stepSpeed = (double)num10 >= 9.0 ? 4f : 2f;
						}
					}
				}
			}
			if ((double)npc.velocity.Y == 0.0)
			{
				int index1 = (int)(((double)npc.position.X + (double)(npc.width / 2) + (double)(15 * npc.direction)) / 16.0);
				int index2 = (int)(((double)npc.position.Y + (double)npc.height - 15.0) / 16.0);

				if (Main.tile[index1, index2] == null)
				{
					Main.tile[index1, index2] = new Tile();
				}

				if (Main.tile[index1, index2 - 1] == null)
				{
					Main.tile[index1, index2 - 1] = new Tile();
				}

				if (Main.tile[index1, index2 - 2] == null)
				{
					Main.tile[index1, index2 - 2] = new Tile();
				}

				if (Main.tile[index1, index2 - 3] == null)
				{
					Main.tile[index1, index2 - 3] = new Tile();
				}

				if (Main.tile[index1, index2 + 1] == null)
				{
					Main.tile[index1, index2 + 1] = new Tile();
				}

				if (Main.tile[index1 + npc.direction, index2 - 1] == null)
				{
					Main.tile[index1 + npc.direction, index2 - 1] = new Tile();
				}

				if (Main.tile[index1 + npc.direction, index2 + 1] == null)
				{
					Main.tile[index1 + npc.direction, index2 + 1] = new Tile();
				}

				if (Main.tile[index1 - npc.direction, index2 + 1] == null)
				{
					Main.tile[index1 - npc.direction, index2 + 1] = new Tile();
				}

				Main.tile[index1, index2 + 1].halfBrick();
				int spriteDirection = npc.spriteDirection;
				if ((double)npc.velocity.X < 0.0 || (double)npc.velocity.X > 0.0)
				{
					if (npc.height >= 24 && Main.tile[index1, index2 - 2].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 2].type])
					{
						if (Main.tile[index1, index2 - 3].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 3].type])
						{
							npc.velocity.Y = -8f;
							npc.netUpdate = true;
						}
						else
						{
							npc.velocity.Y = -8f;
							npc.netUpdate = true;
						}
					}
					else if (Main.tile[index1, index2 - 1].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 1].type])
					{
						npc.velocity.Y = -8f;
						npc.netUpdate = true;
					}
					else if ((double)npc.position.Y + (double)npc.height - (double)(index2 * 16) > 10.0 && Main.tile[index1, index2].nactive() && (!Main.tile[index1, index2].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]))
					{
						npc.velocity.Y = -8f;
						npc.netUpdate = true;
					}
					else if (npc.directionY < 0 && npc.type != 67 && (!Main.tile[index1, index2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 + 1].type]) && (!Main.tile[index1 + npc.direction, index2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[index1 + npc.direction, index2 + 1].type]))
					{
						npc.velocity.Y = -8f;
						npc.velocity.X *= 1.5f;
						npc.netUpdate = true;
					}
					if ((double)npc.velocity.Y == 0.0 && flag1 && (double)npc.ai[3] == 1.0)
					{
						npc.velocity.Y = -8f;
					}
				}
			}
			if (npc.velocity.X < 0.3f && npc.velocity.X > -0.3f)
			{
				npc.velocity.Y = -5f;
                npc.velocity.X += 3f * (float) npc.direction;
			}
		}
		public override void NPCLoot()
		{
			
		}
		public override void HitEffect(int hitDirection, double damage)
		{		
			for (int i = 0; i<4; i++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (Main.rand.Next(-50,10))), new Vector2(hitDirection*3f, 0f), 386, goreScale);
				Main.gore[a].timeLeft = 5;
			}
			for (int i = 0; i<4; i++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + (Main.rand.Next(-50,10))), new Vector2(hitDirection*3f, 0f), 387, goreScale);
				Main.gore[a].timeLeft = 5;
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangroveDefender/ForestSentryGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangroveDefender/ForestSentryGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangroveDefender/ForestSentryGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangroveDefender/ForestSentryGore4"), 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (groundSlamming)
			{
				int num7 = 16;
				float num9 = 6f;
				float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
				float amount1 = 0.5f;
				float num10 = 0.0f;
				float addY = 0f;
				float addHeight = -10f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
				Vector2 position1 = npc.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
				Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.Green);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Microsoft.Xna.Framework.Color newColor2 = color2;
					Microsoft.Xna.Framework.Color color3 = npc.GetAlpha(newColor2) * (1f - num8) * 1.2f;
					Vector2 position2 = new Vector2 (npc.Center.X,npc.Center.Y-2) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + npc.rotation + num10).ToRotationVector2() * (float) (2.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], position2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color3, npc.rotation, vector2_3, npc.scale*1.1f, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(Main.npcTexture[npc.type], position1, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale*1.1f, spriteEffects, 0.0f);
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			for (int i = 0; i<1; i++)
			{
				float addY = 0.0f;
				float addHeight = 0f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Mangrove_Defender/Mangrove_Defender_Glow"), new Vector2((float) ((double) npc.position.X - (double) Main.screenPosition.X + (double) (npc.width / 2) - (double) Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) npc.position.Y - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(npc.frame), Microsoft.Xna.Framework.Color.White, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
				if (npc.velocity.Y != 0f)
				{			
					for (int index = 1; index < 20; ++index)
					{
						Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(255 - index * 10, 110 - index * 10, 110 - index * 10, 110 - index * 10);
						Main.spriteBatch.Draw(mod.GetTexture("NPCs/Mangrove_Defender/Mangrove_Defender_Glow"), new Vector2((float) ((double) npc.position.X - (double) Main.screenPosition.X + (double) (npc.width / 2) - (double) Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) npc.position.Y - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight) - npc.velocity * (float) index * 0.5f, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
					}
				}
			}
			
			if (groundSlamming)
			{	
				for (int i = 0; i<1; i++)
				{
					int num7 = 4;
					float num9 = 6f;
					float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
					float amount1 = 0.5f;
					float num10 = 0.0f;
					float addY = 0f;
					float addHeight = -10f;
					SpriteEffects spriteEffects = SpriteEffects.None;
					if (npc.spriteDirection == 1)
						spriteEffects = SpriteEffects.FlipHorizontally;
					Texture2D texture = Main.npcTexture[npc.type];
					Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
					Vector2 position1 = npc.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
					Microsoft.Xna.Framework.Color color2 = Color.Green;
					for (int index2 = 0; index2 < num7; ++index2)
					{
						Microsoft.Xna.Framework.Color newColor2 = color2;
						Microsoft.Xna.Framework.Color color3 = npc.GetAlpha(newColor2) * (1f - num8);
						Vector2 position2 = new Vector2 (npc.Center.X + 2 * npc.spriteDirection,npc.Center.Y) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + npc.rotation + num10).ToRotationVector2() * (float) (4.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
						Main.spriteBatch.Draw(mod.GetTexture("NPCs/Mangrove_Defender/Mangrove_Defender_Glow"), position2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color3, npc.rotation, vector2_3, npc.scale*1.15f, spriteEffects, 0.0f);
					}
					Main.spriteBatch.Draw(mod.GetTexture("NPCs/Mangrove_Defender/Mangrove_Defender_Glow"), position1, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale*1.15f, spriteEffects, 0.0f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.player.GetSpiritPlayer().ZoneReach && Main.hardMode ? 2.1f : 0f;
			}
			return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (npc.velocity.Y != 0f)
			{
				npc.frame.Y = 13 * frameHeight;
			}
			else
			{
				if (groundSlamming)
				{
					if (npc.frameCounter < 8)
					{
						npc.frame.Y = 6 * frameHeight;
					}
					else if (npc.frameCounter < 16)
					{
						npc.frame.Y = 7 * frameHeight;
					}
					else if (npc.frameCounter < 24)
					{
						npc.frame.Y = 8 * frameHeight;
					}
					else if (npc.frameCounter < 32)
					{
						npc.frame.Y = 9 * frameHeight;
					}
					else if (npc.frameCounter < 40)
					{
						npc.frame.Y = 10 * frameHeight;
					}
					else if (npc.frameCounter < 48)
					{
						npc.frame.Y = 11 * frameHeight;
					}
					else if (npc.frameCounter < 56)
					{
						npc.frame.Y = 12 * frameHeight;
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
				else
				{
					if (npc.frameCounter < 7)
					{
						npc.frame.Y = 0 * frameHeight;
					}
					else if (npc.frameCounter < 14)
					{
						npc.frame.Y = 1 * frameHeight;
					}
					else if (npc.frameCounter < 21)
					{
						npc.frame.Y = 2 * frameHeight;
					}
					else if (npc.frameCounter < 28)
					{
						npc.frame.Y = 3 * frameHeight;
					}
					else if (npc.frameCounter < 35)
					{
						npc.frame.Y = 4 * frameHeight;
					}
					else if (npc.frameCounter < 42)
					{
						npc.frame.Y = 5 * frameHeight;
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
			}
		}
	}
}