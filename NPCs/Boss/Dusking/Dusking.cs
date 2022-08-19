using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.BossLoot.DuskingDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using SpiritMod.Utilities.PhaseIndicatorCompat;

namespace SpiritMod.NPCs.Boss.Dusking
{
	[PhaseIndicator(null, 0.5f)]
	[AutoloadBossHead]
	public class Dusking : ModNPC, IBCRegistrable
	{
		// npc.ai[0] = State Manager.
		// npc.ai[1] = Additional timer (charge timer, state timer, etc).
		// npc.localAI[0] = Outer Circle Opacity.
		// npc.localAI[1] = Outer Circle Rotation.

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusking");
			Main.npcFrameCount[NPC.type] = 5;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { PortraitPositionXOverride = 20, PortraitPositionYOverride = 20 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 120;
			NPC.height = 160;
			NPC.damage = 45;
			NPC.defense = 32;
			NPC.lifeMax = 21000;
			NPC.knockBackResist = 0;
			NPC.boss = true;

			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.ShadowFlame] = true;

			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.npcSlots = 5;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;

			Music = MusicLoader.GetMusicSlot(Mod,"Sounds/Music/DuskingTheme");
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
				new FlavorTextBestiaryInfoElement("The nightmarish consequence of a failed seance by goblin warlocks. This abominable spirit remains a burning reminder of the chaos shadowflame magic can bring."),
			});
		}

		public override bool CheckActive() => NPC.Center.Y < -2000;

		public override bool PreAI()
		{
			NPC.netUpdate = true;
			NPC.TargetClosest(true);

			Lighting.AddLight(NPC.Center, 0.7f, 0.3f, 0.7f);
			Player player = Main.player[NPC.target];

			if (!player.active || player.dead || Main.dayTime || player.DistanceSQ(NPC.Center) > 20000 * 20000)
			{
				NPC.TargetClosest(false);
				NPC.velocity.Y = -100;

				if (NPC.position.Y < -1000)
					NPC.active = false;
			}

			if (NPC.ai[0] == 0) // Flying around and shooting projectiles
			{
				#region Flying Movement
				float speed = 7f;
				float acceleration = 0.09f;
				
				float xDir = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - NPC.Center.X;
				float yDir = (float)(Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2) - 120) - NPC.Center.Y;
				float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
				if (length > 400 && Main.expertMode)
				{
					++speed;
					acceleration += 0.05F;
					if (length > 600)
					{
						++speed;
						acceleration += 0.05F;
						if (length > 800)
						{
							++speed;
							acceleration += 0.05F;
						}
					}
				}

				float num10 = speed / length;
				xDir *= num10;
				yDir *= num10;

				if (NPC.velocity.X < xDir)
				{
					NPC.velocity.X = NPC.velocity.X + acceleration;
					if (NPC.velocity.X < 0 && xDir > 0)
						NPC.velocity.X = NPC.velocity.X + acceleration;
				}
				else if (NPC.velocity.X > xDir)
				{
					NPC.velocity.X = NPC.velocity.X - acceleration;
					if (NPC.velocity.X > 0 && xDir < 0)
						NPC.velocity.X = NPC.velocity.X - acceleration;
				}

				if (NPC.velocity.Y < yDir)
				{
					NPC.velocity.Y = NPC.velocity.Y + acceleration;
					if (NPC.velocity.Y < 0 && yDir > 0)
						NPC.velocity.Y = NPC.velocity.Y + acceleration;
				}
				else if (NPC.velocity.Y > yDir)
				{
					NPC.velocity.Y = NPC.velocity.Y - acceleration;
					if (NPC.velocity.Y > 0 && yDir < 0)
						NPC.velocity.Y = NPC.velocity.Y - acceleration;
				}
				#endregion
				// Shadow Ball Shoot
				if (NPC.ai[1] % 45 == 0)
				{
					Vector2 dir = Main.player[NPC.target].Center - NPC.Center;
					dir.Normalize();
					dir *= 14;
					int newNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<ShadowBall>(), NPC.whoAmI);
					Main.npc[newNPC].velocity = dir;
				}

				// Crystal Shadow Shoot.
				if (NPC.ai[1] == 150)
				{
					for (int i = 0; i < 8; ++i)
					{
						bool expertMode = Main.expertMode;
						Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
						targetDir.Normalize();
						targetDir *= 3;
						int dmg = expertMode ? 23 : 37;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CrystalShadow>(), dmg, 0.5F, Main.myPlayer);
					}
				}

				// Shadowflamer Shoot
				if (NPC.ai[1] % 110 == 0)
				{
					Vector2 dir = Main.player[NPC.target].Center - NPC.Center;
					dir += new Vector2(Main.rand.Next(-40, 41), Main.rand.Next(-40, 41));
					dir.Normalize();
					dir *= 12;
					int newNPC = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Shadowflamer>(), NPC.whoAmI);
					Main.npc[newNPC].velocity = dir;
				}

				NPC.ai[1]++;
				if (NPC.ai[1] >= 300)
				{
					NPC.ai[0] = 1;
					NPC.ai[1] = 60;
					NPC.ai[2] = 0;
					NPC.ai[3] = 0;
				}

				// Rage Phase Switch
				if (NPC.life <= 9000)
				{
					NPC.ai[0] = 2;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
					NPC.ai[3] = 0;
				}
			}
			else if (NPC.ai[0] == 1) // Charging.
			{
				NPC.ai[1]++;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (NPC.ai[1] % 45 == 0)
					{
						NPC.TargetClosest(true);
						float speed = 10 + (2 * (int)(NPC.life / 5000));
						Vector2 vector2_1 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float dirX = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2) - vector2_1.X;
						float dirY = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2) - vector2_1.Y;
						float targetVel = Math.Abs(Main.player[NPC.target].velocity.X) + Math.Abs(Main.player[NPC.target].velocity.Y) / 4f;

						float speedMultiplier = targetVel + (10f - targetVel);
						if (speedMultiplier < 6.0)
							speedMultiplier = 6f;
						if (speedMultiplier > 16.0)
							speedMultiplier = 16f;
						float speedX = dirX - Main.player[NPC.target].velocity.X * speedMultiplier;
						float speedY = dirY - (Main.player[NPC.target].velocity.Y * speedMultiplier / 4);
						speedX *= (float)(1 + Main.rand.Next(-10, 11) * 0.01);
						speedY *= (float)(1 + Main.rand.Next(-10, 11) * 0.01);
						float speedLength = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float actualSpeed = speed / speedLength;
						NPC.velocity.X = speedX * actualSpeed;
						NPC.velocity.Y = speedY * actualSpeed;
						NPC.velocity.X = NPC.velocity.X + Main.rand.Next(-40, 41) * 0.1f;
						NPC.velocity.Y = NPC.velocity.Y + Main.rand.Next(-40, 41) * 0.1f;
						NPC.netUpdate = true;
					}
				}
				if (NPC.ai[1] >= 270)
				{
					NPC.ai[0] = 0;
					NPC.ai[1] = 0;
					NPC.ai[2] = 0;
					NPC.ai[3] = 0;
					NPC.velocity *= 0.3F;
				}
			}
			else if (NPC.ai[0] == 2) // Continuous Charging.
			{
				if (NPC.ai[1] == 0) // Flying Movement
				{
					bool expertMode = Main.expertMode;
					float speed = 38f;
					float acceleration = 1.55f;
					float num7 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - NPC.Center.X;
					float num8 = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2) - 120 - NPC.Center.Y;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);

					if (Main.rand.NextBool(100))
					{
						for (int i = 0; i < 8; ++i)
						{
							Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
							targetDir.Normalize();
							targetDir *= 3;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CrystalShadow>(), 26, 0.5F, Main.myPlayer);
						}
					}

					if (NPC.life >= (NPC.lifeMax / 2))
					{
						if (Main.rand.NextBool(100))
						{
							SoundEngine.PlaySound(SoundID.Item21, player.Center);
							Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
							direction.Normalize();
							direction.X *= 12f;
							direction.Y *= 12f;

							int amountOfProjectiles = 1;
							for (int i = 0; i < amountOfProjectiles; ++i)
							{
								float A = Main.rand.Next(-200, 200) * 0.01f;
								float B = Main.rand.Next(-200, 200) * 0.01f;
								int damage = expertMode ? 23 : 37;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<CrystalShadow>(), damage, 1, Main.myPlayer, 0, 0);
							}
						}
					}

					if (num9 > 400 && Main.expertMode)
					{
						++speed;
						acceleration += 0.25F;
						if (num9 > 600)
						{
							++speed;
							acceleration += 0.25F;
							if (num9 > 800)
							{
								++speed;
								acceleration += 0.25F;
							}
						}
					}

					float num10 = speed / num9;
					float num11 = num7 * num10;
					float num12 = num8 * num10;

					if (NPC.velocity.X < num11)
					{
						NPC.velocity.X = NPC.velocity.X + acceleration;
						if (NPC.velocity.X < 0 && num11 > 0)
							NPC.velocity.X = NPC.velocity.X + acceleration;
					}
					else if (NPC.velocity.X > num11)
					{
						NPC.velocity.X = NPC.velocity.X - acceleration;
						if (NPC.velocity.X > 0 && num11 < 0)
							NPC.velocity.X = NPC.velocity.X - acceleration;
					}

					if (NPC.velocity.Y < num12)
					{
						NPC.velocity.Y = NPC.velocity.Y + acceleration;
						if (NPC.velocity.Y < 0 && num12 > 0)
							NPC.velocity.Y = NPC.velocity.Y + acceleration;
					}
					else if (NPC.velocity.Y > num12)
					{
						NPC.velocity.Y = NPC.velocity.Y - acceleration;
						if (NPC.velocity.Y > 0 && num12 < 0)
							NPC.velocity.Y = NPC.velocity.Y - acceleration;
					}

					NPC.ai[2]++;
					if (NPC.ai[2] >= 120)
					{
						NPC.ai[1] = 1;
						NPC.ai[2] = 0;
					}
				}

				else if (NPC.ai[1] == 1)
				{
					NPC.ai[2]++;
					if (NPC.ai[2] % 45 == 0)
					{
						NPC.TargetClosest(true);
						float speed = 15 + (2 * (int)(NPC.life / 10000));
						Vector2 vector2_1 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float dirX = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2) - vector2_1.X;
						float dirY = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2) - vector2_1.Y;
						float targetVel = Math.Abs(Main.player[NPC.target].velocity.X) + Math.Abs(Main.player[NPC.target].velocity.Y) / 4f;
						float speedMultiplier = targetVel + (10f - targetVel);
						if (speedMultiplier < 35.0)
							speedMultiplier = 35f;
						if (speedMultiplier > 25.0)
							speedMultiplier = 25f;
						float speedX = dirX - Main.player[NPC.target].velocity.X * speedMultiplier;
						float speedY = dirY - (Main.player[NPC.target].velocity.Y * speedMultiplier / 4);
						speedX *= 1 + Main.rand.Next(-10, 11) * 0.019f;
						speedY *= 1 + Main.rand.Next(-10, 11) * 0.019f;
						float speedLength = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float actualSpeed = speed / speedLength;
						NPC.velocity.X = speedX * actualSpeed;
						NPC.velocity.Y = speedY * actualSpeed;
						NPC.velocity.X = NPC.velocity.X + Main.rand.Next(-20, 21) * 0.5f;
						NPC.velocity.Y = NPC.velocity.Y + Main.rand.Next(-20, 21) * 0.5f;
					}

					if (Main.rand.NextBool(100))
					{
						for (int i = 0; i < 8; ++i)
						{
							Vector2 targetDir = ((((float)Math.PI * 2) / 8) * i).ToRotationVector2();
							targetDir.Normalize();
							targetDir *= 3;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, targetDir.X, targetDir.Y, ModContent.ProjectileType<CrystalShadow>(), 26, 0.5F, Main.myPlayer);
						}
					}
					if (NPC.ai[2] >= 270)
					{
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						NPC.velocity *= 0.3F;
					}
				}

				// Circle code.
				if (NPC.localAI[0] < 1)
					NPC.localAI[0] += 0.01F;
				NPC.localAI[1] += 0.03F;
				NPC.ai[3]++;

				if (NPC.ai[3] >= 6)
				{
					for (int i = 0; i < 255; ++i)
					{
						if (Main.player[i].active && !Main.player[i].dead)
						{
							if ((Main.player[i].Center - NPC.Center).Length() <= 200)
							{
								//Main.player[i].Hurt(1, 0, false, false, " was evaporated...", false, 1); commed out because this needs work
								Main.player[i].AddBuff(BuffID.Darkness, 330);
							}
						}
					}
					NPC.ai[3] = 0;
				}
			}
			else if (NPC.ai[0] == 3)
			{
				NPC.velocity *= 0.97F;
				NPC.alpha += 3;

				if (NPC.alpha >= 255)
					NPC.active = false;
			}
			if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);

				if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
					NPC.ai[0] = 3;
			}
			return true;
		}

		public override void AI()
		{
			NPC.netUpdate = true;
			NPC.TargetClosest(true);
			Lighting.AddLight(NPC.Center, 0.7F, 0.3F, 0.7F);
			Player player = Main.player[NPC.target];

			if (Main.expertMode)
			{
				if (Main.rand.NextBool(100) && NPC.life >= (NPC.lifeMax / 2))
				{
					SoundEngine.PlaySound(SoundID.Item33, player.Center);
					Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 12f;

					bool expertMode = Main.expertMode;
					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = Main.rand.Next(-80, 80) * 0.01f;
						float B = Main.rand.Next(-80, 80) * 0.01f;
						int damage = expertMode ? 23 : 37;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<ShadowPulse>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= (NPC.ai[0] == 2 ? 3 : 6))
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
			}
			NPC.spriteDirection = NPC.direction;
		}

		public override bool PreKill()
		{
			MyWorld.downedDusking = true;
			for (int i = 0; i < 15; ++i)
			{
				int newDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, 0f, 0f, 100, default, 2.5f);
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 5f;
				newDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
				Main.dust[newDust].velocity *= 3f;
			}
			return true;
		}

		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddBossBag<DuskingBag>();
			npcLoot.AddCommon<DuskStone>(1, 25, 35);
			npcLoot.AddCommon<DuskingMask>(7);
			npcLoot.AddCommon<Trophy6>(10);
			npcLoot.AddOneFromOptions<ShadowflameSword, UmbraStaff, ShadowSphere, Shadowmoor>();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(ModContent.BuffType<Shadowflame>(), 150);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.localAI[0] > 0)
			{
				Texture2D ring = Mod.Assets.Request<Texture2D>("Effects/Glowmasks/Dusking_Circle").Value;
				Vector2 origin = new Vector2(ring.Width * 0.5f, ring.Height * 0.5f);
				spriteBatch.Draw(ring, (NPC.Center) - screenPos, null, Color.White * NPC.localAI[0], NPC.localAI[1], origin, 1, SpriteEffects.None, 0);
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 7.3f;
			name = "Dusking";
			downedCondition = () => MyWorld.downedDusking;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> { ModContent.NPCType<Dusking>() },
				new List<int> { ModContent.ItemType<DuskCrown>() },
				new List<int>
				{
					ModContent.ItemType<Trophy6>(),
					ModContent.ItemType<DuskingMask>()
				},
				new List<int>
				{
					ModContent.ItemType<DuskPendant>(),
					ModContent.ItemType<ShadowflameSword>(),
					ModContent.ItemType<UmbraStaff>(),
					ModContent.ItemType<ShadowSphere>(),
					ModContent.ItemType<Shadowmoor>(),
					ModContent.ItemType<DuskStone>(),
					ItemID.GreaterHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<DuskCrown>()}] anywhere at nighttime.";
			texture = "SpiritMod/Textures/BossChecklist/DuskingTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Dusking/Dusking_Head_Boss";
		}
	}
}