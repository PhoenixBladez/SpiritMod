using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.BossLoot.AtlasDrops;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.DoT;
using SpiritMod.Items.Armor.JackSet;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Boss.Atlas
{
	[AutoloadBossHead]
	public class Atlas : ModNPC, IBCRegistrable
	{

		int[] arms = new int[2];
		int timer = 0;
		bool secondStage = false;
		bool thirdStage = false;
		bool lastStage = false;
		int collideTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 290;
			NPC.height = 450;
			NPC.damage = 100;
			NPC.lifeMax = 41000;
			NPC.defense = 48;
			NPC.knockBackResist = 0f;
			NPC.boss = true;
			NPC.timeLeft = NPC.activeTime * 30;

			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;

			NPC.noGravity = true;
			NPC.alpha = 255;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Atlas");
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.SpiritSurfaceBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("A fractured sentinel built millenia ago to protect the land from those who would dare to scar its natural form. Now, it seems you have made yourself its target."),
			});
		}

		private int Counter;

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			bool expertMode = Main.expertMode; //expert mode bool
			Player player = Main.player[NPC.target]; //player target
			bool aiChange = NPC.life <= NPC.lifeMax * 0.75f; //ai change to phase 2
			bool aiChange2 = NPC.life <= NPC.lifeMax * 0.5f; //ai change to phase 3
			bool aiChange3 = NPC.life <= NPC.lifeMax * 0.25f; //ai change to phase 4
			bool phaseChange = NPC.life <= NPC.lifeMax * 0.1f; //aggression increase
			player.AddBuff(ModContent.BuffType<UnstableAffliction>(), 2); //buff that causes gravity shit
			int defenseBuff = (int)(35f * (1f - NPC.life / NPC.lifeMax));
			NPC.defense = NPC.defDefense + defenseBuff;
			
			if (NPC.ai[0] == 0f) {
				arms[0] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - 80 - Main.rand.Next(80, 160), (int)NPC.position.Y, ModContent.NPCType<AtlasArmRight>(), NPC.whoAmI, NPC.whoAmI);
				arms[1] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + 80 + Main.rand.Next(80, 160), (int)NPC.position.Y, ModContent.NPCType<AtlasArmLeft>(), NPC.whoAmI, NPC.whoAmI);
				NPC.ai[0] = 1f;
                NPC.netUpdate = true;
                Main.npc[arms[0]].netUpdate = true;
                Main.npc[arms[1]].netUpdate = true;
            }
			else if (NPC.ai[0] == 1f) {
				NPC.ai[1] += 1f;
				if (NPC.ai[1] >= 210f) {
					NPC.alpha -= 4;
					if (NPC.alpha <= 0) {
						NPC.ai[0] = 2f;
						NPC.ai[1] = 0f;
						NPC.alpha = 0;
						NPC.velocity.Y = 14f;
						NPC.netUpdate = true;
					}
				}
			}
			else if (NPC.ai[0] == 2f) {
				if (NPC.alpha == 0) {
					Vector2 dist = player.Center - NPC.Center;
					Vector2 direction = player.Center - NPC.Center;
					NPC.netUpdate = true;
					NPC.TargetClosest(true);
					if (!player.active || player.dead) {
						NPC.TargetClosest(false);
						NPC.velocity.Y = -100f;
					}

					#region Dashing mechanics
					//dash if player is too far away
					if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) > 155) {
						direction.Normalize();
						NPC.velocity *= 0.99f;
						if (Math.Sqrt((NPC.velocity.X * NPC.velocity.X) + (NPC.velocity.Y * NPC.velocity.Y)) >= 7f) {
							int dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Stone, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
							Main.dust[dust].noGravity = true;
							dust = Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Stone, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f);
							Main.dust[dust].noGravity = true;
						}

						if (Math.Sqrt((NPC.velocity.X * NPC.velocity.X) + (NPC.velocity.Y * NPC.velocity.Y)) < 2f) {
							if (Main.netMode != NetmodeID.MultiplayerClient) {
								if (Main.rand.NextBool(25)) {
									direction.X *= Main.rand.Next(19, 24);
									direction.Y *= Main.rand.Next(19, 24);
									NPC.velocity.X = direction.X;
									NPC.velocity.Y = direction.Y;
									NPC.netUpdate = true;
								}
							}
						}
					}
					#endregion

					#region Flying Movement
					if (Math.Sqrt((dist.X * dist.X) + (dist.Y * dist.Y)) < 325) {
						float speed = expertMode ? 21f : 18f; //made more aggressive.  expert mode is more.  dusking base value is 7
						float acceleration = expertMode ? 0.16f : 0.13f; //made more aggressive.  expert mode is more.  dusking base value is 0.09
						Vector2 vector2 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
						float xDir = player.position.X + (player.width / 2) - vector2.X;
						float yDir = (float)(player.position.Y + (player.height / 2) - 120) - vector2.Y;
						float length = (float)Math.Sqrt(xDir * xDir + yDir * yDir);
						if (length > 400f) {
							++speed;
							acceleration += 0.08F;
							if (length > 600f) {
								++speed;
								acceleration += 0.08F;
								if (length > 800f) {
									++speed;
									acceleration += 0.08F;
								}
							}
						}
						float num10 = speed / length;
						xDir *= num10;
						yDir *= num10;
						if (NPC.velocity.X < xDir) {
							NPC.velocity.X = NPC.velocity.X + acceleration;
							if (NPC.velocity.X < 0 && xDir > 0)
								NPC.velocity.X = NPC.velocity.X + acceleration;
						}
						else if (NPC.velocity.X > xDir) {
							NPC.velocity.X = NPC.velocity.X - acceleration;
							if (NPC.velocity.X > 0 && xDir < 0)
								NPC.velocity.X = NPC.velocity.X - acceleration;
						}
						if (NPC.velocity.Y < yDir) {
							NPC.velocity.Y = NPC.velocity.Y + acceleration;
							if (NPC.velocity.Y < 0 && yDir > 0)
								NPC.velocity.Y = NPC.velocity.Y + acceleration;
						}
						else if (NPC.velocity.Y > yDir) {
							NPC.velocity.Y = NPC.velocity.Y - acceleration;
							if (NPC.velocity.Y > 0 && yDir < 0)
								NPC.velocity.Y = NPC.velocity.Y - acceleration;
						}
					}
					#endregion

					timer += phaseChange ? 2 : 1; //if below 20% life fire more often
					int shootThings = expertMode ? 200 : 250; //fire more often in expert mode
					if (timer > shootThings) {
						direction.Normalize();
						direction.X *= 8f;
						direction.Y *= 8f;
						int amountOfProjectiles = Main.rand.Next(6, 8);
						int damageAmount = expertMode ? 54 : 62; //always account for expert damage values
						SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
						for (int num621 = 0; num621 < 30; num621++) {
							Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 2f);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient) {
							for (int i = 0; i < amountOfProjectiles; ++i) {
								float A = Main.rand.Next(-250, 250) * 0.01f;
								float B = Main.rand.Next(-250, 250) * 0.01f;
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<PrismaticBoltHostile>(), damageAmount, 1, Main.myPlayer);
								timer = 0;
							}
						}
					}

					if (aiChange) {
						if (secondStage == false) {
							SoundEngine.PlaySound(SoundID.Item93, NPC.Center);
							float radius = 250;
							float rot = MathHelper.TwoPi / 5;
							for (int I = 0; I < 5; I++) {
								Vector2 position = NPC.Center + radius * (I * rot).ToRotationVector2();
								NPC.NewNPC(NPC.GetSource_FromAI(), (int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye>(), NPC.whoAmI, NPC.whoAmI, I * rot, radius);
							}
							secondStage = true;
						}
					}

					if (aiChange2) {
						if (thirdStage == false) {
							SoundEngine.PlaySound(SoundID.Item93, NPC.Center);
							float radius = 400;
							float rot = MathHelper.TwoPi / 10;
							for (int I = 0; I < 10; I++) {
								Vector2 position = NPC.Center + radius * (I * rot).ToRotationVector2();
								NPC.NewNPC(NPC.GetSource_FromAI(), (int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye2>(), NPC.whoAmI, NPC.whoAmI, I * rot, radius);
							}
							thirdStage = true;
						}
					}

					if (aiChange3) {
						if (lastStage == false) {
							SoundEngine.PlaySound(SoundID.Item93, NPC.Center);
							float radius = 1200;
							float rot = MathHelper.TwoPi / 41;
							for (int I = 0; I < 41; I++) {
								Vector2 position = NPC.Center + radius * (I * rot).ToRotationVector2();
								NPC.NewNPC(NPC.GetSource_FromAI(), (int)(position.X), (int)(position.Y), ModContent.NPCType<CobbledEye3>(), NPC.whoAmI, NPC.whoAmI, I * rot, radius);
							}
							lastStage = true;
						}
					}
					for (int index1 = 0; index1 < 6; ++index1) {
						float x = (NPC.Center.X - 2);
						float xnum2 = (NPC.Center.X + 2);
						float y = (NPC.Center.Y - 160);
						if (NPC.direction == -1) {
							int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, DustID.Electric, 0.0f, 0.0f, 0, new Color(), 1f);
							Main.dust[index2].position.X = x;
							Main.dust[index2].position.Y = y;
							Main.dust[index2].scale = .85f;
							Main.dust[index2].velocity *= 0.02f;
							Main.dust[index2].noGravity = true;
							Main.dust[index2].noLight = false;
						}
						else if (NPC.direction == 1) {
							int index2 = Dust.NewDust(new Vector2(xnum2, y), 1, 1, DustID.Electric, 0.0f, 0.0f, 0, new Color(), 1f);
							Main.dust[index2].position.X = xnum2;
							Main.dust[index2].position.Y = y;
							Main.dust[index2].scale = .85f;
							Main.dust[index2].velocity *= 0.02f;
							Main.dust[index2].noGravity = true;
							Main.dust[index2].noLight = false;
						}
					}
				}
			}

			collideTimer++;
			if (collideTimer == 500) {
				NPC.noTileCollide = true;
			}

			NPC.TargetClosest(true);
			if (!player.active || player.dead) {
				NPC.TargetClosest(false);
				NPC.velocity.Y = -100f;
				timer = 0;
			}

			Counter++;
			if (Counter > 400) {
				SpiritMod.tremorTime = 20;
				Counter = 0;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.85f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.65f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, hitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0) {
				NPC.position = NPC.Center;
				NPC.width = 300;
				NPC.height = 500;
				NPC.position = NPC.Center;
				for (int num621 = 0; num621 < 200; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2)) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++) {
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			int offset = NPC.IsABestiaryIconDummy ? 170 : 0;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY + offset), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/Atlas/Atlas_Glow").Value, screenPos);

		public override bool PreKill()
		{
			MyWorld.downedAtlas = true;
			return true;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddBossBag<AtlasBag>();
			npcLoot.AddCommon<AtlasMask>(7);
			npcLoot.AddCommon<Trophy8>(10);
			npcLoot.AddCommon<ArcaneGeyser>(1, 32, 43);
			npcLoot.AddOneFromOptions(1, ModContent.ItemType<Mountain>(), ModContent.ItemType<TitanboundBulwark>(), ModContent.ItemType<CragboundStaff>(), ModContent.ItemType<QuakeFist>(), ModContent.ItemType<Earthshatter>());
			npcLoot.AddOneFromOptions(30, ModContent.ItemType<JackHead>(), ModContent.ItemType<JackBody>(), ModContent.ItemType<JackLegs>());
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			if (NPC.ai[0] < 2f)
				return false;

			return base.CanHitPlayer(target, ref cooldownSlot);
		}

		public override void BossLoot(ref string name, ref int potionType)
			=> potionType = ItemID.GreaterHealingPotion;

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 12.4f;
			name = "Atlas";
			downedCondition = () => MyWorld.downedAtlas;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Atlas>()
				},
				new List<int> {
					ModContent.ItemType<StoneSkin>()
				},
				new List<int> {
					ModContent.ItemType<Trophy8>(),
					ModContent.ItemType<AtlasMask>(),
					ModContent.ItemType<AtlasBox>()
				},
				new List<int> {
					ModContent.ItemType<AtlasEye>(),
					ModContent.ItemType<Mountain>(),
					ModContent.ItemType<TitanboundBulwark>(),
					ModContent.ItemType<CragboundStaff>(),
					ModContent.ItemType<QuakeFist>(),
					ModContent.ItemType<Earthshatter>(),
					ModContent.ItemType<ArcaneGeyser>(),
					ItemID.GreaterHealingPotion
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<StoneSkin>()}] anywhere at any time.";
			texture = "SpiritMod/Textures/BossChecklist/AtlasTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/Atlas/Atlas_Head_Boss";
		}
	}
}