using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Items;
using SpiritMod.Items.Equipment.AuroraSaddle;
using SpiritMod.Particles;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.AuroraStag
{
	public class AuroraStag : ModNPC, IDrawAdditive
	{
		private float WalkSpeed => 2f;

		private float RunSpeed => 8f;

		public static float TameAnimationLength => 300;

		public static float ParticleAnticipationTime => TameAnimationLength * 0.175f;

		public static float ParticleReturnTime => TameAnimationLength * 0.1f;

		private float Brightness => (float)Math.Pow((TameAnimationLength - TameAnimationTimer) / TameAnimationLength, 3);

		// the time left before the stag starts moving again if it is standing still, or the time left until it stops if it is moving
		// ignored if the stag is alerted or scared
		private ref float TimeBeforeNextAction => ref npc.ai[2];

		private bool Walking {
			get => npc.ai[3] == 1;
			set {
				if (value)
					npc.ai[3] = 1;
				else
					npc.ai[3] = 0;
			}
		}

		public bool Scared
		{
			get => npc.ai[0] == 1;
			private set
			{
				if (value)
					npc.ai[0] = 1;
				else
					npc.ai[0] = 0;
			}
		}

		public bool Alerted
		{
			get => npc.ai[0] == 2;
			private set
			{
				if (value)
					npc.ai[0] = 2;
				else
					npc.ai[0] = 0;
			}
		}

		public float TameAnimationTimer
		{
			get => npc.ai[1];
			set => npc.ai[1] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Stag");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 100;
			npc.defense = 12;
			npc.lifeMax = 800;
			npc.HitSound = SoundID.NPCHit5;
			npc.DeathSound = SoundID.NPCDeath7;
			npc.value = 800;
			npc.knockBackResist = .85f;
			npc.aiStyle = -1;

			for (int i = 0; i < BuffLoader.BuffCount; i++)
				npc.buffImmune[i] = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.player.ZoneSnow && MyWorld.aurora && Main.hardMode)
				return 0.0015f;

			return 0f;
		}

		private Color AuroraColor => Color.Lerp(new Color(85, 255, 229), new Color(28, 155, 255), Main.rand.NextFloat());

		public override void AI()
		{
			npc.TargetClosest(false);
			if (TameAnimationTimer > 0) {
				npc.velocity = Vector2.Zero;
				npc.noGravity = true;
				npc.noTileCollide = true;
				npc.immortal = true;
				npc.dontTakeDamage = true;

				TameAnimationTimer--;

				if (!Main.dedServ) {
					if ((TameAnimationTimer % 12 == 0) && TameAnimationTimer > ParticleReturnTime)
					{
						if (TameAnimationTimer % 36 == 0)
						{
							Main.PlaySound(new LegacySoundStyle(SoundID.Item, 8).WithPitchVariance(0.2f), npc.Center);
													}
							AuroraOrbParticle particle = new AuroraOrbParticle(
							npc,
							npc.Center + Main.rand.NextVector2Circular(30, 30),
							Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 5f),
							AuroraColor,
							Main.rand.NextFloat(0.05f, 0.1f));

						ParticleHandler.SpawnParticle(particle);
					}

					if(Main.rand.NextBool(30))
						MakeStar(Main.rand.NextFloat(0.1f, 0.2f));
				}

				Lighting.AddLight(npc.Center, Brightness, Brightness, Brightness);

				if (TameAnimationTimer == 0) {
					npc.active = false;

					if (!Main.dedServ)
					{
						Main.PlaySound(4, npc.Center, 8);
						Main.PlaySound(new LegacySoundStyle(SoundID.Item, 9).WithPitchVariance(0.2f), npc.Center);
						Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, npc.Center);
						Main.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
						for (int i = 0; i < 25; i++) {
							MakeStar(Main.rand.NextFloat(0.2f, 0.6f));
						}
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Item.NewItem(npc.Center, ModContent.ItemType<AuroraSaddle>());
				}
					
				return;
			}

			float alertRadius = 400;
			float scareRadius = 200;

			if (Main.netMode == NetmodeID.SinglePlayer) {
				float distanceToPlayerSquared = Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center);

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

					float distanceToPlayerSquared = Vector2.DistanceSquared(player.Center, npc.Center);

					if (distanceToPlayerSquared < scareRadius * scareRadius && !Scared && player.velocity.LengthSquared() > 25)
					{
						Scared = true;
						npc.target = player.whoAmI;
					}
					else if (distanceToPlayerSquared < alertRadius * alertRadius && !Scared)
					{
						Alerted = true;
						npc.target = player.whoAmI;
					}
					else if (distanceToPlayerSquared > alertRadius * alertRadius && !Scared)
						Alerted = false;

				}

			if (Scared)
			{
				npc.direction = npc.spriteDirection = Main.player[npc.target].Center.X < npc.Center.X ? 1 : -1;
				npc.velocity.X = RunSpeed * (Main.player[npc.target].Center.X < npc.Center.X ? 1 : -1);
				npc.velocity.Y = 0;
				npc.noGravity = true;
				npc.noTileCollide = true;
				npc.immortal = true;
				npc.dontTakeDamage = true;

				npc.alpha += 5;
				if(npc.alpha >= 255)
					npc.active = false;

				if(Main.rand.NextBool() && !Main.dedServ)
				{
					MakeStar(Main.rand.NextFloat(0.1f, 0.2f));
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 66, npc.velocity.X/2, 0, 100, AuroraColor, Main.rand.NextFloat(0.9f, 1.3f));
					dust.fadeIn = 0.4f;
					dust.noGravity = true;
				}
			}
			else
			{
				if (npc.collideX)
				{
					npc.direction *= -1;
					npc.spriteDirection = npc.direction;
					npc.netUpdate = true;
				}

				if (Alerted)
				{
					npc.TargetClosest(true);
					npc.direction = npc.spriteDirection = Main.player[npc.target].Center.X > npc.Center.X ? 1 : -1;
					npc.velocity.X = 0;
					Walking = false;
				}
				else
				{
					if (--TimeBeforeNextAction <= 0)
					{
						if (npc.velocity.X == 0)
						{
							npc.direction = Main.rand.NextBool() ? -1 : 1;
							Walking = true;
						}
						else
						{
							npc.velocity.X = 0;
							Walking = false;
						}

						TimeBeforeNextAction = Main.rand.Next(2 * 60, 6 * 60);
						npc.netUpdate = true;
					}
				}

				if (Walking && !Scared && !Alerted)
					npc.velocity.X = WalkSpeed * npc.direction;

				Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);
			}

			Lighting.AddLight(npc.Center, new Vector3(0.3f, 0.75f, 1f) * npc.Opacity);
		}

		private void MakeStar(float scale)
		{
			Color color = AuroraColor;
			color.A = (byte)Main.rand.Next(100, 150);
			AuroraStarParticle particle = new AuroraStarParticle(
				npc.Center + Main.rand.NextVector2Circular(30, 30),
				Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 3f),
				color,
				Main.rand.NextFloat(MathHelper.TwoPi),
				scale,
				Main.rand.Next(60, 120));

			ParticleHandler.SpawnParticle(particle);
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) => Scared = true;

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) => Scared = true;

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D orbTexture = mod.GetTexture("NPCs/AuroraStag/GlowOrb");
			Vector2 orbOrigin = new Vector2(orbTexture.Width / 2, orbTexture.Height / 2);
			float opacity = 0.5f * (TameAnimationLength - TameAnimationTimer) / TameAnimationLength;
			float scale = ((float)(Math.Sin(TameAnimationTimer / 30) / 4) + 1f) * ((TameAnimationTimer - TameAnimationLength) / TameAnimationLength);
			if (TameAnimationTimer > 0)
				spriteBatch.Draw(orbTexture, npc.Center - Main.screenPosition, null, new Color(184, 244, 255) * opacity, 0f, orbOrigin, scale, SpriteEffects.None, 0f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D stagTexture = Main.npcTexture[npc.type];
			int frameHeight = stagTexture.Height / Main.npcFrameCount[npc.type];
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
			drawYOffset += npc.gfxOffY;

			Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth - 10, frameHeight);
			Vector2 drawPosition = npc.position - Main.screenPosition + Vector2.UnitY * drawYOffset;
			SpriteEffects effects = npc.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Point npcPoint = npc.Center.ToTileCoordinates();

			spriteBatch.Draw(stagTexture, drawPosition, sourceRectangle, Lighting.GetColor(npcPoint.X, npcPoint.Y) * npc.Opacity, 0f, Vector2.Zero, 1f, effects, 0f);

			for (int i = 0; i < 6; i++)
			{
				float glowtimer = (float)(Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f);
				Color glowcolor = Color.White * glowtimer;
				Vector2 pulsedrawpos = drawPosition + new Vector2(5, 0).RotatedBy(i * MathHelper.TwoPi / 6) * (1.25f - glowtimer);
				spriteBatch.Draw(mod.GetTexture("NPCs/AuroraStag/AuroraStagGlowmask"), pulsedrawpos, sourceRectangle, glowcolor * 0.5f * npc.Opacity, 0f, Vector2.Zero, 1f, effects, 0f);
			}
			spriteBatch.Draw(mod.GetTexture("NPCs/AuroraStag/AuroraStagGlowmask"), drawPosition, sourceRectangle, Color.White * npc.Opacity, 0f, Vector2.Zero, 1f, effects, 0f);

			if (TameAnimationTimer > 0)
				spriteBatch.Draw(mod.GetTexture("NPCs/AuroraStag/AuroraStagOverlay"), drawPosition, sourceRectangle, new Color(184, 244, 255) * Brightness, 0f, Vector2.Zero, 1f, effects, 0f);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (!Alerted || Scared || TameAnimationTimer > 0)
				return;

			Texture2D exclamationTexture = mod.GetTexture("NPCs/AuroraStag/AuroraStagExclamation");
			Vector2 exclamationPos = npc.Top + Vector2.UnitX * npc.width / 2 * npc.spriteDirection;
			float sin = (float)Math.Sin(Main.GlobalTime * 4) * 2;
			float xOffset = (npc.spriteDirection == -1 ? 16 : 0) + sin + 10;

			if (npc.spriteDirection == 1)
				exclamationPos.X += xOffset;
			else
				exclamationPos.X -= xOffset;

			Player player = Main.player[npc.target];
			float opacity = MathHelper.Min(2 - (npc.Distance(player.Center) / 200), 1);
			float scale = MathHelper.Lerp(0.5f, 1.25f, player.velocity.Length() / 5);

			exclamationPos.Y -= (exclamationTexture.Height - 5) + sin;
			SpriteEffects effects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(exclamationTexture, exclamationPos - Main.screenPosition, null, Color.White * opacity, 0f, exclamationTexture.Size()/2, scale, effects, 0f);
		}
	}
}
