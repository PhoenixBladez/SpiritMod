using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.BloodcourtSet;
using System;
using System.Collections.Generic;
using SpiritMod.Items.Weapon.Summon.SacrificialDagger;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.EventSystem;
using SpiritMod.Mechanics.EventSystem.Events;
using Terraria.Localization;
using SpiritMod.Items.Accessory.SanguineWardTree;

namespace SpiritMod.NPCs.Occultist
{
	[AutoloadBossHead]
	public class Occultist : SpiritNPC, IBCRegistrable, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Occultist");
			NPCID.Sets.TrailCacheLength[npc.type] = 6;
			NPCID.Sets.TrailingMode[npc.type] = 1;
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 42;
			npc.height = 56;

			npc.lifeMax = 1000;
			npc.defense = 14;
			npc.damage = 30;
			npc.HitSound = SoundID.DD2_SkeletonHurt;
			npc.DeathSound = SoundID.NPCDeath59;
			npc.aiStyle = -1;
			npc.value = 300f;
			npc.knockBackResist = 0.45f;
			npc.netAlways = true;
			npc.lavaImmune = true;
			banner = npc.type;
			npc.boss = true;
			music = MusicID.Eerie;
			bannerItem = ModContent.ItemType<Items.Banners.OccultistBanner>();
		}

		private ref float AiState => ref npc.ai[0];

		private const float AISTATE_SPAWN = 0;
		private const float AISTATE_DESPAWN = 1;
		private const float AISTATE_PHASE1 = 2;
		private const float AISTATE_PHASETRANSITION = 3;
		private const float AISTATE_PHASE2 = 4;
		private const float AISTATE_DEATH = 5;

		private ref float AttackType => ref npc.ai[1];

		private const float WAVEHANDS = 0;
		private const float SACDAGGERS = 1;
		private const float HOMINGSOULS = 2;
		private const float SUMMONBRUTE = 3;

		private ref float AiTimer => ref npc.ai[2];

		private ref float SecondaryCounter => ref npc.ai[3];

		private float _pulseGlowmask;
		private float _ritualCircle;
		private float _whiteGlow;

		private readonly RotatingObjectManager _rotMan = new RotatingObjectManager();
		private RuneCircle _runeCircle = null;

		private void UpdateAIState(float State)
		{
			AiState = State;
			AiTimer = 0;
			frame.Y = 0;
			SecondaryCounter = 0;
			npc.netUpdate = true;

			if (!Main.dedServ)
				_rotMan.KillAllObjects();
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.12f, .64f);
			Player target = Main.player[npc.target];

			if ((AiState == AISTATE_PHASE1 || AiState == AISTATE_PHASE2))
			{
				if(Main.dayTime)
					UpdateAIState(AISTATE_DESPAWN);

				if (target.dead || !target.active)
				{
					npc.TargetClosest(true); //look for another player
					if (target.dead || !target.active)
						UpdateAIState(AISTATE_DESPAWN); //despawn if still none alive
				}
			}

			if (AiState == AISTATE_PHASE1 && npc.life < (npc.lifeMax / 2))
				UpdateAIState(AISTATE_PHASETRANSITION);

			switch (AiState)
			{
				case AISTATE_SPAWN:
					npc.TargetClosest(true);
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = true;
					SpawnAnimation(target);
					break;

				case AISTATE_DESPAWN:
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = true;
					Despawn();
					break;

				case AISTATE_PHASE1:
					npc.noGravity = false;
					npc.noTileCollide = false;
					npc.dontTakeDamage = false;
					npc.velocity.X *= 0.9f;
					Phase1(target);
					break;

				case AISTATE_PHASETRANSITION:
					npc.TargetClosest(true);
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = true;
					PhaseTransition();
					break;

				case AISTATE_PHASE2:
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.dontTakeDamage = false;
					Phase2(target);
					break;

				case AISTATE_DEATH:
					npc.dontTakeDamage = true;
					npc.noTileCollide = true;
					DeathAnim();
					break;
			}
			++AiTimer;

			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new GlowParticle(npc.Center + Main.rand.NextVector2Circular(15, 20), -Vector2.UnitY * Main.rand.NextFloat(), Color.Red * 0.75f, Main.rand.NextFloat(0.02f, 0.04f), 60));

			if (!Main.dedServ)
			{
				if (_runeCircle != null)
					_runeCircle.Update(npc.velocity.Length(), npc.direction);

				_rotMan.UpdateObjects();
			}
		}

		#region Animations
		private void SpawnAnimation(Player target)
		{
			int animtime = 120;

			float halfanimtime = animtime / 2f;
			if (SecondaryCounter == 0)
			{
				if (!EventManager.IsPlaying<FollowNPCThenReturn>() && !Main.dedServ)
					EventManager.PlayEvent(new FollowNPCThenReturn(npc, 1.5f, (animtime / 60), 1.5f));

				AiTimer = 0;
				frame.X = 3;
				UpdateYFrame(8, 0, 6, delegate (int frameY) 
				{
					if(frameY == 6) //when anim is complete, increase counter and reset frame
					{
						frame = new Point(1, 0);
						SecondaryCounter++;
						npc.netUpdate = true;
					}
				});
			}
			else if(AiTimer <= animtime)
			{
				npc.velocity.Y = (float)Math.Sin(AiTimer / 12f);
				_pulseGlowmask = _ritualCircle = (halfanimtime - Math.Abs(halfanimtime - AiTimer)) / halfanimtime;

				if (Main.rand.NextBool(3) && !Main.dedServ)
				{
					Vector2 offset = Main.rand.NextVector2CircularEdge(200, 200) * _ritualCircle;
					ParticleHandler.SpawnParticle(new GlowParticle(npc.Center + offset, -offset/30f, Color.Magenta * 0.75f, Main.rand.NextFloat(0.06f, 0.12f) * _ritualCircle, 30));
				}

				UpdateYFrame(8, 0, 8, null, 4);
				if(AiTimer == animtime)
				{
					frame = new Point(2, 0);
					npc.netUpdate = true;
				}
			}
			else
			{
				_pulseGlowmask = _ritualCircle = 0;

				npc.velocity.Y = 0;
				UpdateYFrame(10, 0, 7, delegate (int frameY)
				{
					if (frameY == 7)
					{
						UpdateAIState(AISTATE_PHASE1);
						Teleport(target);
						frameY = 7;
					}
				});
			}
		}

		private void PhaseTransition()
		{
			//slow rise and make glowmask
			int RiseTime = 40;
			int ChargeTime = 200;
			int EndTime = 60;

			if (AiTimer <= RiseTime)
			{
				if (!EventManager.IsPlaying<FollowNPCThenReturn>() && !Main.dedServ)
					EventManager.PlayEvent(new FollowNPCThenReturn(npc, 1.5f, (RiseTime + ChargeTime + EndTime/2) / 60, 1.5f));

				frame.X = 1;
				UpdateYFrame(4, 0, 8, null, 4);
				npc.velocity = new Vector2(0, (float)-Math.Cos((AiTimer/RiseTime) * MathHelper.PiOver2) * 0.75f);
				_pulseGlowmask = MathHelper.Lerp(_pulseGlowmask, 1, 0.1f);
			}

			// make circle and create runes
			else if(AiTimer < (RiseTime + ChargeTime))
			{
				frame.X = 1;
				UpdateYFrame(7, 0, 8, null, 4);
				_ritualCircle = (AiTimer - RiseTime) / ChargeTime;
				_whiteGlow = 0.5f * (float)Math.Pow((AiTimer - RiseTime) / ChargeTime, 2);
				if (AiTimer % 4 == 0)
					AddRune();
			}
			//burst of souls and particles
			else if(AiTimer == RiseTime + ChargeTime)
			{
				for(int i = 0; i < 8; i++)
					VisualSoul(5f);

				if (!Main.dedServ)
				{
					_rotMan.KillAllObjects();
					for (int i = 0; i < 30; i++)
						ParticleHandler.SpawnParticle(new GlowParticle(npc.Center, Main.rand.NextVector2Circular(12, 12), new Color(99, 23, 51), Main.rand.NextFloat(0.04f, 0.08f), 40));

					for (int i = 0; i < 3; i++)
						ParticleHandler.SpawnParticle(new PulseCircle(npc.Center, new Color(99, 23, 51), 200 * i, 20));

					_runeCircle = new RuneCircle(80, 50, 10, 8);

					EventManager.PlayEvent(new ScreenShake(30f, 0.33f));
				}
			}

			else
			{
				frame.X = 0;
				UpdateYFrame(3, 0, 2);
				_ritualCircle = 0;
				_whiteGlow = 0; 

				if (Main.rand.NextBool() && !Main.dedServ)
					ParticleHandler.SpawnParticle(new GlowParticle(npc.Center + Main.rand.NextVector2Circular(15, 20), -Vector2.UnitY * Main.rand.NextFloat(), Color.Red * 0.75f * npc.Opacity, Main.rand.NextFloat(0.02f, 0.04f), 60));

				if (AiTimer > RiseTime + ChargeTime + EndTime)
				{
					SwapAttack();
					UpdateAIState(AISTATE_PHASE2);
				}
			}
		}

		private void Despawn()
		{
			int RiseTime = 40;
			_pulseGlowmask = 0;
			_ritualCircle = 0;
			if (AiTimer <= RiseTime)
			{
				frame.X = 0;
				UpdateYFrame(3, 0, 2);
				npc.velocity = new Vector2(0, (float)-Math.Cos((AiTimer / RiseTime) * MathHelper.PiOver2) * 0.75f);
			}

			else
			{
				frame.X = 2;
				UpdateYFrame(12, 0, 7, delegate (int frameY)
				{
					if (frameY == 7)
					{
						npc.active = false;
					}
				});
			}
		}

		private void DeathAnim()
		{
			int ChargeTime = 200;
			int FallTime = 60;

			if(AiTimer < ChargeTime)
			{
				if (!EventManager.IsPlaying<FollowNPCThenReturn>() && !Main.dedServ)
					EventManager.PlayEvent(new FollowNPCThenReturn(npc, 1.5f, (ChargeTime) / 60, 1.5f));

				npc.noGravity = true;
				npc.velocity.X = 0;
				_pulseGlowmask = Math.Max(_pulseGlowmask - 0.1f, 0);
				float halftime = ChargeTime / 2f;
				npc.velocity.Y = -(float)Math.Pow((halftime - Math.Abs(halftime - AiTimer))/halftime, 2);

				float speedMod = MathHelper.Lerp(1, 0.5f, AiTimer / ChargeTime);
				if (AiTimer % (int)(20 * speedMod) == 0)
					VisualSoul(2f);

				if (AiTimer % (int)(30 * speedMod) == 0 && !Main.dedServ)
					ParticleHandler.SpawnParticle(new PulseCircle(npc.Center, new Color(252, 3, 148, 100) * 0.5f, 120, 12));

				if(Main.rand.NextBool((int)(50 * Math.Pow(speedMod, 2))) && !Main.dedServ)
					ParticleHandler.SpawnParticle(new OccultistDeathBoom(npc.Center + Main.rand.NextVector2Circular(50, 60), Main.rand.NextFloat(0.2f, 0.3f), Main.rand.NextFloat(-0.1f, 0.1f)));

				_whiteGlow = 0.33f * AiTimer / ChargeTime;
				frame.X = 0;
				UpdateYFrame(4, 0, 2);
			}
			else if(AiTimer == ChargeTime)
			{
				NPCLoot();
				frame.X = 4;
				frame.Y = 0;
				_whiteGlow = 0;
				npc.velocity = new Vector2(-npc.direction * 6, -6);

				for (int i = 0; i < 12; i++)
					VisualSoul(6f);

				if (!Main.dedServ)
				{
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), CombatText.DamagedHostileCrit, 9999, true);
					Main.PlaySound(npc.DeathSound, npc.Center);
					ParticleHandler.SpawnParticle(new OccultistDeathBoom(npc.Center, 0.8f));

					//omnidirectional particle burst
					for (int i = 0; i < 30; i++)
						ParticleHandler.SpawnParticle(new GlowParticle(npc.Center, Main.rand.NextVector2Circular(12, 12), new Color(99, 23, 51), Main.rand.NextFloat(0.04f, 0.08f), 40));

					//vertical upwards particle burst
					for (int i = 0; i < 20; i++)
						ParticleHandler.SpawnParticle(new GlowParticle(npc.Center + Main.rand.NextVector2Circular(10, 20), -Vector2.UnitY * Main.rand.NextFloat(12), new Color(99, 23, 51), Main.rand.NextFloat(0.04f, 0.08f), 40));

					//pulsing circles
					for (int i = 0; i < 4; i++)
						ParticleHandler.SpawnParticle(new PulseCircle(npc.Center, new Color(99, 23, 51), 150 * i, 20));

					EventManager.PlayEvent(new ScreenShake(30f, 0.33f));
				}

				if (Main.netMode != NetmodeID.Server)
					Main.NewText(Language.GetTextValue("Announcement.HasBeenDefeated_Single", Lang.GetNPCNameValue(npc.type)), 175, 75);
				else
					NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasBeenDefeated_Single", Lang.GetNPCNameValue(npc.type)), new Color(175, 75, 0));
			}
			else
			{
				npc.noGravity = false;
				if (frame.Y != 5)
					UpdateYFrame(5 * (60 / FallTime), 0, 5);

				npc.alpha += 255 / FallTime;
				if (AiTimer > ChargeTime + FallTime)
				{
					npc.life = 0;
					npc.active = false;
				}
			}
		}

		#endregion

		private void AddRune()
		{
			if (!Main.dedServ)
			{
				Texture2D rune = mod.GetTexture("Textures/Runes");
				int framenum = Main.rand.Next(4);
				Rectangle frame = new Rectangle(0, framenum * (int)(rune.Height / 4f), rune.Width, (int)(rune.Height / 4f));
				float Scale = Main.rand.NextFloat(0.4f, 0.6f);
				float YPos = Main.rand.NextFloat(-13, 13);
				float Radius = Main.rand.NextFloat(20, 40);
				float Offset = Main.rand.Next(80);
				_rotMan.AddObject(rune, YPos, Radius, Scale, new Color(252, 3, 102), 60, frame, 50, Offset);
			}
		}

		private void VisualSoul(float Velocity)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 vel = Main.rand.NextVector2CircularEdge(Velocity, Velocity) * Main.rand.NextFloat(0.8f, 1.2f);

				Projectile.NewProjectileDirect(npc.Center + vel * Main.rand.NextFloat(2, 6), vel, ModContent.ProjectileType<OccultistSoul>(), 0, 1, Main.myPlayer).netUpdate = true;
			}
		}

		private void SwapAttack()
		{
			float newattack = AttackType;
			while (newattack == AttackType)
				newattack = Main.rand.Next(new float[] { WAVEHANDS, HOMINGSOULS, SACDAGGERS });

			AttackType = newattack;
		}

		#region Phase 1

		private void Phase1(Player target)
		{
			switch (SecondaryCounter)
			{
				case 0:
					npc.TargetClosest(true);
					AiTimer = 0;
					frame.X = 3;
					UpdateYFrame(10, 0, 6, delegate (int frameY)
					{
						if (frameY == 6)
						{
							frame.Y = 0;
							SecondaryCounter++;
						}
					});
					break;

				case 1:
					frame.X = 1;
					switch (AttackType)
					{
						case WAVEHANDS:
							WaveHandsP1(target);
							break;
						case SACDAGGERS:
							DaggersP1(target);
							break;
						case HOMINGSOULS:
							SoulsP1(target);
							break;
						case SUMMONBRUTE:
							WaveHandsP1(target);
							//	BruteP1(target);
							break;
					}

					break;

				case 2:
					frame.X = 2;
					UpdateYFrame(10, 0, 7, delegate (int frameY)
					{
						if (frameY == 7)
						{
							SwapAttack();
							Teleport(target);
							SecondaryCounter = 0;
						}
					});
					break;
			}
		}

		private void WaveHandsP1(Player target)
		{
			if(AiTimer < 40)
			{
				npc.TargetClosest(true);
				UpdateYFrame(8, 0, 8, null, 4);
				if (AiTimer % 6 == 0)
					AddRune();
				return;
			}
			UpdateYFrame(11, 4, 8, delegate (int frameY)
			{
				if ((frameY == 4 || frameY == 8) && Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 spawnPos = npc.Center + (Vector2.UnitX * npc.direction).RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(20, 40);
					float amplitude = Main.rand.NextFloat(5, 10);
					float periodOffset = Main.rand.Next(160);
					Projectile proj = Projectile.NewProjectileDirect(spawnPos, npc.direction * Vector2.UnitX * Main.rand.NextFloat(2, 3), ModContent.ProjectileType<OccultistHand>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, amplitude, periodOffset);
					proj.netUpdate = true;
				}
			});

			ResetAttackP1(100);
		}

		private void DaggersP1(Player target)
		{
			if (AiTimer < 60)
				npc.TargetClosest(true);
			else
				UpdateYFrame(8, 0, 8, null, 4);

			if ((AiTimer == 1 || AiTimer == 20) && !Main.dedServ)
			{
				Texture2D dagger = ModContent.GetTexture(Texture + "Dagger");
				Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
				float height = (AiTimer == 1) ? 0 : -10;
				float radius = (AiTimer == 1) ? 30 : 40;
				float scale = (AiTimer == 1) ? 0.25f : 0.5f;
				float opacity = (AiTimer == 1) ? 0.66f : 0.8f;
				float offset = (AiTimer == 1) ? 0 : 10;
				for(int i = 0; i < 3; i++)
				{
					_rotMan.AddObject(bloom, height, radius, scale / 3, Color.Pink * opacity * 0.66f, -1, null, 60, (40 * i) + offset);
					_rotMan.AddObject(dagger, height, radius, scale, Color.White * opacity, -1, null, 60, (40 * i) + offset);
				}
			}

			if (AiTimer == 60 && !Main.dedServ)
				_rotMan.KillAllObjects();

			if(AiTimer > 60 && AiTimer % 5 == 0 && AiTimer <= 90)
			{
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					float timer = AiTimer - 50;
					if (npc.direction > 0)
						timer = 40 - timer;

					float angle = (timer / 40) * MathHelper.Pi;
					Vector2 spawnPos = npc.Center - new Vector2(0, 500) + new Vector2(0, 300).RotatedBy((angle - MathHelper.PiOver2)/2.5f);
					Projectile proj = Projectile.NewProjectileDirect(spawnPos, Vector2.Zero, ModContent.ProjectileType<OccultistDagger>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, angle);
					proj.netUpdate = true;
				}
			}

			ResetAttackP1(120);
		}

		private void SoulsP1(Player Target)
		{
			npc.TargetClosest(true);

			UpdateYFrame(8, 4, 8);

			if (AiTimer % 12 == 0)
				VisualSoul(2.5f);

			if (AiTimer % 20 == 0 && !Main.dedServ)
				ParticleHandler.SpawnParticle(new PulseCircle(npc.Center, new Color(252, 3, 148, 100) * 0.5f, 120, 12));
				

			if(AiTimer % 15 == 0 && AiTimer <= 45)
			{
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 vel = npc.DirectionFrom(Target.Center).RotatedByRandom(MathHelper.PiOver2) * 4;
					Projectile.NewProjectileDirect(npc.Center + vel * Main.rand.NextFloat(10, 20), vel, ModContent.ProjectileType<OccultistSoul>(), NPCUtils.ToActualDamage(30, 1.5f), 1, Main.myPlayer, 1, Target.whoAmI).netUpdate = true;
				}
			}


			ResetAttackP1(120);
		}

		public void Teleport(Player player)
		{
			Point desiredPos;
			Point FindRandomPos() => (player.Center + new Vector2((Main.rand.NextBool() ? -1 : 1) * Main.rand.NextFloat(200, 300), Main.rand.NextFloat(-100, 0))).ToTileCoordinates();
			desiredPos = FindRandomPos();
			while (desiredPos.X < 0 || desiredPos.Y < 0 || desiredPos.X > Main.maxTilesX || desiredPos.Y > Main.maxTilesY) //out of bounds bad
				desiredPos = FindRandomPos();

			int numTries = 0;
			int maxTries = 1000;

			while (numTries < maxTries && ExtraUtils.CheckSolidTilesAndPlatforms(new Rectangle(desiredPos.X, desiredPos.Y, 1, 3))) //find a random point not within tiles
			{
				numTries++;
				desiredPos = FindRandomPos();
				while (desiredPos.X < 0 || desiredPos.Y < 0 || desiredPos.X > Main.maxTilesX || desiredPos.Y > Main.maxTilesY) //out of bounds still bad
					desiredPos = FindRandomPos();
			}

			while (desiredPos.Y < Main.maxTilesY && !ExtraUtils.CheckSolidTilesAndPlatforms(new Rectangle(desiredPos.X, desiredPos.Y + 4, 1, 0)))
				desiredPos.Y++;

			npc.position = desiredPos.ToWorldCoordinates();
			npc.netUpdate = true;
		}

		private void ResetAttackP1(int endTime)
		{
			if(AiTimer > endTime)
			{
				SecondaryCounter++;
				frame.Y = 0;
				npc.netUpdate = true;
			}
		}
		#endregion

		#region Phase 2
		private void Phase2(Player target)
		{
			int numAttacksPerCooldown = 3;
			if(SecondaryCounter < numAttacksPerCooldown)
			{
				switch (AttackType)
				{
					case WAVEHANDS:
						WaveHandsP2(target);
						break;
					case SACDAGGERS:
						DaggersP2(target);
						break;
					case HOMINGSOULS:
						SoulsP2(target);
						break;
					case SUMMONBRUTE:
						WaveHandsP2(target);
						//	BruteP2(target);
						break;
				}
			}
			else
			{
				npc.TargetClosest(true);
				float RestingTime = 150;
				float halfRestTime = RestingTime / 2;
				frame.X = 0;
				UpdateYFrame(4, 0, 2);
				_pulseGlowmask = (float)Math.Max(Math.Pow(Math.Abs(halfRestTime - AiTimer) / halfRestTime, 3) - 0.2f, 0);
				npc.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / halfRestTime) * 0.8f;
				npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.1f);
				if(AiTimer > RestingTime)
				{
					ResetAttackP2();
					SecondaryCounter = 0;
				}
			}
		}

		private void WaveHandsP2(Player target)
		{
			npc.TargetClosest(true);
			int TPTime = 60;
			if (AiTimer <= TPTime)
			{
				Vector2 TPPosition = target.Center + (target.velocity.X < 0 ? -1 : 1) * Vector2.UnitX * 300;
				TeleportP2(TPPosition, TPTime);
			}
			else
			{
				float timer = AiTimer - TPTime; //make timing a bit simpler
				if (timer % 6 == 0)
					AddRune();

				if (timer < 80) //home in on the side of the player that's closer
				{
					Vector2 DesiredPosition = target.Center + (target.DirectionTo(npc.Center).X < 0 ? -1 : 1) * Vector2.UnitX * 300;
					DesiredPosition.Y += (float)Math.Sin(Main.GameUpdateCount / 10f) * 20;
					float accel = MathHelper.Clamp(npc.Distance(DesiredPosition) / 400, 0.1f, 0.4f);
					npc.AccelFlyingMovement(DesiredPosition, accel, 0.1f, 30);
				}
				else
					npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.085f);

				if (timer > 80)
				{ 
					frame.X = 1;
					UpdateYFrame(8, 0, 8, delegate (int frameY)
					{
						if (frameY == 4 && Main.netMode != NetmodeID.MultiplayerClient && timer < 150)
						{
							Vector2 spawnPos = npc.Center + (Vector2.UnitX * npc.direction).RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(20, 40);
							float amplitude = 70;
							for (int i = -1; i <= 1; i++)
							{
								if (i == 0)
									continue;

								Projectile.NewProjectileDirect(spawnPos, npc.direction * Vector2.UnitX * 2.25f, ModContent.ProjectileType<OccultistHand2>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, amplitude * i, 60).netUpdate = true;
							}
							npc.velocity.X -= npc.direction * 5;
							npc.netUpdate = true;
						}
					}, 4);
				}
				else
				{
					frame.X = 0;
					UpdateYFrame(4, 0, 2);
					if(timer == 80)
						frame.Y = 0;
				}

				if (timer > 180)
					ResetAttackP2();
			}
		}

		private void DaggersP2(Player target)
		{
			int TPTime = 60;
			if (AiTimer <= TPTime)
			{
				npc.TargetClosest(true);
				Vector2 TPPosition = target.Center + ((target.velocity.X < 0 ? -1 : 1) * Vector2.UnitX * 550) - Vector2.UnitY * 220;
				TeleportP2(TPPosition, TPTime);
			}
			else
			{
				float timer = AiTimer - TPTime; //make timing a bit simpler
				float attacktime = 140;
				float daggerstarttime = 40;

				if (timer == 1 && !Main.dedServ)
				{
					Texture2D dagger = ModContent.GetTexture(Texture + "Dagger");
					Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
					float radius = 30;
					float scale = 0.5f;
					float opacity = 0.8f;
					for (int i = 0; i < 3; i++)
					{
						_rotMan.AddObject(bloom, 0, radius, scale / 3, Color.Pink * opacity * 0.66f, -1, null, 60, 40 * i, MathHelper.Pi);
						_rotMan.AddObject(dagger, 0, radius, scale, Color.White * opacity, -1, null, 60, 40 * i, MathHelper.Pi);
					}

					for (int j = 0; j < 5; j++)
					{
						_rotMan.AddObject(bloom, 10, radius * 1.5f, scale / 3, Color.Pink * opacity * 0.66f, -1, null, 60, 24 * j, MathHelper.Pi);
						_rotMan.AddObject(dagger, 10, radius * 1.5f, scale, Color.White * opacity, -1, null, 60, 24 * j, MathHelper.Pi);
					}
				}

				if (timer < daggerstarttime) //home in on the side of the player that's closer
				{
					Vector2 DesiredPosition = target.Center + ((target.DirectionTo(npc.Center).X < 0 ? -1 : 1) * Vector2.UnitX * 550) - Vector2.UnitY * 220;
					float accel = MathHelper.Clamp(npc.Distance(DesiredPosition) / 400, 0.1f, 0.4f);
					npc.AccelFlyingMovement(DesiredPosition, accel, 0.15f, 30);
				}
				else //move in direction it's facing, with quadratic easing formula to smooth out velocity
				{
					float progress = Math.Max((timer - daggerstarttime) / (attacktime - daggerstarttime), 0);

					npc.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / 90);
					npc.velocity.X = npc.direction * 22f * (progress < 0.5f ? 4 * (float)Math.Pow(progress, 2) : (float)Math.Pow((-2 * progress) + 2, 2));
				}

				if(timer % 7 == 0 && Main.netMode != NetmodeID.Server && timer > daggerstarttime)
				{
					Projectile proj = Projectile.NewProjectileDirect(npc.Center + Main.rand.NextVector2CircularEdge(30, 50), Vector2.Zero, 
						ModContent.ProjectileType<OccultistDagger>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, 
						Main.rand.NextFloat(-0.12f, 0.12f) + MathHelper.PiOver2);
					proj.netUpdate = true;
				}

				frame.X = 0;
				UpdateYFrame(7, 0, 2);

				if (timer > attacktime)
				{
					if (!Main.dedServ)
						_rotMan.KillAllObjects();

					ResetAttackP2();
				}
			}
		}

		private void SoulsP2(Player target)
		{
			npc.TargetClosest(true);
			int TPTime = 60;
			if (AiTimer <= TPTime)
			{
				npc.TargetClosest(true);
				Vector2 TPPosition = target.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(150, 200);
				TeleportP2(TPPosition, TPTime);
			}
			else
			{
				float timer = AiTimer - TPTime; //make timing a bit simpler
				float attacktime = 130;

				frame.X = 1;
				UpdateYFrame(8, 4, 8);

				npc.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / 90);
				npc.velocity.X = MathHelper.Lerp(npc.velocity.X, 0, 0.1f);
				if (timer % 10 == 0)
					VisualSoul(2.5f);

				if (AiTimer % 20 == 0 && !Main.dedServ)
					ParticleHandler.SpawnParticle(new PulseCircle(npc.Center, new Color(252, 3, 148, 100) * 0.5f, 120, 12));

				if (timer % 12 == 0 && timer <= 60)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 vel = npc.DirectionFrom(target.Center).RotatedByRandom(MathHelper.PiOver2) * 4;
						Projectile.NewProjectileDirect(npc.Center + vel * Main.rand.NextFloat(10, 20), vel, ModContent.ProjectileType<OccultistSoul>(), NPCUtils.ToActualDamage(30, 1.5f), 1, Main.myPlayer, 1, target.whoAmI).netUpdate = true;
					}
				}

				if (timer > attacktime)
					ResetAttackP2();
			}
		}

		private void ResetAttackP2()
		{
			SwapAttack();
			SecondaryCounter++;
			AiTimer = 0;
			frame.Y = 0;
			npc.velocity = Vector2.Zero;
			npc.netUpdate = true;
		}

		private void TeleportP2(Vector2 position, int time) //method to handle teleporting for at the start of the attack, as to reduce boilerplate
		{
			int halftime = time / 2;
			if (AiTimer <= halftime)
			{
				frame.X = 2;
				UpdateYFrame(7 * (60/halftime), 0, 7, delegate (int frameY)
				{
					if (frameY == 7)
					{
						npc.Center = position;
						npc.netUpdate = true;
						AiTimer = halftime;
					}
				});
			}
			else
			{
				frame.X = 3;
				UpdateYFrame(6 * (60 / halftime), 0, 6);
			}
		}

		#endregion

		#region Drawing

		private float DrawTimer => (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f;

		private void DrawTex(SpriteBatch sB, Texture2D tex, Color color, float scale = 1f, Vector2? position = null)
		{
			var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			sB.Draw(tex, (position ?? npc.Center) - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame, color * npc.Opacity, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Color glowColor = Color.Lerp(Color.Red, Color.Magenta, DrawTimer);

			if(npc.frame.Width > 72) //workaround for framing not working properly on first tick
			{
				frame = new Point(3, 0);
				npc.FindFrame();
			}

			//draw ritual circle and a bloom
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, npc.Center - Main.screenPosition, null, glowColor * _ritualCircle * 0.66f, 0, bloom.Size() / 2, _ritualCircle * 1.25f, SpriteEffects.None, 0);

			Texture2D circle = ModContent.GetTexture(Texture + "_circle");
			spriteBatch.Draw(circle, npc.Center - Main.screenPosition, null, glowColor * _ritualCircle * 0.75f, Main.GlobalTime * 2, circle.Size() / 2, _ritualCircle, SpriteEffects.None, 0);
			spriteBatch.Draw(circle, npc.Center - Main.screenPosition, null, glowColor * _ritualCircle * 0.75f, Main.GlobalTime * -2, circle.Size() / 2, _ritualCircle, SpriteEffects.None, 0);

			if (_rotMan != null)
				_rotMan.DrawBack(spriteBatch, npc.Center);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);



			Texture2D mask = ModContent.GetTexture(Texture + "_mask");

			for(int j = 0; j < NPCID.Sets.TrailCacheLength[npc.type]; j++)
			{
				float Opacity = (NPCID.Sets.TrailCacheLength[npc.type] - j) / (float)NPCID.Sets.TrailCacheLength[npc.type];
				DrawTex(spriteBatch, mask, glowColor * _pulseGlowmask * 0.5f * Opacity, 1f, npc.oldPos[j] + npc.Size/2);
			}

			//pulse glowmask effect
			for(int i = 0; i < 6; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * DrawTimer * 8;
				DrawTex(spriteBatch, mask, glowColor * (1 - DrawTimer) * _pulseGlowmask, 1f, npc.Center + offset);
			}
			DrawTex(spriteBatch, mask, glowColor * _pulseGlowmask, 1.1f);

			//normal drawing replacement
			DrawTex(spriteBatch, Main.npcTexture[npc.type], drawColor);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D glow = mod.GetTexture("NPCs/Occultist/Occultist_glow");
			for(int i = 0; i < 6; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * DrawTimer * 2;
				DrawTex(spriteBatch, glow, Color.White * 0.5f * (1.5f - DrawTimer), 1f, npc.Center + offset);
			}
			DrawTex(spriteBatch, glow, Color.White);

			Texture2D mask = ModContent.GetTexture(Texture + "_mask");
			DrawTex(spriteBatch, mask, Color.White * _whiteGlow);
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, npc.Center - Main.screenPosition, null, Color.White * _whiteGlow * 0.8f, 0, bloom.Size() / 2, _whiteGlow, SpriteEffects.None, 0);

			if (_rotMan != null)
				_rotMan.DrawFront(spriteBatch, npc.Center);

			if (_runeCircle != null)
			{
				Color runeColor = new Color(252, 3, 102) * 0.5f * _pulseGlowmask;
				switch (frame.X)
				{
					case 2:
						runeColor *= Math.Max((6 - frame.Y) / 6f, 0);
						break;
					case 3:
						runeColor *= Math.Max(frame.Y / 7f, 0);
						break;
				}
				_runeCircle.Draw(spriteBatch, npc.Center, runeColor);
			}
		}

		#endregion

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override bool CheckActive() => false; //uses custom despawn so not needed

		public override bool CheckDead()
		{
			if (AiState != AISTATE_DEATH)
			{
				UpdateAIState(AISTATE_DEATH);
				npc.life = 1;
				npc.dontTakeDamage = true;
				return false;
			}

			return true;
		}

		public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            MyWorld.downedOccultist = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
            return true;
        }

		public override void SafeFindFrame(int frameHeight) => npc.frame.Width = 72;

		//public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && !NPC.AnyNPCs(ModContent.NPCType<Occultist>()) && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedAncientFlier || MyWorld.downedReachBoss || MyWorld.downedMoonWizard || MyWorld.downedRaider) ? 0.03f : 0f;
		
		public override void NPCLoot()
		{
			string[] lootTable = { "Handball", "SacrificialDagger", "BloodWard" };
			int loot = Main.rand.Next(lootTable.Length);
			{
				npc.DropItem(mod.ItemType(lootTable[loot]));
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BloodFire>(), 4 + Main.rand.Next(3, 5));
		}

		public override void SafeHitEffect(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			Main.PlaySound(SoundID.NPCHit, npc.Center, 2);
			for(int i = 0; i < 3; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(npc.Center + Main.rand.NextVector2Circular(15, 20), 
					(Vector2.UnitX * hitDirection).RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(2, 3), Color.Red, Main.rand.NextFloat(0.02f, 0.04f), 30));
		}

		public override void OnHitKill(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			for (int i = 0; i < 30; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(npc.Center + Main.rand.NextVector2Circular(15, 20),
					Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), Color.Red, Main.rand.NextFloat(0.03f, 0.05f), 30));

			//for (int j = 0; j < 12; j++)
				//Gore.NewGore(npc.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(6), mod.GetGoreSlot("Gores/Skelet/grave" + Main.rand.Next(1, 5)));
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 1.5f;
			name = "Occultist";
			downedCondition = () => MyWorld.downedOccultist;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Occultist>()
				},
				null,
				null,
				new List<int> {
					ModContent.ItemType<Handball>(),
					ModContent.ItemType<SacrificialDagger>(),
					ModContent.ItemType<BloodWard>(),
					ModContent.ItemType<BloodFire>()
				});
			spawnInfo =
				"The Occultist spawns rarely during a Blood Moon after any prehardmode boss has been defeated.";
			texture = "SpiritMod/Textures/BossChecklist/OccultistTexture";
			headTextureOverride = "SpiritMod/NPCs/BloodMoon/Occultist_Head_Boss";
		}
	}
}