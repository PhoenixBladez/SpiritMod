using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Items;
using SpiritMod.Items.Equipment.AuroraSaddle;
using SpiritMod.Mechanics.EventSystem;
using SpiritMod.Mechanics.EventSystem.Events;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.Particles;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.AuroraStag
{
	public class AuroraStag : ModNPC//, IDrawAdditive
	{
		private float WalkSpeed => 2f;

		private float RunSpeed => 8f;

		public static float TameAnimationLength => 200;

		public static float ParticleAnticipationTime => TameAnimationLength * 0.2f;

		public static float ParticleReturnTime => TameAnimationLength * 0.1f;

		private float Brightness => (float)Math.Pow((TameAnimationLength - TameAnimationTimer) / TameAnimationLength, 30);

		// the time left before the stag starts moving again if it is standing still, or the time left until it stops if it is moving
		// ignored if the stag is alerted or scared
		private ref float TimeBeforeNextAction => ref NPC.ai[2];

		private bool Walking {
			get => NPC.ai[3] == 1;
			set {
				if (value)
					NPC.ai[3] = 1;
				else
					NPC.ai[3] = 0;
			}
		}

		public bool Scared
		{
			get => NPC.ai[0] == 1;
			private set
			{
				if (value)
					NPC.ai[0] = 1;
				else
					NPC.ai[0] = 0;
			}
		}

		public bool Alerted
		{
			get => NPC.ai[0] == 2;
			private set
			{
				if (value)
					NPC.ai[0] = 2;
				else
					NPC.ai[0] = 0;
			}
		}

		public float TameAnimationTimer
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Stag");
			Main.npcFrameCount[NPC.type] = 10;
			NPC.frame.Width = 402;
		}

		public override void SetDefaults()
		{
			NPC.width = 80;
			NPC.height = 100;
			NPC.defense = 12;
			NPC.lifeMax = 800;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 800;
			NPC.knockBackResist = .85f;
			NPC.aiStyle = -1;
			NPC.chaseable = false;

			for (int i = 0; i < BuffLoader.BuffCount; i++)
				NPC.buffImmune[i] = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.Player.ZoneSnow && MyWorld.aurora && Main.hardMode;
			if (NPC.AnyNPCs(NPC.type) || !valid)
				return 0f;
			if (QuestManager.GetQuest<AuroraStagQuest>().IsActive)
				return 0.05f;
			return 0.0015f;
		}

		private Color AuroraColor => Color.Lerp(new Color(85, 255, 229), new Color(28, 155, 255), Main.rand.NextFloat());

		public override void AI()
		{
			NPC.TargetClosest(false);
			if (TameAnimationTimer > 0) {
				NPC.velocity = Vector2.Zero;
				NPC.noGravity = true;
				NPC.noTileCollide = true;
				NPC.immortal = true;
				NPC.dontTakeDamage = true;

				TameAnimationTimer--;

				if (!Main.dedServ) 
				{
					if (!EventManager.IsPlaying<FollowNPCThenReturn>() && !Main.dedServ)
						EventManager.PlayEvent(new FollowNPCThenReturn(NPC, 1.5f, 1 + (int)(TameAnimationLength) / 60, 1.5f));

					void SpawnParticle()
					{
						AuroraOrbParticle particle = new AuroraOrbParticle(
						NPC,
						NPC.Center + Main.rand.NextVector2Circular(30, 30),
						Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 5f),
						AuroraColor,
						Main.rand.NextFloat(0.05f, 0.1f));

						ParticleHandler.SpawnParticle(particle);
					}

					if (TameAnimationTimer == TameAnimationLength - 1) //initial burst
					{
						SoundEngine.PlaySound(new LegacySoundStyle(SoundID.Item, 8).WithPitchVariance(0.2f), NPC.Center);

						for (int i = 0; i <= 8; i++)
							SpawnParticle();
					}
					//continue to spawn particles afterwards
					float particlespawnendtime = MathHelper.Lerp(TameAnimationLength, ParticleAnticipationTime, 0.66f);
					if (TameAnimationTimer % 36 == 0 && TameAnimationTimer > particlespawnendtime)
						SoundEngine.PlaySound(new LegacySoundStyle(SoundID.Item, 8).WithPitchVariance(0.2f), NPC.Center);

					if (TameAnimationTimer % 12 == 0 && TameAnimationTimer > particlespawnendtime)
						SpawnParticle();
				}

				Lighting.AddLight(NPC.Center, Brightness, Brightness, Brightness);

				if (TameAnimationTimer == 0) {
					NPC.active = false;

					if (!Main.dedServ)
					{
						SoundEngine.PlaySound(SoundID.NPCKilled, NPC.Center, 8);
						SoundEngine.PlaySound(new LegacySoundStyle(SoundID.Item, 9).WithPitchVariance(0.2f), NPC.Center);
						SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse, NPC.Center);
						SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, NPC.Center);
						for (int i = 0; i < 25; i++) {
							MakeStar(Main.rand.NextFloat(0.2f, 0.6f));
						}

						EventManager.PlayEvent(new ScreenFlash(Color.White, 0.05f, 0.2f, 0.8f));
						EventManager.PlayEvent(new ScreenShake(30f, 0.33f));
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int i = Item.NewItem(NPC.Center, ModContent.ItemType<AuroraSaddle>());
						Main.item[i].noGrabDelay = 120;
						Main.item[i].velocity = new Vector2(0, -3);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
					}
				}
					
				return;
			}

			float alertRadius = 400;
			float scareRadius = 200;

			if (Main.netMode == NetmodeID.SinglePlayer) {
				float distanceToPlayerSquared = Vector2.DistanceSquared(Main.LocalPlayer.Center, NPC.Center);

				if (distanceToPlayerSquared < scareRadius * scareRadius && !Scared && Main.LocalPlayer.velocity.LengthSquared() > 25)
					Scared = true;
				else if (distanceToPlayerSquared <= alertRadius * alertRadius && !Scared)
					Alerted = true;
				else if (distanceToPlayerSquared > alertRadius * alertRadius && !Scared)
					Alerted = false;
			}
			else
				foreach (Player player in Main.player) {
					if (!player.active)
						continue;

					float distanceToPlayerSquared = Vector2.DistanceSquared(player.Center, NPC.Center);

					if (distanceToPlayerSquared < scareRadius * scareRadius && !Scared && player.velocity.LengthSquared() > 25)
					{
						Scared = true;
						NPC.target = player.whoAmI;
					}
					else if (distanceToPlayerSquared < alertRadius * alertRadius && !Scared)
					{
						Alerted = true;
						NPC.target = player.whoAmI;
					}
					else if (distanceToPlayerSquared > alertRadius * alertRadius && !Scared)
						Alerted = false;

				}

			if (Scared)
			{
				NPC.direction = NPC.spriteDirection = Main.player[NPC.target].Center.X < NPC.Center.X ? 1 : -1;
				NPC.velocity.X = RunSpeed * (Main.player[NPC.target].Center.X < NPC.Center.X ? 1 : -1);
				NPC.velocity.Y = 0;
				NPC.noGravity = true;
				NPC.noTileCollide = true;
				NPC.immortal = true;
				NPC.dontTakeDamage = true;

				NPC.alpha += 5;
				if(NPC.alpha >= 255)
					NPC.active = false;

				if(Main.rand.NextBool() && !Main.dedServ)
				{
					MakeStar(Main.rand.NextFloat(0.1f, 0.2f));
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Rainbow, NPC.velocity.X/2, 0, 100, AuroraColor, Main.rand.NextFloat(0.9f, 1.3f));
					dust.fadeIn = 0.4f;
					dust.noGravity = true;
				}
			}
			else
			{
				if (NPC.collideX)
				{
					NPC.direction *= -1;
					NPC.spriteDirection = NPC.direction;
					NPC.netUpdate = true;
				}

				if (Alerted)
				{
					NPC.TargetClosest(true);
					NPC.direction = NPC.spriteDirection = Main.player[NPC.target].Center.X > NPC.Center.X ? 1 : -1;
					NPC.velocity.X = 0;
					Walking = false;
				}
				else
				{
					if (--TimeBeforeNextAction <= 0)
					{
						if (NPC.velocity.X == 0)
						{
							NPC.direction = Main.rand.NextBool() ? -1 : 1;
							Walking = true;
						}
						else
						{
							NPC.velocity.X = 0;
							Walking = false;
						}

						TimeBeforeNextAction = Main.rand.Next(2 * 60, 6 * 60);
						NPC.netUpdate = true;
					}
				}

				if (Walking && !Scared && !Alerted)
					NPC.velocity.X = WalkSpeed * NPC.direction;

				Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
			}

			Lighting.AddLight(NPC.Center, new Vector3(0.3f, 0.75f, 1f) * NPC.Opacity);
		}

		private void MakeStar(float scale)
		{
			Color color = AuroraColor;
			color.A = (byte)Main.rand.Next(100, 150);
			StarParticle particle = new StarParticle(
				NPC.Center + Main.rand.NextVector2Circular(30, 30),
				Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 3f),
				color,
				scale,
				Main.rand.Next(60, 120));

			ParticleHandler.SpawnParticle(particle);
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) => Scared = true;
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) => Scared = true;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D stagTexture = TextureAssets.Npc[NPC.type].Value;
			int frameHeight = stagTexture.Height / Main.npcFrameCount[NPC.type];
			int frameWidth = stagTexture.Width / 3;
			int frameX = 0;
			int frameY = 0;
			float drawYOffset = -18;

			if (Scared) {
				frameX = frameWidth * 2 - 20;
				frameY = (frameHeight + 6) * (int)(Main.GameUpdateCount / 6 % 6) - 2;
				frameHeight += 6;
				frameWidth += 32;

				if (frameY > 6 * frameHeight)
					frameY = 0;

				drawYOffset = -24;
			}
			else if (Walking) {
				frameX = frameWidth - 12;
				frameY = (frameHeight + 1) * (int)((Main.GameUpdateCount / 8) % 10);
				drawYOffset = -20;
			}
			drawYOffset += NPC.gfxOffY;

			Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth - 10, frameHeight);
			Vector2 drawPosition = NPC.position - Main.screenPosition + Vector2.UnitY * drawYOffset;
			SpriteEffects effects = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Point npcPoint = NPC.Center.ToTileCoordinates();

			spriteBatch.Draw(stagTexture, drawPosition, sourceRectangle, Lighting.GetColor(npcPoint.X, npcPoint.Y) * NPC.Opacity, 0f, Vector2.Zero, 1f, effects, 0f);

			for (int i = 0; i < 6; i++)
			{
				float glowtimer = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f);
				Color glowcolor = Color.White * glowtimer;
				Vector2 pulsedrawpos = drawPosition + new Vector2(5, 0).RotatedBy(i * MathHelper.TwoPi / 6) * (1.25f - glowtimer);
				spriteBatch.Draw(Mod.GetTexture("NPCs/AuroraStag/AuroraStagGlowmask"), pulsedrawpos, sourceRectangle, glowcolor * 0.5f * NPC.Opacity, 0f, Vector2.Zero, 1f, effects, 0f);
			}
			spriteBatch.Draw(Mod.GetTexture("NPCs/AuroraStag/AuroraStagGlowmask"), drawPosition, sourceRectangle, Color.White * NPC.Opacity, 0f, Vector2.Zero, 1f, effects, 0f);

			if (TameAnimationTimer > 0)
				spriteBatch.Draw(Mod.GetTexture("NPCs/AuroraStag/AuroraStagOverlay"), drawPosition, sourceRectangle, new Color(184, 244, 255) * Brightness, 0f, Vector2.Zero, 1f, effects, 0f);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!Alerted || Scared || TameAnimationTimer > 0)
				return;

			Texture2D exclamationTexture = Mod.GetTexture("NPCs/AuroraStag/AuroraStagExclamation");
			Vector2 exclamationPos = NPC.Top + Vector2.UnitX * NPC.width / 2 * NPC.spriteDirection;
			float sin = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4) * 2;
			float xOffset = (NPC.spriteDirection == -1 ? 16 : 0) + sin + 10;

			if (NPC.spriteDirection == 1)
				exclamationPos.X += xOffset;
			else
				exclamationPos.X -= xOffset;

			Player player = Main.player[NPC.target];
			float opacity = MathHelper.Min(2 - (NPC.Distance(player.Center) / 200), 1);
			float scale = MathHelper.Lerp(0.5f, 1.25f, player.velocity.Length() / 5);

			exclamationPos.Y -= (exclamationTexture.Height - 5) + sin;
			SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(exclamationTexture, exclamationPos - Main.screenPosition, null, Color.White * opacity, 0f, exclamationTexture.Size()/2, scale, effects, 0f);
		}
	}
}
