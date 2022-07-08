using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using SpiritMod.Biomes;

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
			Main.npcFrameCount[NPC.type] = 12;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10; 
			NPCID.Sets.TrailingMode[NPC.type] = 0;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 65;
			NPC.defense = 5;
			NPC.value = 400f;
			AIType = 0;
			NPC.knockBackResist = 0.2f;
			NPC.width = 30;
			NPC.height = 56;
			NPC.damage = 0;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.noGravity = false;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.AstralAdventurerBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
				new FlavorTextBestiaryInfoElement("It appears you weren’t the only one after the meteor. These sharpshooters have been tracking it for a while and will stop at nothing to siphon its precious resources."),
			});
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Texture2D tex = Mod.Assets.Request<Texture2D>("NPCs/AstralAdventurer/AstralAdventurer_Glow").Value;
			spriteBatch.Draw(tex, new Vector2(NPC.Center.X, NPC.Center.Y + 5) - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			PlayerPlatformCheck(player);

			flyingTimer++;
			weaponTimer++;
			projectileTimer++;
			if (weaponTimer >= 250)
			{
				weaponTimer = 0;
				projectileTimer = 0;
				pickedWeapon = Main.rand.Next(10000) < 5000 ? 1 : 0;
				NPC.netUpdate = true;
			}
			
			if (flyingTimer > 240)
			{
				NPC.aiStyle = 0;
				NPC.spriteDirection = -NPC.direction;
				Flying();
				if (NPC.velocity.X > .5f || NPC.velocity.X < -.5f)
				NPC.rotation = NPC.velocity.X * 0.15f;
			}
			else
			{
				NPC.rotation = 0f;
				NPC.aiStyle = -1;
				NPC.spriteDirection = -NPC.direction;
				Walking();
			}
			
			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), 0.5f, 0.25f, 0f);
			if (pickedWeapon == 1)
            {
                if (projectileTimer > 80 && projectileTimer < 110)
                {
                    Vector2 Position = new Vector2(NPC.Center.X - (37 * NPC.spriteDirection), NPC.Center.Y);
                    Vector2 vector2 = new Vector2((float)(NPC.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.2f;
                    Dust dust = Main.dust[Dust.NewDust(Position, 8, 8, DustID.Torch, vector2.X, vector2.Y, 100, Color.Transparent, (float)(1.0 + (double)Main.rand.NextFloat() * 1))];
                    dust.noGravity = true;
                    dust.velocity = vector2;
                    dust.fadeIn += .1f;
                    dust.customData = (object)NPC;
                }
                if (projectileTimer >= 110 && projectileTimer % 12 == 0)
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						SoundEngine.PlaySound(SoundID.Item34, NPC.position);
						projectileTimer++;
						if (projectileTimer >= 150)
							projectileTimer = 0;
						float num5 = 6f;
						Vector2 vector2 = new Vector2(NPC.Center.X + 30 * -NPC.spriteDirection, NPC.position.Y + (float)NPC.height * 0.5f);
						float num6 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector2.X;
						float num7 = Math.Abs(num6) * 0.1f;
						float num8 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector2.Y - num7;
						float num14 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num8 * (double)num8);
						NPC.netUpdate = true;
						float num15 = num5 / num14;
						float num16 = num6 * num15;
						float SpeedY = num8 * num15;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector2.X, vector2.Y, num16, SpeedY, ProjectileID.FlamesTrap, 8, 0.0f, Main.myPlayer, 0.0f, 0.0f);
						Main.projectile[p].friendly = false;
						Main.projectile[p].hostile = true;
					}
				}
			}
			else
			{
				if (projectileTimer > 120 && projectileTimer < 160)
                {
                    Vector2 Position = new Vector2(NPC.Center.X - (37 * NPC.spriteDirection), NPC.Center.Y);
                    Vector2 vector2 = new Vector2((float)(NPC.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.2f;
                    Dust dust = Main.dust[Dust.NewDust(Position, 8, 8, DustID.Torch, vector2.X, vector2.Y, 100, Color.Transparent, (float)(1.0 + (double)Main.rand.NextFloat() * 1))];
                    dust.noGravity = true;
                    dust.velocity = vector2;
                    dust.customData = (object)NPC;
                }
				if (projectileTimer >= 160)
				{
					SoundEngine.PlaySound(SoundID.Item40, NPC.position);
					projectileTimer = 0;
					float num5 = 9f;
					Vector2 vector2 = new Vector2(NPC.Center.X + 30*-NPC.spriteDirection, NPC.position.Y + (float)NPC.height * 0.5f);
					float num6 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector2.X;
					float num7 = Math.Abs(num6) * 0.1f;
					float num8 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector2.Y - num7;
					float num14 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num8 * (double)num8);
					NPC.netUpdate = true;
					float num15 = num5 / num14;
					float num16 = num6 * num15;
					float SpeedY = num8 * num15;
					int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector2.X, vector2.Y, num16, SpeedY, ProjectileID.ExplosiveBullet, 14, 0.0f, Main.myPlayer, 0.0f, 0.0f);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
				}
			}
			
			if (NPC.velocity.Y != 0F)
			{
				Vector2 Position = new Vector2(NPC.Center.X, NPC.Center.Y + 20) + new Vector2((float) (NPC.direction * -14), -8f) - Vector2.One * 4f;
				Vector2 vector2 = new Vector2((float) (NPC.direction * -6), 12f) * 0.2f + Utils.RandomVector2(Main.rand, -1f, 1f) * 0.2f;
				Dust dust = Main.dust[Dust.NewDust(Position, 8, 8, DustID.Torch, vector2.X, vector2.Y, 100, Color.Transparent, (float) (1.0 + (double) Main.rand.NextFloat() * 1))];
				dust.noGravity = true;
				dust.velocity = vector2;
				dust.customData = (object) NPC;
			}
			
		}

		public void Flying()
		{	 
			NPC.TargetClosest(true);
			flyingTimer++;
			if (!propelled)
			{
				propelled = true;
				NPC.velocity.Y = -8f;
			}
			if (flyingTimer > 240+60*12)
			{
				propelled = false;
				flyingTimer = 0;
			}
			if ((double) NPC.velocity.Y == 0.0)
				NPC.ai[2] = 0.0f;
			else 
				NPC.ai[2] = 1f;
			if ((double) NPC.velocity.Y != 0.0 && (double) NPC.ai[2] == 1.0)
			{
			  NPC.TargetClosest(true);
			  NPC.spriteDirection = -NPC.direction;
				float num1 = Main.player[NPC.target].Center.X - (float) (NPC.direction * 150) - NPC.Center.X;
				float num2 = Main.player[NPC.target].Bottom.Y - NPC.Bottom.Y - 150;
				if ((double) num1 < 0.0 && (double) NPC.velocity.X > 0.0)
				  NPC.velocity.X *= 0.45f;
				else if ((double) num1 > 0.0 && (double) NPC.velocity.X < 0.0)
				  NPC.velocity.X *= 0.45f;
				if ((double) num1 < 0.0 && (double) NPC.velocity.X > -3.0)
				  NPC.velocity.X -= 0.3f;
				else if ((double) num1 > 0.0 && (double) NPC.velocity.X < 3.0)
				  NPC.velocity.X += 0.3f;
				if ((double) NPC.velocity.X > 3.0)
				  NPC.velocity.X = 3f;
				if ((double) NPC.velocity.X < -3.0)
				  NPC.velocity.X = -3f;
				if ((double) num2 < -20.0 && (double) NPC.velocity.Y > 0.0)
				  NPC.velocity.Y *= 0.9f;
				else if ((double) num2 > 20.0 && (double) NPC.velocity.Y < 0.0)
				  NPC.velocity.Y *= 0.9f;
				if ((double) num2 < -20.0 && (double) NPC.velocity.Y > -5.0)
				  NPC.velocity.Y -= 0.8f;
				else if ((double) num2 > 20.0 && (double) NPC.velocity.Y < 5.0)
				  NPC.velocity.Y += 0.8f;
		
			  for (int index = 0; index < 200; ++index)
			  {
				if (index != NPC.whoAmI && Main.npc[index].active && (Main.npc[index].type == NPC.type && (double) Math.Abs(NPC.position.X - Main.npc[index].position.X) + (double) Math.Abs(NPC.position.Y - Main.npc[index].position.Y) < (double) NPC.width))
				{
				  if ((double) NPC.position.X < (double) Main.npc[index].position.X)
					NPC.velocity.X -= 0.05f;
				  else
					NPC.velocity.X += 0.05f;
				  if ((double) NPC.position.Y < (double) Main.npc[index].position.Y)
					NPC.velocity.Y -= 0.03f;
				  else
					NPC.velocity.Y += 0.03f;
				}
			  }
			}
			else if ((double) Main.player[NPC.target].Center.Y + 100.0 < (double) NPC.position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
			{
			  NPC.velocity.Y = -2f;
			}
		}

		public void Walking()
		{
			int num1 = 30;
			int num2 = 10;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			if ((double)NPC.velocity.Y == 0.0 && ((double)NPC.velocity.X > 0.0 && NPC.direction < 0 || (double)NPC.velocity.X < 0.0 && NPC.direction > 0))
			{
				flag2 = true;
				++NPC.ai[3];
			}
			if ((double)NPC.position.X == (double)NPC.oldPosition.X || (double)NPC.ai[3] >= (double)num1 || flag2)
			{
				++NPC.ai[3];
				flag3 = true;
			}
			else if ((double)NPC.ai[3] > 0.0)
			{
				--NPC.ai[3];
			}

			if ((double)NPC.ai[3] > (double)(num1 * num2))
			{
				NPC.ai[3] = 0.0f;
			}

			if (NPC.justHit)
			{
				NPC.ai[3] = 0.0f;
			}

			if ((double)NPC.ai[3] == (double)num1)
			{
				NPC.netUpdate = true;
			}

			Vector2 vector2_1 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num3 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector2_1.X;
			float num4 = Main.player[NPC.target].position.Y - vector2_1.Y;
			float num5 = (float)Math.Sqrt((double)num3 * (double)num3 + (double)num4 * (double)num4);
			if ((double)num5 < 200.0 && !flag3)
			{
				NPC.ai[3] = 0.0f;
			}

			if ((double)NPC.ai[3] < (double)num1)
			{
				NPC.TargetClosest(true);
			}
			else
			{
				if ((double)NPC.velocity.X == 0.0)
				{
					if ((double)NPC.velocity.Y == 0.0)
					{
						++NPC.ai[0];
						if ((double)NPC.ai[0] >= 2.0)
						{
							NPC.direction *= -1;
							if (NPC.velocity.X < 0f)
							{
								NPC.spriteDirection = -1;
							}
							else if (NPC.velocity.X > 0f)
							{
								NPC.spriteDirection = 1;
							}

							NPC.ai[0] = 0.0f;
						}
					}
				}
				else
				{
					NPC.ai[0] = 0.0f;
				}

				NPC.directionY = -1;
				if (NPC.direction == 0)
				{
					NPC.direction = 1;
				}
			}
			float num6 = 1.4f; //walking speed
			float num7 = 0.5f; //regular speed (x)
			if (!flag1 && ((double)NPC.velocity.Y == 0.0 || NPC.wet || (double)NPC.velocity.X <= 0.0 && NPC.direction < 0 || (double)NPC.velocity.X >= 0.0 && NPC.direction > 0))
			{
				if ((double)NPC.velocity.X < -(double)num6 || (double)NPC.velocity.X > (double)num6)
				{
					if ((double)NPC.velocity.Y == 0.0)
					{
						Vector2 vector2_2 = NPC.velocity * 0.5f; ///////SLIDE SPEED
						NPC.velocity = vector2_2;
					}
				}
				else if ((double)NPC.velocity.X < (double)num6 && NPC.direction == 1)
				{
					NPC.velocity.X += num7;
					if ((double)NPC.velocity.X > (double)num6)
					{
						NPC.velocity.X = num6;
					}
				}
				else if ((double)NPC.velocity.X > -(double)num6 && NPC.direction == -1)
				{
					NPC.velocity.X -= num7;
					if ((double)NPC.velocity.X < -(double)num6)
					{
						NPC.velocity.X = -num6;
					}
				}
			}
			if ((double)NPC.velocity.Y >= 0.0)
			{
				int num8 = 0;
				if ((double)NPC.velocity.X < 0.0)
				{
					num8 = -1;
				}

				if ((double)NPC.velocity.X > 0.0)
				{
					num8 = 1;
				}

				Vector2 position = NPC.position;
				position.X += NPC.velocity.X;
				int index1 = (int)(((double)position.X + (double)(NPC.width / 2) + (double)((NPC.width / 2 + 1) * num8)) / 16.0);
				int index2 = (int)(((double)position.Y + (double)NPC.height - 1.0) / 16.0);

				if ((double)(index1 * 16) < (double)position.X + (double)NPC.width && (double)(index1 * 16 + 16) > (double)position.X && (Main.tile[index1, index2].HasUnactuatedTile && !Main.tile[index1, index2].TopSlope && (!Main.tile[index1, index2 - 1].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2].TileType]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].TileType] || Main.tile[index1, index2 - 1].IsHalfBlock && Main.tile[index1, index2 - 1].HasUnactuatedTile) && ((!Main.tile[index1, index2 - 1].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].TileType] || Main.tile[index1, index2 - 1].IsHalfBlock && (!Main.tile[index1, index2 - 4].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].TileType])) && ((!Main.tile[index1, index2 - 2].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].TileType]) && (!Main.tile[index1, index2 - 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].TileType]) && (!Main.tile[index1 - num8, index2 - 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1 - num8, index2 - 3].TileType]))))
				{
					float num9 = (float)(index2 * 16);
					if (Main.tile[index1, index2].IsHalfBlock)
					{
						num9 += 8f;
					}

					if (Main.tile[index1, index2 - 1].IsHalfBlock)
					{
						num9 -= 8f;
					}

					if ((double)num9 < (double)position.Y + (double)NPC.height)
					{
						float num10 = position.Y + (float)NPC.height - num9;
						if ((double)num10 <= 16.1)
						{
							NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num9;
							NPC.position.Y = num9 - (float)NPC.height;
							NPC.stepSpeed = (double)num10 >= 9.0 ? 2f : 1f;
						}
					}
				}
			}
			if ((double)NPC.velocity.Y == 0.0)
			{
				int index1 = (int)(((double)NPC.position.X + (double)(NPC.width / 2) + (double)((NPC.width / 2 + 2) * NPC.direction) + (double)NPC.velocity.X * 5.0) / 16.0);/////////
				int index2 = (int)(((double)NPC.position.Y + (double)NPC.height - 15.0) / 16.0);

				int spriteDirection = NPC.spriteDirection;
				if ((double)NPC.velocity.X < 0.0 && spriteDirection == -1 || (double)NPC.velocity.X > 0.0 && spriteDirection == 1)
				{
					float num8 = 3f;
					if (Main.tile[index1, index2 - 2].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[index1, index2 - 2].TileType])
					{
						if (Main.tile[index1, index2 - 3].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[index1, index2 - 3].TileType])
						{
							NPC.velocity.Y = -8.5f;
							NPC.netUpdate = true;
						}
						else
						{
							NPC.velocity.Y = -8.5f;
							NPC.netUpdate = true;
						}
					}
					else if (Main.tile[index1, index2 - 1].HasUnactuatedTile && !Main.tile[index1, index2 - 1].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2 - 1].TileType])
					{
						NPC.velocity.Y = -8.5f;
						NPC.netUpdate = true;
					}
					else if ((double)NPC.position.Y + (double)NPC.height - (double)(index2 * 16) > 20.0 && Main.tile[index1, index2].HasUnactuatedTile && (!Main.tile[index1, index2].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2].TileType]))
					{
						NPC.velocity.Y = -8.5f;
						NPC.netUpdate = true;
					}
					else if ((NPC.directionY < 0 || (double)Math.Abs(NPC.velocity.X) > (double)num8) && ((!Main.tile[index1, index2 + 2].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 + 2].TileType]) && (!Main.tile[index1 + NPC.direction, index2 + 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1 + NPC.direction, index2 + 3].TileType])))
					{
						NPC.velocity.Y = -8.5f;
						NPC.netUpdate = true;
					}
				}
			}
		}

		//public override void OnKill()
		//{
		//	if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.StylistQuestMeteor>().IsActive && Main.rand.NextBool(3))
		//		Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.MeteorDyeMaterial>());
		//}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(116, 5));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Sets.GunsMisc.MeteoriteSpewer.Meteorite_Spewer>(), 33));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
			}
		}

		public override void OnHitKill(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			SoundEngine.PlaySound(SoundID.Item14, NPC.Center);

			for (int i = 1; i < 5; ++i)
				Gore.NewGore(NPC.GetSource_OnHit(NPC), NPC.position, NPC.velocity, Mod.Find<ModGore>("AstralAdventurer/AstralAdventurerGore" + i).Type, 1f);
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
			NPC.frameCounter++;
			if (NPC.velocity.Y == 0f)
			{
				if (pickedWeapon == 0)
				{
					if (NPC.frameCounter < 6)
						NPC.frame.Y = 0 * frameHeight;
					else if (NPC.frameCounter < 12)
						NPC.frame.Y = 1 * frameHeight;
					else if (NPC.frameCounter < 18)
						NPC.frame.Y = 2 * frameHeight;
					else if (NPC.frameCounter < 24)
						NPC.frame.Y = 3 * frameHeight;
					else if (NPC.frameCounter < 30)
						NPC.frame.Y = 4 * frameHeight;
					else if (NPC.frameCounter < 36)
						NPC.frame.Y = 5 * frameHeight;
					else
						NPC.frameCounter = 0;
				}
				else
				{
					if (NPC.frameCounter < 6)
						NPC.frame.Y = 6 * frameHeight;
					else if (NPC.frameCounter < 12)
						NPC.frame.Y = 7 * frameHeight;
					else if (NPC.frameCounter < 18)
						NPC.frame.Y = 8 * frameHeight;
					else if (NPC.frameCounter < 24)
						NPC.frame.Y = 9 * frameHeight;
					else if (NPC.frameCounter < 30)
						NPC.frame.Y = 10 * frameHeight;
					else if (NPC.frameCounter < 36)
						NPC.frame.Y = 11 * frameHeight;
					else
						NPC.frameCounter = 0;
				}	
			}

			if (pickedWeapon == 0 && NPC.velocity.Y != 0f)
			{
				NPC.frame.Y = 5 * frameHeight;
				NPC.frame.X = 0;
			}

			if (pickedWeapon == 1 && NPC.velocity.Y != 0f)
			{
				NPC.frame.Y = 11 * frameHeight;
				NPC.frame.X = 0;
			}
		}
	}
}