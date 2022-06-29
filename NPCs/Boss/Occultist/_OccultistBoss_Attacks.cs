using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.NPCs.Boss.Occultist.Particles;
using SpiritMod.NPCs.Boss.Occultist.Projectiles;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist
{
	public partial class OccultistBoss
	{
		private const float WAVEHANDS = 0;
		private const float SACDAGGERS = 1;
		private const float HOMINGSOULS = 2;
		private const float SUMMONBRUTE = 3;

		private void SwapAttack()
		{
			float newattack = AttackType;
			while (newattack == AttackType)
				newattack = Main.rand.Next(new float[] { WAVEHANDS, HOMINGSOULS, SACDAGGERS, SUMMONBRUTE });

			AttackType = newattack;
		}

		#region Phase 1

		private void WaveHandsP1(Player target)
		{
			int chargetime = 40;
			int attacktelegraphtime = 30;
			int attacktime = 30;

			if (AiTimer < chargetime)
			{
				NPC.TargetClosest(true);
				if (AiTimer % 6 == 0)
					AddRune();
			}

			if (AiTimer == chargetime && !Main.dedServ)
				ParticleHandler.SpawnParticle(new OccultistTelegraphBeam(NPC, (Vector2.UnitX * NPC.direction).ToRotation(), 400, attacktelegraphtime));

			if (AiTimer > attacktelegraphtime + chargetime)
			{
				UpdateYFrame(11, 4, 8);
				if (AiTimer % 6 == 0)
				{
					Vector2 spawnPos = NPC.Center + (Vector2.UnitX * NPC.direction).RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(20, 40);
					Vector2 velocity = NPC.direction * Vector2.UnitX * Main.rand.NextFloat(3, 4);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float amplitude = Main.rand.NextFloat(5, 8);
						float periodOffset = Main.rand.Next(160);
						var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, velocity, ModContent.ProjectileType<OccultistHand>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, amplitude, periodOffset);
						proj.netUpdate = true;
					}
				}
			}
			else
				UpdateYFrame(8, 0, 8, null, 4);

			ResetAttackP1(attacktelegraphtime + chargetime + attacktime);
		}

		private void DaggersP1(Player target)
		{
			int daggerspintime = 40;

			if (AiTimer < daggerspintime)
				NPC.TargetClosest(true);
			else
				UpdateYFrame(8, 0, 8, null, 4);

			if ((AiTimer == 1 || AiTimer == 20) && !Main.dedServ)
			{
				Texture2D dagger = Mod.Assets.Request<Texture2D>("NPCs/Boss/Occultist/Projectiles/OccultistDagger").Value;
				Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
				float height = AiTimer == 1 ? 0 : -10;
				float radius = AiTimer == 1 ? 30 : 40;
				float scale = AiTimer == 1 ? 0.25f : 0.5f;
				float opacity = AiTimer == 1 ? 0.66f : 0.8f;
				float offset = AiTimer == 1 ? 0 : 10;
				for (int i = 0; i < 3; i++)
				{
					_rotMan.AddObject(bloom, height, radius, scale / 3, Color.Pink * opacity * 0.66f, -1, null, 60, 40 * i + offset);
					_rotMan.AddObject(dagger, height, radius, scale, Color.White * opacity, -1, null, 60, 40 * i + offset);
				}
			}

			if (AiTimer == daggerspintime && !Main.dedServ)
				_rotMan.KillAllObjects();

			if (AiTimer > daggerspintime && AiTimer % 5 == 0 && AiTimer <= daggerspintime + 30)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float timer = AiTimer - daggerspintime + 10;
					if (NPC.direction > 0)
						timer = 40 - timer;

					float angle = timer / 40 * MathHelper.Pi;
					Vector2 spawnPos = NPC.Center - new Vector2(0, 500) + new Vector2(0, 300).RotatedBy((angle - MathHelper.PiOver2) / 2.5f);
					var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, Vector2.Zero, ModContent.ProjectileType<OccultistDagger>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, angle);
					proj.netUpdate = true;
				}
			}

			ResetAttackP1(daggerspintime + 60);
		}

		private void SoulsP1(Player Target)
		{
			NPC.TargetClosest(true);

			if (AiTimer <= 30)
			{
				UpdateYFrame(8, 4, 8);
				if (AiTimer % 15 == 0)
				{
					Vector2 vel = NPC.DirectionFrom(Target.Center).RotatedByRandom(MathHelper.PiOver2) * 14;
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + vel, vel.RotatedByRandom(MathHelper.Pi / 12), ModContent.ProjectileType<OccultistSoul>(), NPCUtils.ToActualDamage(30, 1.5f), 1, Main.myPlayer, Main.rand.NextBool() ? -1 : 1, Target.whoAmI).netUpdate = true;

					if (!Main.dedServ)
					{
						ParticleHandler.SpawnParticle(new PulseCircle(NPC.Center, new Color(252, 3, 148) * 0.6f, 120, 15) { RingColor = new Color(255, 115, 239) * 0.4f });
						ParticleHandler.SpawnParticle(new StarParticle(NPC.Center + vel, vel / 8, Color.Lerp(Color.Red, Color.White, 0.15f), 0.5f, 20, 4f));
						for (int i = 0; i < 4; i++)
							ParticleHandler.SpawnParticle(new OccultistSoulVisual(NPC.Center + vel * Main.rand.NextFloat(), vel.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.2f, 0.7f),
								Main.rand.NextFloat(0.4f, 0.5f), 30));
					}
				}
			}
			else
				UpdateYFrame(8, 8, 0, null, 0);

			ResetAttackP1(150);
		}

		private void BruteP1(Player Target)
		{
			int chargeTime = 40;
			float circleOpacity = 0.66f;
			int bruteSlamTime = BruteSlam.TOTALTIME;
			int restTime = 60;

			if (AiTimer < chargeTime)
				NPC.TargetClosest(true);

			if (AiTimer <= chargeTime + bruteSlamTime)
			{
				UpdateYFrame(8, 0, 8, null, 4);
				_ritualCircle = Math.Min(_ritualCircle + circleOpacity / chargeTime, circleOpacity);

				if (AiTimer == chargeTime)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Bottom, Vector2.Zero, ModContent.ProjectileType<BruteSlam>(), NPCUtils.ToActualDamage(60, 1.5f), 1, Main.myPlayer, NPC.whoAmI);
						proj.position.Y -= proj.height * 0.66f;
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
					}
				}
			}
			else
			{
				_ritualCircle = Math.Max(_ritualCircle - circleOpacity / restTime, 0f);
				UpdateYFrame(8, 8, 0, null, 0);
			}

			ResetAttackP1(chargeTime + bruteSlamTime + restTime);
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

			while (numTries < maxTries && ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(desiredPos.X, desiredPos.Y, 1, 3))) //find a random point not within tiles
			{
				numTries++;
				desiredPos = FindRandomPos();
				while (desiredPos.X < 0 || desiredPos.Y < 0 || desiredPos.X > Main.maxTilesX || desiredPos.Y > Main.maxTilesY) //out of bounds still bad
					desiredPos = FindRandomPos();
			}

			while (desiredPos.Y < Main.maxTilesY && !ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(desiredPos.X, desiredPos.Y + 4, 1, 0)))
				desiredPos.Y++;

			NPC.position = desiredPos.ToWorldCoordinates();
			NPC.netUpdate = true;
		}

		private void ResetAttackP1(int endTime)
		{
			if (AiTimer > endTime)
			{
				SecondaryCounter++;
				frame.Y = 0;
				NPC.netUpdate = true;
			}
		}

		#endregion Phase 1

		#region Phase 2

		private void WaveHandsP2(Player target)
		{
			NPC.TargetClosest(true);
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
					Vector2 DesiredPosition = target.Center + (target.DirectionTo(NPC.Center).X < 0 ? -1 : 1) * Vector2.UnitX * 300;
					DesiredPosition.Y += (float)Math.Sin(Main.GameUpdateCount / 10f) * 20;
					float accel = MathHelper.Clamp(NPC.Distance(DesiredPosition) / 400, 0.1f, 0.4f);
					NPC.AccelFlyingMovement(DesiredPosition, accel, 0.1f, 30);
				}
				else
					NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.085f);

				if (timer > 80)
				{
					frame.X = 1;
					UpdateYFrame(8, 0, 8, delegate (int frameY)
					{
						if (frameY == 4 && Main.netMode != NetmodeID.MultiplayerClient && timer < 150)
						{
							Vector2 spawnPos = NPC.Center + (Vector2.UnitX * NPC.direction).RotatedByRandom(MathHelper.Pi / 4) * Main.rand.NextFloat(20, 40);
							float amplitude = 70;
							for (int i = -1; i <= 1; i++)
							{
								if (i == 0)
									continue;

								Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), spawnPos, NPC.direction * Vector2.UnitX * 2.25f, ModContent.ProjectileType<OccultistHandFiery>(), NPCUtils.ToActualDamage(40, 1.5f), 1f, Main.myPlayer, amplitude * i, 60).netUpdate = true;
							}
							NPC.velocity.X -= NPC.direction * 5;
							NPC.netUpdate = true;
						}
					}, 4);
				}
				else
				{
					frame.X = 0;
					UpdateYFrame(4, 0, 2);
					if (timer == 80)
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
				NPC.TargetClosest(true);
				Vector2 TPPosition = target.Center + (target.velocity.X < 0 ? -1 : 1) * Vector2.UnitX * 550 - Vector2.UnitY * 220;
				TeleportP2(TPPosition, TPTime);
			}
			else
			{
				float timer = AiTimer - TPTime; //make timing a bit simpler
				float attacktime = 140;
				float daggerstarttime = 40;

				if (timer == 1 && !Main.dedServ)
				{
					Texture2D dagger = Mod.Assets.Request<Texture2D>("NPCs/Boss/Occultist/Projectiles/OccultistDagger").Value;
					Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
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
					Vector2 DesiredPosition = target.Center + (target.DirectionTo(NPC.Center).X < 0 ? -1 : 1) * Vector2.UnitX * 550 - Vector2.UnitY * 220;
					float accel = MathHelper.Clamp(NPC.Distance(DesiredPosition) / 400, 0.1f, 0.4f);
					NPC.AccelFlyingMovement(DesiredPosition, accel, 0.15f, 30);
				}
				else //move in direction it's facing, with quadratic easing formula to smooth out velocity
				{
					float progress = Math.Max((timer - daggerstarttime) / (attacktime - daggerstarttime), 0);

					NPC.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / 90);
					NPC.velocity.X = NPC.direction * 22f * (progress < 0.5f ? 4 * (float)Math.Pow(progress, 2) : (float)Math.Pow(-2 * progress + 2, 2));
				}

				if (timer % 7 == 0 && Main.netMode != NetmodeID.Server && timer > daggerstarttime)
				{
					var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2CircularEdge(30, 50), Vector2.Zero,
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
			NPC.TargetClosest(true);
			int TPTime = 60;
			if (AiTimer <= TPTime)
			{
				NPC.TargetClosest(true);
				Vector2 TPPosition = target.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(150, 200);
				TeleportP2(TPPosition, TPTime);
			}
			else
			{
				float timer = AiTimer - TPTime; //make timing a bit simpler
				float attacktime = 130;

				frame.X = 1;
				UpdateYFrame(8, 4, 8);

				NPC.velocity.Y = (float)Math.Sin(MathHelper.TwoPi * AiTimer / 90);
				NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, 0, 0.1f);
				if (timer % 10 == 0)
					VisualSoul(2.5f);

				if (AiTimer % 20 == 0 && !Main.dedServ)
					ParticleHandler.SpawnParticle(new PulseCircle(NPC.Center, new Color(252, 3, 148, 100) * 0.5f, 120, 12));

				if (timer % 12 == 0 && timer <= 60)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Vector2 vel = NPC.DirectionFrom(target.Center).RotatedByRandom(MathHelper.PiOver2) * 4;
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + vel * Main.rand.NextFloat(10, 20), vel, ModContent.ProjectileType<OccultistSoul>(), NPCUtils.ToActualDamage(30, 1.5f), 1, Main.myPlayer, NPC.whoAmI, target.whoAmI).netUpdate = true;
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
			NPC.velocity = Vector2.Zero;
			NPC.netUpdate = true;
		}

		private void TeleportP2(Vector2 position, int time) //method to handle teleporting for at the start of the attack, as to reduce boilerplate
		{
			int halftime = time / 2;
			if (AiTimer <= halftime)
			{
				frame.X = 2;
				UpdateYFrame(7 * (60 / halftime), 0, 7, delegate (int frameY)
				  {
					  if (frameY == 7)
					  {
						  NPC.Center = position;
						  NPC.netUpdate = true;
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

		#endregion Phase 2
	}
}