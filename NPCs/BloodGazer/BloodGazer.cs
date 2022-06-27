using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory.UmbillicalEyeball;
using SpiritMod.Items.Sets.BloodcourtSet;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.BloodGazer
{
	internal enum BloodGazerAiStates
	{
		Passive = 0,
		EyeSpawn = 1,
		Hostile = 2,
		EyeSwing = 3,
		EyeSpin = 4,
		DetatchingEyes = 5,
		Despawn = 6,
		Phase2Transition = 7,
		EyeMortar = 8,
		RuneEyes = 9,
		SpookyDash = 10
	}

	[AutoloadBossHead]
	public class BloodGazer : ModNPC, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Gazer");
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(50, 68);
			NPC.damage = 45;
			NPC.defense = 8;
			NPC.lifeMax = 6000;
			NPC.knockBackResist = 0.1f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.npcSlots = 8;
			NPC.aiStyle = -1;
			Main.npcFrameCount[NPC.type] = 4;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath10;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BloodGazerBanner>();
			NPC.netAlways = true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 0.66f);
			NPC.damage = (int)(NPC.damage * bossLifeScale * 0.66f);
		}

		private ref float AiState => ref NPC.ai[0];
		private ref float AiTimer => ref NPC.ai[1];
		private ref float AttackCounter => ref NPC.ai[2];
		private ref float Phase => ref NPC.ai[3];
		private ref float GlowmaskOpacity => ref NPC.localAI[1];
		private ref float GlowTrailOpacity => ref NPC.localAI[2];
		private float _pulseDrawTimer;
		public bool trailing = false;

		private void UpdateAiState(BloodGazerAiStates aistate)
		{
			trailing = false;
			AiState = (float)aistate;
			AiTimer = 0;
			NPC.netUpdate = true;
		}

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];
			if (player.active && !player.dead && Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0) && NPC.Distance(player.Center) < 500 && AiState == (float)BloodGazerAiStates.Passive)   //aggro on player if able to reach them, and in its passive state
				UpdateAiState(BloodGazerAiStates.EyeSpawn);

			AiTimer++;
			NPC.rotation = NPC.velocity.X * 0.05f;
			if (Main.netMode != NetmodeID.Server && Main.rand.Next(60) == 0)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/HeartbeatFx"), NPC.position);

			if (Phase == 0 && NPC.life <= NPC.lifeMax / 2)
			{
				Phase++;
				UpdateAiState(BloodGazerAiStates.Phase2Transition);
			}

			switch ((BloodGazerAiStates)AiState)
			{
				case BloodGazerAiStates.EyeSpawn:
					NPC.knockBackResist = 0.1f;
					NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);

					if (AiTimer % 40 == 0)
					{
						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.NPCDeath22 with { PitchVariance = 0.2f, Volume = 0.8f }, NPC.Center);

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NPC eye = Main.npc[NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<BloodGazerEye>(), 0, NPC.whoAmI, (int)(AiTimer / 40))];
							Vector2 velocity = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(2, 4);
							eye.velocity = velocity;
							for (int i = 0; i < 25; i++)
							{
								Dust dust = Dust.NewDustDirect(NPC.Center, 10, 10, ModContent.DustType<Dusts.Blood>(), 0f, -2f, 0, default, Main.rand.NextFloat(0.9f, 1.5f));
								dust.velocity = velocity.RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(0.5f, 1f);
							}
							NPC.velocity = -velocity / 2;
							if (Main.netMode != NetmodeID.SinglePlayer)
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, eye.whoAmI);

							eye.netUpdate = true;
						}
						NPC.netUpdate = true;
					}

					if (AiTimer > 120)
						UpdateAiState(BloodGazerAiStates.Hostile);

					break;

				case BloodGazerAiStates.Hostile:
					NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * MathHelper.Clamp(NPC.Distance(player.Center) / 200, 1, 4), 0.03f);

					if (AiTimer >= 200)
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeSwing : BloodGazerAiStates.EyeSpin);

					break;

				case BloodGazerAiStates.EyeSwing:
					NPC.knockBackResist = 0f;
					NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 2, 0.015f);

					if (AiTimer >= 250)
					{
						UpdateAiState(AttackCounter == 0 ? BloodGazerAiStates.EyeSpin : BloodGazerAiStates.DetatchingEyes);
						AttackCounter++;
					}

					break;

				case BloodGazerAiStates.EyeSpin:
					NPC.spriteDirection = Math.Sign(NPC.velocity.X) < 0 ? -1 : 1;
					NPC.knockBackResist = 0f;
					float prespintime = 60;
					float spintime = 60;
					float slowdowntime = 40;
					if (AiTimer < prespintime)
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);

					if ((AiTimer > prespintime + spintime) && (Math.Sign(NPC.velocity.X) * (player.Center.X - NPC.Center.X)) < -300)
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.07f);

					float startvel = 1;
					float maxvel = 16;
					if (AiTimer == prespintime)
					{
						NPC.velocity.X = (Math.Sign(NPC.DirectionTo(player.Center).X) == 0 ? 1 : Math.Sign(NPC.DirectionTo(player.Center).X)) * startvel;
						NPC.netUpdate = true;

						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.NPCDeath10 with { Volume = 0.5f }, NPC.Center);
					}

					if (AiTimer <= prespintime + spintime && Math.Abs(NPC.velocity.X) < maxvel)
						NPC.velocity.X *= 1.06f;

					if (AiTimer >= prespintime + spintime + slowdowntime && Math.Abs(NPC.velocity.X) < 4)
					{
						UpdateAiState(AttackCounter == 0 ? BloodGazerAiStates.EyeSwing : BloodGazerAiStates.DetatchingEyes);
						AttackCounter++;
					}

					break;

				case BloodGazerAiStates.DetatchingEyes:
					NPC.knockBackResist = 0.1f;
					NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.03f);

					if (AiTimer >= 180)
					{
						AttackCounter = 0;
						UpdateAiState(BloodGazerAiStates.EyeSpawn);
					}
					break;

				case BloodGazerAiStates.Passive:
					NPC.spriteDirection = Math.Sign(NPC.velocity.X) < 0 ? -1 : 1;
					NPC.velocity.X = (float)Math.Sin(AiTimer / 360) * 2;
					NPC.velocity.Y = (float)Math.Cos(AiTimer / 90);
					break;

				case BloodGazerAiStates.Despawn:
					NPC.knockBackResist = 0f;
					NPC.spriteDirection = Math.Sign(NPC.velocity.X) < 0 ? -1 : 1;
					NPC.timeLeft = Math.Min(NPC.timeLeft - 1, 60);
					NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, NPC.spriteDirection * 14, 0.00175f);
					NPC.velocity.Y = (float)Math.Cos(AiTimer / 90);
					break;

				case BloodGazerAiStates.Phase2Transition:
					NPC.knockBackResist = 0f;
					GlowmaskOpacity = MathHelper.Lerp(NPC.localAI[1], 0.75f, 0.05f);

					if (AiTimer <= 31)
						NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionFrom(player.Center), 0.1f);

					else
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.075f);

					if (GlowmaskOpacity > 0.6f)
					{
						GlowmaskOpacity = 0.75f;
						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 2, 0.75f, -5f);

						NPC.netUpdate = true;
					}

					if (AiTimer > 90)
						UpdateAiState(BloodGazerAiStates.RuneEyes);

					break;

				case BloodGazerAiStates.EyeMortar:
					GlowTrailOpacity = MathHelper.Lerp(GlowTrailOpacity, 0, 0.1f);

					if (AiTimer < 110)
						NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center - Vector2.UnitY * 300) * MathHelper.Clamp(NPC.Distance(player.Center - Vector2.UnitY * 200) / 75, 3, 12) * 1.5f, 0.07f);
					else
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);

					if (AiTimer % 8 == 0 && AiTimer > 130 && AiTimer < 280)
					{
						Vector2 targetPos = player.Center + new Vector2(MathHelper.Lerp(1500 * NPC.spriteDirection, 0, Math.Min((AiTimer - 130) / 100f, 1)), 0);
						Vector2 vel = NPC.GetArcVel(targetPos, 0.35f, 300, 1000, heightabovetarget: 350);
						NPC.velocity = -Vector2.Normalize(vel);
						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.NPCDeath22 with { PitchVariance = 0.2f, Volume = 0.8f }, NPC.Center);

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile eye = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, vel.RotatedByRandom(MathHelper.Pi / 16), ModContent.ProjectileType<MortarEye>(), NPC.damage / 4, 1, Main.myPlayer, NPC.whoAmI);
							eye.netUpdate = true;
						}
					}

					if (AiTimer >= 300)
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.SpookyDash : BloodGazerAiStates.RuneEyes);

					break;

				case BloodGazerAiStates.RuneEyes:
					NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center), 0.075f);
					_pulseDrawTimer++;
					GlowTrailOpacity = MathHelper.Lerp(GlowTrailOpacity, 0, 0.1f);

					if (AiTimer % 15 == 0 && AiTimer > 15 && AiTimer < 105)
					{
						Vector2 spawnPos = NPC.DirectionTo(player.Center).RotatedBy(MathHelper.Pi * Main.rand.NextFloat(0.08f, 0.12f) * (Main.rand.NextBool() ? -1 : 1)) * AiTimer * 12;
						spawnPos += NPC.Center;

						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.Item104 with { PitchVariance = 0.3f, Volume = 0.5f }, spawnPos);

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile eye = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, Vector2.Zero, ModContent.ProjectileType<RunicEye>(), NPC.damage / 4, 1, Main.myPlayer, 0, NPC.whoAmI);
							eye.netUpdate = true;
						}
					}

					if (AiTimer >= 115)
					{
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeMortar : BloodGazerAiStates.SpookyDash);
						_pulseDrawTimer = 0;
					}

					break;

				case BloodGazerAiStates.SpookyDash:
					if (AiTimer >= 360)
					{
						UpdateAiState(Main.rand.NextBool() ? BloodGazerAiStates.EyeMortar : BloodGazerAiStates.RuneEyes);
						return;
					}

					NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * 4, 0.04f);
					GlowTrailOpacity = MathHelper.Lerp(GlowTrailOpacity, 1, 0.1f);
					GlowTrailOpacity = MathHelper.Lerp(GlowTrailOpacity, 1, 0.1f);
					NPC.rotation = NPC.velocity.ToRotation() + (NPC.spriteDirection < 0 ? MathHelper.PiOver2 : MathHelper.PiOver2);

					if (AiTimer % 60 == 0 && AiTimer > 60)
					{
						trailing = true;
						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown with { Volume = 1.5f }, NPC.Center);
						NPC.velocity = NPC.DirectionTo(player.Center) * 40;
						NPC.netUpdate = true;
					}

					break;
			}

			if ((NPC.Distance(player.Center) > 2000) && AiState != (float)BloodGazerAiStates.Passive && AiState != (float)BloodGazerAiStates.Despawn && Phase == 0)  //deaggro if player is too far away
				UpdateAiState(BloodGazerAiStates.Passive);

			if (!player.active || player.dead || ((NPC.Distance(player.Center) > 3000 && AiState != (float)BloodGazerAiStates.Phase2Transition) || Main.dayTime) && AiState != (float)BloodGazerAiStates.Despawn)  //despawn if day or player dead
				UpdateAiState(BloodGazerAiStates.Despawn);
		}

		public override bool CheckActive()
		{
			if (AiState == (float)BloodGazerAiStates.Passive || AiState == (float)BloodGazerAiStates.Despawn)
				return true;
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
			{
				float opacity = 0.5f * (float)(NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type];
				spriteBatch.Draw(tex, NPC.oldPos[i] + NPC.Size / 2 - Main.screenPosition, NPC.frame, Color.White * opacity * GlowTrailOpacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale * 1.35f, SpriteEffects.None, 0);
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => trailing;

		public override bool? CanHitNPC(NPC target) => trailing;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.knockBackResist);
			writer.Write(trailing);
			writer.Write(GlowmaskOpacity);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.knockBackResist = reader.ReadSingle();
			trailing = reader.ReadBoolean();
			GlowmaskOpacity = reader.ReadSingle();
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 || NPC.life >= 0)
			{
				for (int k = 0; k < 25; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
				}
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Gazer/Gazer1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Gazer/Gazer2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Gazer/Gazer3").Type, 1f);

				for (int k = 0; k < 25; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.97f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 1.27f);
				}
			}
		}

		public override bool PreKill()
		{
			if (!Main.dedServ)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.position);
			MyWorld.downedGazer = true;
			return true;
		}

		public override void OnKill()
		{
			NPC.DropItem(ModContent.ItemType<UmbillicalEyeball>());
			NPC.DropItem(ModContent.ItemType<DreamstrideEssence>(), 12 + Main.rand.Next(3, 5));
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = TextureAssets.Npc[NPC.type].Value;

			if (trailing)
			{
				for (int i = 0; i < NPCID.Sets.TrailCacheLength[NPC.type]; i++)
				{
					float opacity = 0.25f * (float)(NPCID.Sets.TrailCacheLength[NPC.type] - i) / NPCID.Sets.TrailCacheLength[NPC.type];
					spriteBatch.Draw(tex, NPC.oldPos[i] + NPC.Size / 2 - Main.screenPosition, NPC.frame, drawColor * opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
				}
			}

			Texture2D glowmask = ModContent.Request<Texture2D>(Texture + "_mask", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			void DrawMask(Vector2 center, float opacity) => spriteBatch.Draw(glowmask, center - Main.screenPosition, NPC.frame, Color.Red * opacity * GlowmaskOpacity * NPC.Opacity, NPC.rotation,
																				NPC.frame.Size() / 2, NPC.scale * 1.2f, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			DrawMask(NPC.Center, 0.5f);
			int numtodraw = 6;
			for (int i = 0; i < numtodraw; i++)
			{
				float timer = ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f);
				Vector2 center = NPC.Center + new Vector2(0, 8).RotatedBy(i * MathHelper.TwoPi / numtodraw) * timer;
				DrawMask(center, 1 - timer);
			}

			spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			float pulsetime = 75;
			if(_pulseDrawTimer > 0 && _pulseDrawTimer < pulsetime)
			{
				for(int i = 0; i < 6; i++)
				{
					Vector2 drawOffset = Vector2.UnitX.RotatedBy(i/6f * MathHelper.TwoPi) * 20 * (1 - Math.Abs((pulsetime/2) - _pulseDrawTimer) / (pulsetime/2));
					float opacity = 1 - (Math.Abs((pulsetime / 2) - _pulseDrawTimer) / (pulsetime / 2));
					opacity *= 0.66f;
					spriteBatch.Draw(tex, NPC.Center + drawOffset - Main.screenPosition, NPC.frame, NPC.GetAlpha(drawColor) * opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
				}
			}
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D glowmask = ModContent.Request<Texture2D>(Texture + "_mask2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Color col = Color.Red * GlowmaskOpacity * NPC.Opacity;
			SpriteEffects effect = (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			void DrawMask(Vector2 center, float opacity) => spriteBatch.Draw(glowmask, center - Main.screenPosition, NPC.frame, col * opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0);

			DrawMask(NPC.Center, 0.5f);

			int numtodraw = 6;
			for (int i = 0; i < numtodraw; i++)
			{
				float timer = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f;
				Vector2 center = NPC.Center + new Vector2(0, 8).RotatedBy(i * MathHelper.TwoPi / numtodraw) * timer;
				DrawMask(center, 1 - timer);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.SpawnTileY < Main.rockLayer && (Main.bloodMoon) && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<BloodGazer>()) ? SpawnCondition.OverworldNightMonster.Chance * 0.05f : 0f;
	}
}