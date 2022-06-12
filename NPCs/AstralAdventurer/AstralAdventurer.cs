using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem;

namespace SpiritMod.NPCs.AstralAdventurer
{
	public class AstralAdventurer : SpiritNPC
	{
		public int pickedWeapon = 1;
		public int flyingTimer = 0;
		public int projectileTimer = 0;
		public int weaponTimer = 0;
		public bool propelled = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Adventurer");
			Main.npcFrameCount[npc.type] = 12;
			NPCID.Sets.TrailCacheLength[npc.type] = 10; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 65;
			npc.defense = 5;
			npc.value = 400f;
			aiType = 0;
			npc.knockBackResist = 0.2f;
			npc.width = 30;
			npc.height = 56;
			npc.damage = 0;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.noGravity = false;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath1;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.AstralAdventurerBanner>();
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * bossLifeScale);

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(mod.GetTexture("NPCs/AstralAdventurer/AstralAdventurer_Glow"), new Vector2(npc.Center.X, npc.Center.Y + 5) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			PlayerPlatformCheck(player);

			flyingTimer++;
			weaponTimer++;
			projectileTimer++;
			if (weaponTimer >= 250)
			{
				weaponTimer = 0;
				projectileTimer = 0;
				pickedWeapon = Main.rand.Next(10000) < 5000 ? 1 : 0;
				npc.netUpdate = true;
			}
			
			if (flyingTimer > 240)
			{
				npc.aiStyle = 0;
				npc.spriteDirection = -npc.direction;
				Flying();
				if (npc.velocity.X > .5f || npc.velocity.X < -.5f)
				npc.rotation = npc.velocity.X * 0.15f;
			}
			else
			{
				npc.rotation = 0f;
				npc.aiStyle = -1;
				npc.spriteDirection = -npc.direction;
				Walking();
			}
			
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.5f, 0.25f, 0f);
			if (pickedWeapon == 1)
            {
                if (projectileTimer > 80 && projectileTimer < 110)
                {
                    Vector2 Position = new Vector2(npc.Center.X - (37 * npc.spriteDirection), npc.Center.Y);
                    Vector2 vector2 = new Vector2((float)(npc.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.2f;
                    Dust dust = Main.dust[Dust.NewDust(Position, 8, 8, DustID.Fire, vector2.X, vector2.Y, 100, Color.Transparent, (float)(1.0 + (double)Main.rand.NextFloat() * 1))];
                    dust.noGravity = true;
                    dust.velocity = vector2;
                    dust.fadeIn += .1f;
                    dust.customData = (object)npc;
                }
                if (projectileTimer >= 110 && projectileTimer % 12 == 0)
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 34, 1f, 0f);
						projectileTimer++;
						if (projectileTimer >= 150)
							projectileTimer = 0;
						float num5 = 6f;
						Vector2 vector2 = new Vector2(npc.Center.X + 30 * -npc.spriteDirection, npc.position.Y + (float)npc.height * 0.5f);
						float num6 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector2.X;
						float num7 = Math.Abs(num6) * 0.1f;
						float num8 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector2.Y - num7;
						float num14 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num8 * (double)num8);
						npc.netUpdate = true;
						float num15 = num5 / num14;
						float num16 = num6 * num15;
						float SpeedY = num8 * num15;
						int p = Projectile.NewProjectile(vector2.X, vector2.Y, num16, SpeedY, ProjectileID.FlamesTrap, 8, 0.0f, Main.myPlayer, 0.0f, 0.0f);
						Main.projectile[p].friendly = false;
						Main.projectile[p].hostile = true;
					}
				}
			}
			else
			{
				if (projectileTimer > 120 && projectileTimer < 160)
                {
                    Vector2 Position = new Vector2(npc.Center.X - (37 * npc.spriteDirection), npc.Center.Y);
                    Vector2 vector2 = new Vector2((float)(npc.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.2f;
                    Dust dust = Main.dust[Dust.NewDust(Position, 8, 8, DustID.Fire, vector2.X, vector2.Y, 100, Color.Transparent, (float)(1.0 + (double)Main.rand.NextFloat() * 1))];
                    dust.noGravity = true;
                    dust.velocity = vector2;
                    dust.customData = (object)npc;
                }
				if (projectileTimer >= 160)
				{
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 40, 1f, 0f);
					projectileTimer = 0;
					float num5 = 9f;
					Vector2 vector2 = new Vector2(npc.Center.X + 30*-npc.spriteDirection, npc.position.Y + (float)npc.height * 0.5f);
					float num6 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector2.X;
					float num7 = Math.Abs(num6) * 0.1f;
					float num8 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector2.Y - num7;
					float num14 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num8 * (double)num8);
					npc.netUpdate = true;
					float num15 = num5 / num14;
					float num16 = num6 * num15;
					float SpeedY = num8 * num15;
					int p = Projectile.NewProjectile(vector2.X, vector2.Y, num16, SpeedY, ProjectileID.ExplosiveBullet, 14, 0.0f, Main.myPlayer, 0.0f, 0.0f);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}
			
			if (npc.velocity.Y != 0F)
			{
				Vector2 Position = new Vector2(npc.Center.X, npc.Center.Y + 20) + new Vector2((float) (npc.direction * -14), -8f) - Vector2.One * 4f;
				Vector2 vector2 = new Vector2((float) (npc.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.2f;
				Dust dust = Main.dust[Dust.NewDust(Position, 8, 8, DustID.Fire, vector2.X, vector2.Y, 100, Color.Transparent, (float) (1.0 + (double) Main.rand.NextFloat() * 1))];
				dust.noGravity = true;
				dust.velocity = vector2;
				dust.customData = (object) npc;
			}
			
		}

		public void Flying()
		{	 
			npc.TargetClosest(true);
			flyingTimer++;
			if (!propelled)
			{
				propelled = true;
				npc.velocity.Y = -8f;
			}
			if (flyingTimer > 240+60*12)
			{
				propelled = false;
				flyingTimer = 0;
			}
			if ((double) npc.velocity.Y == 0.0)
				npc.ai[2] = 0.0f;
			else 
				npc.ai[2] = 1f;
			if ((double) npc.velocity.Y != 0.0 && (double) npc.ai[2] == 1.0)
			{
			  npc.TargetClosest(true);
			  npc.spriteDirection = -npc.direction;
				float num1 = Main.player[npc.target].Center.X - (float) (npc.direction * 150) - npc.Center.X;
				float num2 = Main.player[npc.target].Bottom.Y - npc.Bottom.Y - 150;
				if ((double) num1 < 0.0 && (double) npc.velocity.X > 0.0)
				  npc.velocity.X *= 0.45f;
				else if ((double) num1 > 0.0 && (double) npc.velocity.X < 0.0)
				  npc.velocity.X *= 0.45f;
				if ((double) num1 < 0.0 && (double) npc.velocity.X > -3.0)
				  npc.velocity.X -= 0.3f;
				else if ((double) num1 > 0.0 && (double) npc.velocity.X < 3.0)
				  npc.velocity.X += 0.3f;
				if ((double) npc.velocity.X > 3.0)
				  npc.velocity.X = 3f;
				if ((double) npc.velocity.X < -3.0)
				  npc.velocity.X = -3f;
				if ((double) num2 < -20.0 && (double) npc.velocity.Y > 0.0)
				  npc.velocity.Y *= 0.9f;
				else if ((double) num2 > 20.0 && (double) npc.velocity.Y < 0.0)
				  npc.velocity.Y *= 0.9f;
				if ((double) num2 < -20.0 && (double) npc.velocity.Y > -5.0)
				  npc.velocity.Y -= 0.8f;
				else if ((double) num2 > 20.0 && (double) npc.velocity.Y < 5.0)
				  npc.velocity.Y += 0.8f;
		
			  for (int index = 0; index < 200; ++index)
			  {
				if (index != npc.whoAmI && Main.npc[index].active && (Main.npc[index].type == npc.type && (double) Math.Abs(npc.position.X - Main.npc[index].position.X) + (double) Math.Abs(npc.position.Y - Main.npc[index].position.Y) < (double) npc.width))
				{
				  if ((double) npc.position.X < (double) Main.npc[index].position.X)
					npc.velocity.X -= 0.05f;
				  else
					npc.velocity.X += 0.05f;
				  if ((double) npc.position.Y < (double) Main.npc[index].position.Y)
					npc.velocity.Y -= 0.03f;
				  else
					npc.velocity.Y += 0.03f;
				}
			  }
			}
			else if ((double) Main.player[npc.target].Center.Y + 100.0 < (double) npc.position.Y && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
			{
			  npc.velocity.Y = -2f;
			}
		}

		public void Walking()
		{
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
			float num6 = 1.4f; //walking speed
			float num7 = 0.5f; //regular speed (x)
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
							npc.stepSpeed = (double)num10 >= 9.0 ? 2f : 1f;
						}
					}
				}
			}
			if ((double)npc.velocity.Y == 0.0)
			{
				int index1 = (int)(((double)npc.position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 2) * npc.direction) + (double)npc.velocity.X * 5.0) / 16.0);/////////
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

				int spriteDirection = npc.spriteDirection;
				if ((double)npc.velocity.X < 0.0 && spriteDirection == -1 || (double)npc.velocity.X > 0.0 && spriteDirection == 1)
				{
					float num8 = 3f;
					if (Main.tile[index1, index2 - 2].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 2].type])
					{
						if (Main.tile[index1, index2 - 3].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 3].type])
						{
							npc.velocity.Y = -8.5f;
							npc.netUpdate = true;
						}
						else
						{
							npc.velocity.Y = -8.5f;
							npc.netUpdate = true;
						}
					}
					else if (Main.tile[index1, index2 - 1].nactive() && !Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2 - 1].type])
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
					else if ((double)npc.position.Y + (double)npc.height - (double)(index2 * 16) > 20.0 && Main.tile[index1, index2].nactive() && (!Main.tile[index1, index2].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]))
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
					else if ((npc.directionY < 0 || (double)Math.Abs(npc.velocity.X) > (double)num8) && ((!Main.tile[index1, index2 + 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 + 2].type]) && (!Main.tile[index1 + npc.direction, index2 + 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 + npc.direction, index2 + 3].type])))
					{
						npc.velocity.Y = -8.5f;
						npc.netUpdate = true;
					}
				}
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(5) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 116, 1);

			if (Main.rand.Next(33) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeteoriteSpewer"), 1);

			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.StylistQuestMeteor>().IsActive && Main.rand.NextBool(3))
				Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.MeteorDyeMaterial>());
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
			}
		}

		public override void OnHitKill(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			Main.PlaySound(SoundID.Item, npc.Center, 14);
			Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AstralAdventurer/AstralAdventurerGore1"), 1f);
			Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AstralAdventurer/AstralAdventurerGore2"), 1f);
			Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AstralAdventurer/AstralAdventurerGore3"), 1f);
			Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AstralAdventurer/AstralAdventurerGore4"), 1f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pickedWeapon);
			writer.Write(flyingTimer);
			writer.Write(pickedWeapon);
			writer.Write(projectileTimer);
			writer.Write(propelled);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pickedWeapon = reader.ReadInt32();
			flyingTimer = reader.ReadInt32();
			projectileTimer = reader.ReadInt32();
			weaponTimer = reader.ReadInt32();
			propelled = reader.ReadBoolean();
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Meteor.Chance * 0.05f;

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.velocity.Y == 0f)
			{
				if (pickedWeapon == 0)
				{
					if (npc.frameCounter < 6)
						npc.frame.Y = 0 * frameHeight;
					else if (npc.frameCounter < 12)
						npc.frame.Y = 1 * frameHeight;
					else if (npc.frameCounter < 18)
						npc.frame.Y = 2 * frameHeight;
					else if (npc.frameCounter < 24)
						npc.frame.Y = 3 * frameHeight;
					else if (npc.frameCounter < 30)
						npc.frame.Y = 4 * frameHeight;
					else if (npc.frameCounter < 36)
						npc.frame.Y = 5 * frameHeight;
					else
						npc.frameCounter = 0;
				}
				else
				{
					if (npc.frameCounter < 6)
						npc.frame.Y = 6 * frameHeight;
					else if (npc.frameCounter < 12)
						npc.frame.Y = 7 * frameHeight;
					else if (npc.frameCounter < 18)
						npc.frame.Y = 8 * frameHeight;
					else if (npc.frameCounter < 24)
						npc.frame.Y = 9 * frameHeight;
					else if (npc.frameCounter < 30)
						npc.frame.Y = 10 * frameHeight;
					else if (npc.frameCounter < 36)
						npc.frame.Y = 11 * frameHeight;
					else
						npc.frameCounter = 0;
				}	
			}

			if (pickedWeapon == 0 && npc.velocity.Y != 0f)
			{
				npc.frame.Y = 5 * frameHeight;
				npc.frame.X = 0;
			}

			if (pickedWeapon == 1 && npc.velocity.Y != 0f)
			{
				npc.frame.Y = 11 * frameHeight;
				npc.frame.X = 0;
			}
		}
	}
}