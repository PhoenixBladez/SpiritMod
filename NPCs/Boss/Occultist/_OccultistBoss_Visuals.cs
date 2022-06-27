using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.EventSystem;
using SpiritMod.Mechanics.EventSystem.Events;
using Terraria.Localization;
using SpiritMod.NPCs.Boss.Occultist.Particles;

namespace SpiritMod.NPCs.Boss.Occultist
{
	public partial class OccultistBoss : IDrawAdditive
	{
		private float _pulseGlowmask;
		private float _ritualCircle;
		private float _whiteGlow;

		private readonly RotatingObjectManager _rotMan = new RotatingObjectManager();
		private RuneCircle _runeCircle = null;

		public override void PostAI()
		{
			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center + Main.rand.NextVector2Circular(15, 20), -Vector2.UnitY * Main.rand.NextFloat(), Color.Red * 0.75f, Main.rand.NextFloat(0.02f, 0.04f), 60));

			if (!Main.dedServ)
			{
				if (_runeCircle != null)
					_runeCircle.Update(NPC.velocity.Length(), NPC.direction, 1f);

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
				//if (!EventManager.IsPlaying<FollowNPCThenReturn>() && !Main.dedServ) //Plays the spawn animation
				//	EventManager.PlayEvent(new FollowNPCThenReturn(npc, 1.5f, animtime / 60, 1.5f));

				AiTimer = 0;
				frame.X = 3;
				UpdateYFrame(10, 0, 6, delegate (int frameY)
				{
					if (frameY == 6) //when anim is complete, increase counter and reset frame
					{
						frame = new Point(1, 0);
						SecondaryCounter++;
						NPC.netUpdate = true;
					}
				});
			}
			else if (AiTimer <= animtime)
			{
				NPC.velocity.Y = (float)Math.Sin(AiTimer / 12f);
				_pulseGlowmask = _ritualCircle = (halfanimtime - Math.Abs(halfanimtime - AiTimer)) / halfanimtime;

				if (Main.rand.NextBool(3) && !Main.dedServ)
				{
					Vector2 offset = Main.rand.NextVector2CircularEdge(200, 200) * _ritualCircle;
					ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center + offset, -offset / 30f, Color.Magenta * 0.75f, Main.rand.NextFloat(0.06f, 0.12f) * _ritualCircle, 30));
				}

				UpdateYFrame(8, 0, 8, null, 4);
				if (AiTimer == animtime)
				{
					frame = new Point(2, 0);
					NPC.netUpdate = true;
				}
			}
			else
			{
				_pulseGlowmask = _ritualCircle = 0;

				NPC.velocity.Y = 0;
				UpdateYFrame(12, 0, 7, delegate (int frameY)
				{
					if (frameY == 7)
					{
						UpdateAIState(AISTATE_PHASE1);
						Teleport(target);
						frame.X = 3;
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
					EventManager.PlayEvent(new FollowNPCThenReturn(NPC, 1.5f, (RiseTime + ChargeTime + EndTime / 2) / 60, 1.5f));

				frame.X = 1;
				UpdateYFrame(4, 0, 8, null, 4);
				NPC.velocity = new Vector2(0, (float)-Math.Cos((AiTimer / RiseTime) * MathHelper.PiOver2) * 0.75f);
				_pulseGlowmask = MathHelper.Lerp(_pulseGlowmask, 1, 0.1f);
			}

			// make circle and create runes
			else if (AiTimer < (RiseTime + ChargeTime))
			{
				frame.X = 1;
				UpdateYFrame(7, 0, 8, null, 4);
				_ritualCircle = (AiTimer - RiseTime) / ChargeTime;
				_whiteGlow = 0.66f * (float)Math.Pow((AiTimer - RiseTime) / ChargeTime, 2);
				if (AiTimer % 4 == 0)
					AddRune();
			}
			//burst of souls and particles
			else if (AiTimer == RiseTime + ChargeTime)
			{
				for (int i = 0; i < 8; i++)
					VisualSoul(5f);

				if (!Main.dedServ)
				{
					_rotMan.KillAllObjects();
					for (int i = 0; i < 30; i++)
						ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center, Main.rand.NextVector2Circular(12, 12), new Color(99, 23, 51), Main.rand.NextFloat(0.04f, 0.08f), 40));

					for (int i = 0; i < 3; i++)
						ParticleHandler.SpawnParticle(new PulseCircle(NPC.Center, new Color(255, 33, 66) * 0.7f, 200 * i, 20) {RingColor = new Color(255, 115, 239) });

					_runeCircle = new RuneCircle(80, 50, 10, 8);

					EventManager.PlayEvent(new ScreenShake(30f, 0.33f));
					EventManager.PlayEvent(new ScreenFlash(new Color(255, 99, 161), 0.1f, 0.23f, 0.3f));
				}
			}

			else
			{
				frame.X = 0;
				UpdateYFrame(3, 0, 2);
				_ritualCircle = 0;
				_whiteGlow = 0;

				if (Main.rand.NextBool() && !Main.dedServ)
					ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center + Main.rand.NextVector2Circular(15, 20), -Vector2.UnitY * Main.rand.NextFloat(), Color.Red * 0.75f * NPC.Opacity, Main.rand.NextFloat(0.02f, 0.04f), 60));

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
				NPC.velocity = new Vector2(0, (float)-Math.Cos((AiTimer / RiseTime) * MathHelper.PiOver2) * 0.75f);
			}

			else
			{
				frame.X = 2;
				UpdateYFrame(12, 0, 7, delegate (int frameY)
				{
					if (frameY == 7)
						NPC.active = false;
				});
			}
		}

		private void DeathAnim()
		{
			int ChargeTime = 140;
			int FallTime = 60;

			if (AiTimer < ChargeTime)
			{
				//if (!EventManager.IsPlaying<FollowNPCThenReturn>() && !Main.dedServ) //Starts real death anim
				//	EventManager.PlayEvent(new FollowNPCThenReturn(npc, 1.5f, (ChargeTime) / 60, 1.5f));

				NPC.noGravity = true;
				NPC.velocity.X = 0;
				_pulseGlowmask = Math.Max(_pulseGlowmask - 0.1f, 0);
				float halftime = ChargeTime / 2f;
				NPC.velocity.Y = -(float)Math.Pow((halftime - Math.Abs(halftime - AiTimer)) / halftime, 2);

				float speedMod = MathHelper.Lerp(1, 0.5f, AiTimer / ChargeTime);
				if (AiTimer % (int)(20 * speedMod) == 0)
					VisualSoul(2f);

				if (AiTimer % (int)(30 * speedMod) == 0 && !Main.dedServ)
				{
					ParticleHandler.SpawnParticle(new PulseCircle(NPC.Center, new Color(255, 33, 66) * 0.4f, 120, 12) { RingColor = new Color(255, 115, 239) * 0.4f});
					ParticleHandler.SpawnParticle(new OccultistDeathBoom(NPC.Center + Main.rand.NextVector2Circular(50, 60), Main.rand.NextFloat(0.2f, 0.3f), Main.rand.NextFloat(-0.1f, 0.1f)));
				}

				_whiteGlow = (float)Math.Pow(AiTimer / ChargeTime, 2);
				frame = new Point(4, 1);
			}
			else if (AiTimer == ChargeTime)
			{
				NPCLoot();
				frame.X = 4;
				frame.Y = 0;
				_whiteGlow = 0;
				NPC.velocity = new Vector2(-NPC.direction * 6, -6);

				for (int i = 0; i < 12; i++)
					VisualSoul(6f);

				if (!Main.dedServ)
				{
					CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), CombatText.DamagedHostileCrit, 9999, true);
					SoundEngine.PlaySound(NPC.DeathSound, NPC.Center);
					ParticleHandler.SpawnParticle(new OccultistDeathBoom(NPC.Center, 0.8f));

					//omnidirectional particle burst
					for (int i = 0; i < 30; i++)
						ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center, Main.rand.NextVector2Circular(12, 12), new Color(99, 23, 51), Main.rand.NextFloat(0.04f, 0.08f), 40));

					//vertical upwards particle burst
					for (int i = 0; i < 20; i++)
						ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center + Main.rand.NextVector2Circular(10, 20), -Vector2.UnitY * Main.rand.NextFloat(12), new Color(99, 23, 51), Main.rand.NextFloat(0.04f, 0.08f), 40));

					//pulsing circles
					for (int i = 0; i < 4; i++)
						ParticleHandler.SpawnParticle(new PulseCircle(NPC.Center, new Color(255, 33, 66) * 0.7f, 150 * i, 20) { RingColor = new Color(255, 115, 239) });

					EventManager.PlayEvent(new ScreenShake(30f, 0.33f));
					EventManager.PlayEvent(new ScreenFlash(new Color(255, 99, 161), 0.1f, 0.23f, 0.3f));
				}

				if (Main.netMode != NetmodeID.Server)
					Main.NewText(Language.GetTextValue("Announcement.HasBeenDefeated_Single", Lang.GetNPCNameValue(NPC.type)), 175, 75);
				else
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasBeenDefeated_Single", Lang.GetNPCNameValue(NPC.type)), new Color(175, 75, 0));
			}
			else
			{
				NPC.noGravity = false;
				if (frame.Y != 5)
					UpdateYFrame(5 * (60 / FallTime), 0, 5);

				NPC.alpha += 255 / FallTime;
				if (AiTimer > ChargeTime + FallTime)
				{
					NPC.life = 0;
					NPC.active = false;
				}
			}
		}

		#endregion

		private void AddRune()
		{
			if (!Main.dedServ)
			{
				Texture2D rune = Mod.GetTexture("Textures/Runes");
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
			if (!Main.dedServ)
			{
				Vector2 vel = Main.rand.NextVector2CircularEdge(Velocity, Velocity) * Main.rand.NextFloat(0.8f, 1.2f);

				ParticleHandler.SpawnParticle(new OccultistSoulVisual(NPC.Center + vel * Main.rand.NextFloat(4, 6), vel, Main.rand.NextFloat(0.4f, 0.5f), 60));
			}
		}

		public override void SafeHitEffect(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			SoundEngine.PlaySound(SoundID.NPCHit, NPC.Center, 2);
			for (int i = 0; i < 3; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center + Main.rand.NextVector2Circular(15, 20),
					(Vector2.UnitX * hitDirection).RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(2, 3), Color.Red, Main.rand.NextFloat(0.02f, 0.04f), 30));
		}

		public override void OnHitKill(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			for (int i = 0; i < 30; i++)
				ParticleHandler.SpawnParticle(new GlowParticle(NPC.Center + Main.rand.NextVector2Circular(15, 20),
					Main.rand.NextVector2Unit() * Main.rand.NextFloat(4), Color.Red, Main.rand.NextFloat(0.03f, 0.05f), 30));

			//for (int j = 0; j < 12; j++)
			//Gore.NewGore(npc.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(6), mod.GetGoreSlot("Gores/Skelet/grave" + Main.rand.Next(1, 5)));
		}

		#region Drawing

		private float DrawTimer => (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f;

		private void DrawTex(SpriteBatch sB, Texture2D tex, Color color, float scale = 1f, Vector2? position = null)
		{
			var effects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			sB.Draw(tex, (position ?? NPC.Center) - Main.screenPosition + new Vector2(0, NPC.gfxOffY),
				NPC.frame, color * NPC.Opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Color glowColor = Color.Lerp(Color.Red, Color.Magenta, DrawTimer);

			if (NPC.frame.Width > 72) //workaround for framing not working properly on first tick
			{
				frame = new Point(3, 0);
				NPC.FindFrame();
			}

			//draw ritual circle and a bloom
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Texture2D bloom = Mod.GetTexture("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, NPC.Center - Main.screenPosition, null, glowColor * _ritualCircle * 0.66f, 0, bloom.Size() / 2, _ritualCircle * 1.25f, SpriteEffects.None, 0);

			Texture2D circle = ModContent.Request<Texture2D>(Texture + "_circle");
			spriteBatch.Draw(circle, NPC.Center - Main.screenPosition, null, glowColor * _ritualCircle * 0.75f, Main.GlobalTimeWrappedHourly * 2, circle.Size() / 2, _ritualCircle, SpriteEffects.None, 0);
			spriteBatch.Draw(circle, NPC.Center - Main.screenPosition, null, glowColor * _ritualCircle * 0.75f, Main.GlobalTimeWrappedHourly * -2, circle.Size() / 2, _ritualCircle, SpriteEffects.None, 0);

			//bloom for phase transition/death anim glow
			spriteBatch.Draw(bloom, NPC.Center - Main.screenPosition, null, Color.White * _whiteGlow * 0.8f, 0, bloom.Size() / 2, 0.5f, SpriteEffects.None, 0);

			if (_rotMan != null)
				_rotMan.DrawBack(spriteBatch, NPC.Center);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);



			Texture2D mask = ModContent.Request<Texture2D>(Texture + "_mask");

			for (int j = 0; j < NPCID.Sets.TrailCacheLength[NPC.type]; j++)
			{
				float Opacity = (NPCID.Sets.TrailCacheLength[NPC.type] - j) / (float)NPCID.Sets.TrailCacheLength[NPC.type];
				DrawTex(spriteBatch, TextureAssets.Npc[NPC.type].Value, glowColor * _pulseGlowmask * Opacity, 1f, NPC.oldPos[j] + NPC.Size / 2);
			}

			//pulse glowmask effect
			for (int i = 0; i < 6; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * DrawTimer * 8;
				DrawTex(spriteBatch, mask, glowColor * (1 - DrawTimer) * _pulseGlowmask, 1f, NPC.Center + offset);
			}
			DrawTex(spriteBatch, mask, glowColor * _pulseGlowmask, 1.1f);

			//normal drawing replacement
			DrawTex(spriteBatch, TextureAssets.Npc[NPC.type].Value, drawColor);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D glow = ModContent.Request<Texture2D>(Texture + "_glow");
			for (int i = 0; i < 6; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * DrawTimer * 2;
				DrawTex(spriteBatch, glow, Color.White * 0.5f * (1.5f - DrawTimer), 1f, NPC.Center + offset);
			}
			DrawTex(spriteBatch, glow, Color.White);

			Texture2D mask = ModContent.Request<Texture2D>(Texture + "_mask");
			DrawTex(spriteBatch, mask, Color.Black * _whiteGlow);
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (_rotMan != null)
				_rotMan.DrawFront(spriteBatch, NPC.Center);

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
				_runeCircle.Draw(spriteBatch, NPC.Center, runeColor);
			}
		}

		#endregion
	}
}