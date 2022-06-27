using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Particles;
using System.Collections.Generic;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class StarWeaverHead : ModProjectile
	{
		private const int TrailLength = 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Weaver");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailLength;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(54, 54);
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 0.66f;
			Projectile.alpha = 255;
		}

		private NPC Parent => Main.npc[(int)Projectile.ai[0]];
		private StarWeaverNPC WeaverNPC => Parent.ModNPC as StarWeaverNPC;

		private ref float AiTimer => ref Projectile.localAI[0];
		private ref float AiState => ref Projectile.ai[1];

		private ref float TrailProgress => ref Projectile.localAI[1];

		private const float RETURNTHRESHOLD = 200;
		private const float HOVERTHRESHOLD = 40;
		private const int ANTICIPATIONTIME = 30;

		private const int STATE_HOVERTOPARENT = 0;
		private const int STATE_RETURNTOPARENT = 1;

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => TrailProgress == 1 || WeaverNPC.AiState == StarWeaverNPC.STATE_STARGLOOP && Projectile.scale == 0.66f;

		public override void AI()
		{
			if(!Parent.active || Parent.type != ModContent.NPCType<StarWeaverNPC>())
			{
				Projectile.Kill();
				return;
			}

			if(WeaverNPC.Head != Projectile)
			{
				Projectile.Kill();
				return;
			}
			Projectile.UpdateFrame(10);
			Projectile.timeLeft = 2;
			Vector2 TargetPos = Parent.Top - new Vector2(0, 20);
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			int fadeTime = 35;
			if (WeaverNPC.AiState == StarWeaverNPC.STATE_STARGLOOP) //Fade out head and do stargloop dust if parent is doing stargloop attack
			{
				for (int i = 0; i < 5; i++)
					Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), ModContent.DustType<Dusts.EnemyStargoopDustFastDissipate>(), 
						Vector2.UnitY * Main.rand.NextFloat(-4.5f, -0.5f) + (Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) / 5), Scale: 3);

				Projectile.scale = Math.Max(Projectile.scale - 0.33f / fadeTime, 0.66f);
				Projectile.alpha = Math.Min(Projectile.alpha + 255 / fadeTime, 255);

				//Shoot after fully fading out, calculated based on star weaver's stargloop attack length, and how many times it shoots
				int ShotTimer = (int)(WeaverNPC.AiTimer - fadeTime);
				int TimeBetweenShots = ((StarWeaverNPC.STARGLOOP_TIME - fadeTime) / StarWeaverNPC.STARGLOOP_NUMSHOTS);
				if (WeaverNPC.AiTimer > fadeTime && ShotTimer % TimeBetweenShots == 0)
				{
					Vector2 velocity = Projectile.DirectionTo(Main.player[Parent.target].Center)
						.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver4, MathHelper.PiOver2) * (Main.rand.NextBool() ? -1 : 1)) * Main.rand.NextFloat(4, 6);
					Projectile.velocity -= velocity * 1.33f;

					int p = Projectile.NewProjectile(Projectile.Center, velocity, ModContent.ProjectileType<WeaverStargloopChaser>(), Projectile.damage, 1f, Main.myPlayer, Parent.target);

					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
				}
			}
			else
			{
				if (Projectile.scale >= 1)
					Projectile.scale = MathHelper.Lerp(1, 1.2f, TrailProgress);
				else
					Projectile.scale = Math.Min(Projectile.scale + 0.33f / fadeTime, 1);

				Projectile.alpha = Math.Max(Projectile.alpha - 255 / fadeTime, 0);
			}

			//projectile.alpha = Math.Max(projectile.alpha - 255 / fadeTime, 0);

			switch (AiState)
			{
				case STATE_HOVERTOPARENT:
					AiTimer = 0;
					Projectile.extraUpdates = 0;
					TrailProgress = Math.Max(TrailProgress - 0.05f, 0);
					Projectile.spriteDirection = Parent.spriteDirection;
					if (Vector2.Distance(Projectile.Center, TargetPos) > RETURNTHRESHOLD)
					{
						AiState = STATE_RETURNTOPARENT;
						Projectile.netUpdate = true;
						break;
					}
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Lerp(Projectile.Center, TargetPos, 0.2f) - Projectile.Center, 0.12f);

					break;

				case STATE_RETURNTOPARENT:
					AiTimer++;
					Projectile.extraUpdates = 1;
					TrailProgress = Math.Min(TrailProgress + 0.05f, 1);

					if(AiTimer == 1)
					{
						Projectile.NewProjectileDirect(Projectile.Center, (TargetPos - Projectile.position) / 20, ModContent.ProjectileType<WeaverHeadTelegraph>(), 0, 0, Main.myPlayer);
					}

					if (AiTimer < ANTICIPATIONTIME)
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionFrom(TargetPos) * 6, 0.06f);

					else if (AiTimer == ANTICIPATIONTIME)
					{
						Projectile.netUpdate = true;
						Projectile.velocity = Projectile.DirectionTo(TargetPos) * 10;
					}

					else
					{
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(TargetPos) * 20, 0.06f);
						if (Vector2.Distance(Projectile.Center, TargetPos) < HOVERTHRESHOLD)
						{
							AiState = STATE_HOVERTOPARENT;
							Projectile.netUpdate = true;
							break;
						}

						if (Main.rand.NextBool(3) && !Main.dedServ)
							ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.Pink, Color.Red, Main.rand.NextFloat(0.1f, 0.2f) * Projectile.scale, 25));
					}

					break;
			}
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) => drawCacheProjsBehindNPCs.Add(index);

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AiTimer);
			writer.Write(TrailProgress);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AiTimer = reader.ReadSingle();
			TrailProgress = reader.ReadSingle();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (WeaverNPC == null)
				return false;

			Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Ripple").Value;

			//Draw trail when returning to npc
			for(int i = 0; i < TrailLength; i++)
			{
				Vector2 position = Projectile.oldPos[i] + Projectile.Size / 2;
				float rotation = Projectile.oldRot[i];
				float scale = (TrailLength - i) / (float)TrailLength;
				float opacity = TrailProgress * scale;

				spriteBatch.Draw(ripple, position - Main.screenPosition, null, Projectile.GetAlpha(Color.Red) * TrailProgress, 0, ripple.Size() / 2, 1.5f * scale, SpriteEffects.None, 0);

				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, position - Main.screenPosition, Projectile.DrawFrame(), Projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f)) * opacity, 
					rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale * scale, SpriteEffects.None, 0);

				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_glowRed"), position - Main.screenPosition, Projectile.DrawFrame(), Projectile.GetAlpha(Color.White) * opacity,
					rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale * scale, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(ripple, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.Red) * TrailProgress, 0, ripple.Size() / 2, 1.5f, SpriteEffects.None, 0);

			Color bloomColor = Color.White;
			bloomColor.A = 0;
			float AttackProgress = StarWeaverNPC.GetBloomIntensity(WeaverNPC);

			//Glowy bloom during parent's starburst attacks
			spriteBatch.Draw(ripple, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.Gold) * (1 - TrailProgress) * AttackProgress, 0, 
				ripple.Size() / 2, 1.5f, SpriteEffects.None, 0);

			SpriteEffects flip = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			//Bloom glowmask
			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 12, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_glow"), Projectile.Center - Main.screenPosition + posOffset, Projectile.DrawFrame(),
					Projectile.GetAlpha(bloomColor) * (1 - TrailProgress) * AttackProgress * opacityMod,
					Projectile.rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale, flip, 0);
			});

			Projectile.QuickDraw(spriteBatch, drawColor: Projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f)));

			Color glowmaskColor = Color.Lerp(Color.White, bloomColor, AttackProgress);
			Projectile.QuickDrawGlow(spriteBatch, Projectile.GetAlpha(glowmaskColor) * (1 - TrailProgress));
			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_glowRed"), Projectile.Center - Main.screenPosition, Projectile.DrawFrame(), Projectile.GetAlpha(Color.White) * TrailProgress,
				Projectile.rotation, Projectile.DrawFrame().Size() / 2, Projectile.scale, flip, 0);
			return false;
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (Projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_mask"), Projectile.Center + offset - Main.screenPosition - Projectile.velocity * (float)i * trailLengthModifier, 
					Projectile.DrawFrame(), color, Projectile.rotation, Projectile.DrawFrame().Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
	}
}