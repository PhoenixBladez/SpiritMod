using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Items;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.AuroraStag
{
	public class AuroraStag : ModNPC
	{
		private struct GlowOrb
		{
			public Vector2 Position;
			public Vector2 Velocity;
			public float Opaqueness;
			public float Scale;
			public float TimeAlive;

			public GlowOrb(Vector2 position, Vector2 velocity, float opaqueness, float scale)
			{
				Position = position;
				Velocity = velocity;
				Opaqueness = opaqueness;
				Scale = scale;
				TimeAlive = 0;
			}
		}

		private List<GlowOrb> glowies = new List<GlowOrb>();

		private float WalkSpeed => 2f;

		private float RunSpeed => 8f;

		public static float TameAnimationLength => 300;

		private float Brightness => (TameAnimationLength - TameAnimationTimer) / TameAnimationLength;

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
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.player.ZoneSnow && MyWorld.aurora && Main.hardMode)
				return 0.0015f;

			return 0f;
		}

		public override void AI()
		{
			if (TameAnimationTimer > 0) {
				npc.velocity = Vector2.Zero;
				npc.noGravity = true;
				npc.noTileCollide = true;
				npc.immortal = true;
				npc.dontTakeDamage = true;

				TameAnimationTimer--;

				if (!Main.dedServ) {
					if (Main.rand.NextBool(15))
						glowies.Add(new GlowOrb(npc.Center + new Vector2(1), Main.rand.NextVector2Unit() * 8, 1f, Main.rand.NextFloat(0.7f, 2f)));

					for (int i = 0; i < glowies.Count; i++) {
						Vector2 velocity = npc.DirectionFrom(glowies[i].Position);

						if (TameAnimationTimer > 10) {
							velocity += glowies[i].Velocity;
							velocity.Normalize();
							velocity *= 8;
						}
						else
							velocity *= 30;

						GlowOrb updatedOrb = new GlowOrb(glowies[i].Position + glowies[i].Velocity, velocity, glowies[i].Opaqueness - 0.005f, glowies[i].Scale);
						updatedOrb.TimeAlive++;

						glowies[i] = updatedOrb;

						float brightness = updatedOrb.Opaqueness * 0.2f;
						Lighting.AddLight(updatedOrb.Position, brightness, brightness, brightness);
					}
				}

				Lighting.AddLight(npc.Center, Brightness, Brightness, Brightness);

				if (TameAnimationTimer == 0) {
					npc.active = false;

					if (!Main.dedServ)
						for (int i = 0; i < 25; i++)
							Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<AuroraStagDust>(), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));

					if (Main.netMode != NetmodeID.MultiplayerClient)
						Item.NewItem(npc.Center, ModContent.ItemType<AuroraSaddle>());
				}
					
				return;
			}

			float alertRadius = 230;
			float scareRadius = 180;
			float returnToCalmRadius = 2000;

			if (Main.netMode == NetmodeID.SinglePlayer) {
				float distanceToPlayerSquared = Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center);

				if (distanceToPlayerSquared > returnToCalmRadius * returnToCalmRadius)
					Scared = false;
				else if (distanceToPlayerSquared < scareRadius * scareRadius && !Scared && Main.LocalPlayer.velocity.LengthSquared() > 1)
					Scared = true;
				else if (distanceToPlayerSquared < alertRadius * alertRadius && !Scared)
					Alerted = true;
			}
			else
				foreach (Player player in Main.player) {
					if (!player.active)
						continue;

					float distanceToPlayerSquared = Vector2.DistanceSquared(player.Center, npc.Center);

					if (distanceToPlayerSquared > returnToCalmRadius * returnToCalmRadius)
						Scared = false;
					else if (distanceToPlayerSquared < scareRadius * scareRadius && !Scared && player.velocity.LengthSquared() > 1) {
						Scared = true;
						npc.target = player.whoAmI;
					}
					else if (distanceToPlayerSquared < alertRadius * alertRadius && !Scared) {
						Alerted = true;
						npc.target = player.whoAmI;
					}
				}

			bool justStartedWalking = false;

			if (Scared) {
				npc.velocity.X = RunSpeed * (Main.player[npc.target].Center.X < npc.Center.X ? 1 : -1);
				Walking = false;
			}
			else if (Alerted) {
				npc.direction = npc.spriteDirection = Main.player[npc.target].Center.X > npc.Center.X ? 1 : -1;
				npc.velocity.X = 0;
				Walking = false;
			}
			else {
				if (--TimeBeforeNextAction <= 0) {
					if (npc.velocity.X == 0) {
						npc.velocity.X = WalkSpeed * (Main.rand.NextBool() ? 1 : -1);
						Walking = true;
						justStartedWalking = true;
					}
					else {
						npc.velocity.X = 0;
						Walking = false;
					}

					TimeBeforeNextAction = Main.rand.Next(2 * 60, 6 * 60);
					npc.netUpdate = true;
				}
			}

			if (Walking && !Scared && !Alerted)
				npc.velocity.X = WalkSpeed * npc.direction;

			if (npc.velocity.X != 0)
				npc.direction = npc.spriteDirection = npc.velocity.X > 0 ? 1 : -1;

			if (npc.velocity.X != 0 && npc.oldPosition == npc.position && !justStartedWalking) {
				npc.velocity.Y = -10;
				npc.netUpdate = true;
			}

			Lighting.AddLight(npc.Center, 0.3f, 0.75f, 1f);
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			npc.TargetClosest();
			Scared = true;
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			npc.TargetClosest();
			Scared = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D orbTexture = mod.GetTexture("NPCs/AuroraStag/GlowOrb");
			Vector2 orbOrigin = new Vector2(orbTexture.Width / 2, orbTexture.Height / 2);

			foreach (GlowOrb glowOrb in glowies)
				spriteBatch.Draw(orbTexture, glowOrb.Position - Main.screenPosition, null, Color.White * glowOrb.Opaqueness, 0f, orbOrigin, (float)Math.Sin(glowOrb.TimeAlive / 4) * glowOrb.Scale, SpriteEffects.None, 0f);

			if (TameAnimationTimer > 0)
				spriteBatch.Draw(orbTexture, npc.Center - Main.screenPosition, null, Color.White, 0f, orbOrigin, (float)Math.Sin(TameAnimationTimer / 30) * ((TameAnimationTimer - TameAnimationLength) / TameAnimationLength) * 2, SpriteEffects.None, 0f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D stagTexture = Main.npcTexture[npc.type];
			int frameHeight = stagTexture.Height / Main.npcFrameCount[npc.type];
			int frameWidth = stagTexture.Width / 3;
			int frameX = 0;
			int frameY = 0;
			int drawYOffset = -18;

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

			Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth - 10, frameHeight);
			Vector2 drawPosition = npc.position - Main.screenPosition + Vector2.UnitY * drawYOffset;
			SpriteEffects effects = npc.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Point npcPoint = npc.Center.ToTileCoordinates();

			spriteBatch.Draw(stagTexture, drawPosition, sourceRectangle, Lighting.GetColor(npcPoint.X, npcPoint.Y), 0f, Vector2.Zero, 1f, effects, 0f);

			Color glowColor = Color.White * (float)Math.Abs(Math.Sin(Main.GlobalTime));
			spriteBatch.Draw(mod.GetTexture("NPCs/AuroraStag/AuroraStagGlowmask"), drawPosition, sourceRectangle, glowColor, 0f, Vector2.Zero, 1f, effects, 0f);

			if (TameAnimationTimer > 0)
				spriteBatch.Draw(mod.GetTexture("NPCs/AuroraStag/AuroraStagOverlay"), drawPosition, sourceRectangle, Color.White * Brightness, 0f, Vector2.Zero, 1f, effects, 0f);

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

			exclamationPos.Y -= (exclamationTexture.Height - 5) + sin;
			SpriteEffects effects = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(exclamationTexture, exclamationPos - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, effects, 0f);
		}
	}
}
