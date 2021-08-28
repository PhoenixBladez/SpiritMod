using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Flesh_Larvae
{
	public class Flesh_Larvae : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Larvae");
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 75;
			npc.defense = 20;
			npc.value = 500f;
			aiType = 0;
			npc.knockBackResist = 0.6f;
			npc.width = 40;
			npc.height = 24;
			npc.damage = 25;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 11);
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);

			if ((double)Vector2.Distance(player.Center, npc.Center) < (double)500f)
			{
				if (npc.alpha <= 200 && npc.alpha >= 5)
				{
					npc.alpha -= 5;
				}

				movement();
				if (npc.velocity.X < 0f)
				{
					npc.spriteDirection = -1;
				}
				else if (npc.velocity.X > 0f)
				{
					npc.spriteDirection = 1;
				}

				npc.dontTakeDamage = false;
				npc.damage = 25;
				int num = Main.rand.Next(1, 3);
				if ((double)Vector2.Distance(player.Center, npc.Center) < (double)350f)
				{
					for (int index1 = 0; index1 < num; ++index1)
					{
						if (npc.velocity.X != 0f)
						{
							if ((double)Vector2.Distance(player.Center, npc.Center) < (double)150f)
							{
								player.AddBuff(22, 2);
								player.AddBuff(32, 2);
								if (Main.rand.Next(300) == 0)
								{
									player.AddBuff(31, 2);
								}
							}
						}
						else if (npc.velocity.X == 0f)
						{
							if ((double)Vector2.Distance(player.Center, npc.Center) < (double)50f)
							{
								player.AddBuff(22, 2);
								player.AddBuff(32, 2);
								if (Main.rand.Next(300) == 0)
								{
									player.AddBuff(31, 2);
								}
							}
						}
						int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Smoke, 0.0f, 0.0f, 100, Color.Pink, 0.4f);
						Main.dust[index2].alpha += Main.rand.Next(100);
						Main.dust[index2].velocity *= 0.3f;
						Main.dust[index2].velocity.X += (float)Main.rand.Next(-10, 11) * 0.025f * npc.velocity.X;
						Main.dust[index2].velocity.Y -= (float)(0.400000005960464 + (double)Main.rand.Next(-3, 14) * 0.150000005960464);
						Main.dust[index2].fadeIn = (float)(0.25 + (double)Main.rand.Next(10) * 0.150000005960464);
					}
				}
			}
			else
			{
				npc.velocity.X = 0f;
				if (npc.alpha < 200)
				{
					npc.alpha += 5;
				}

				npc.dontTakeDamage = true;
				npc.damage = 0;
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
							if (npc.velocity.X < 0f)
							{
								npc.spriteDirection = -1;
							}
							else if (npc.velocity.X > 0f)
							{
								npc.spriteDirection = 1;
							}

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
			float num6 = 3f; //walking speed
			if ((double)Vector2.Distance(player.Center, npc.Center) < (double)50f)
			{
				num6 = 5f;
			}
			else if ((double)Vector2.Distance(player.Center, npc.Center) >= (double)50f && (double)Vector2.Distance(player.Center, npc.Center) < (double)100f)
			{
				num6 = 2.5f;
			}
			else if ((double)Vector2.Distance(player.Center, npc.Center) >= (double)100f && (double)Vector2.Distance(player.Center, npc.Center) < (double)200f)
			{
				num6 = 2.5f;
			}
			else if ((double)Vector2.Distance(player.Center, npc.Center) >= (double)200f && (double)Vector2.Distance(player.Center, npc.Center) < (double)300f)
			{
				num6 = 2f;
			}
			else if ((double)Vector2.Distance(player.Center, npc.Center) >= (double)300f && (double)Vector2.Distance(player.Center, npc.Center) < (double)400f)
			{
				num6 = 1.5f;
			}

			if ((double)Vector2.Distance(player.Center, npc.Center) >= (double)400f)
			{
				num6 = 1f;
			}

			float num7 = 0.08f; //regular speed (x)
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
					npc.velocity.X -= num7;
					if ((double)npc.velocity.X > (double)num6)
					{
						npc.velocity.X = -num6;
					}
				}
				else if ((double)npc.velocity.X > -(double)num6 && npc.direction == -1)
				{
					npc.velocity.X += num7;
					if ((double)npc.velocity.X < -(double)num6)
					{
						npc.velocity.X = num6;
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
				if (npc.type == NPCID.VortexRifleman)
				{
					spriteDirection *= -1;
				}

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
					else if (npc.directionY < 0 && npc.type != NPCID.Crab && (!Main.tile[index1, index2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 + 1].type]) && (!Main.tile[index1 + npc.direction, index2 + 1].nactive() || !Main.tileSolid[(int)Main.tile[index1 + npc.direction, index2 + 1].type]))
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
			if (npc.velocity.X == 0f)
			{
				npc.velocity.Y = -8f;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LarvaeGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LarvaeGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LarvaeGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LarvaeGore4"), 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.downedBoss2)
			{
				return SpawnCondition.Crimson.Chance * 0.15f;
			}
			else
			{
				return 0f;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;
			const int Frame_5 = 4;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if ((double)Vector2.Distance(player.Center, npc.Center) < (double)500f)
			{
				if (npc.frameCounter < 7)
				{
					npc.frame.Y = Frame_1 * frameHeight;
				}
				else if (npc.frameCounter < 14)
				{
					npc.frame.Y = Frame_2 * frameHeight;
				}
				else if (npc.frameCounter < 21)
				{
					npc.frame.Y = Frame_3 * frameHeight;
				}
				else if (npc.frameCounter < 28)
				{
					npc.frame.Y = Frame_4 * frameHeight;
				}
				else if (npc.frameCounter < 35)
				{
					npc.frame.Y = Frame_5 * frameHeight;
				}
				else if (npc.frameCounter < 42)
				{
					npc.frame.Y = Frame_4 * frameHeight;
				}
				else if (npc.frameCounter < 49)
				{
					npc.frame.Y = Frame_3 * frameHeight;
				}
				else if (npc.frameCounter < 56)
				{
					npc.frame.Y = Frame_2 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
			else
			{
				npc.frame.Y = Frame_3 * frameHeight;
			}
		}
	}
}