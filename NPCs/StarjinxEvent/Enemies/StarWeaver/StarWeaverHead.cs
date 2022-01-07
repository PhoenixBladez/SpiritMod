using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = TrailLength;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(54, 54);
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.scale = 0.66f;
			projectile.alpha = 255;
		}

		private NPC Parent => Main.npc[(int)projectile.ai[0]];
		private StarWeaverNPC WeaverNPC => Parent.modNPC as StarWeaverNPC;

		private ref float AiTimer => ref projectile.localAI[0];
		private ref float AiState => ref projectile.ai[1];

		private ref float TrailProgress => ref projectile.localAI[1];

		private const float RETURNTHRESHOLD = 200;
		private const float HOVERTHRESHOLD = 40;
		private const int ANTICIPATIONTIME = 30;

		private const int STATE_HOVERTOPARENT = 0;
		private const int STATE_RETURNTOPARENT = 1;

		public override bool CanDamage() => TrailProgress == 1 || WeaverNPC.AiState == StarWeaverNPC.STATE_STARGLOOP && projectile.scale == 0.66f;

		public override void AI()
		{
			if(!Parent.active || Parent.type != ModContent.NPCType<StarWeaverNPC>())
			{
				projectile.Kill();
				return;
			}

			if(WeaverNPC.Head != projectile)
			{
				projectile.Kill();
				return;
			}
			projectile.UpdateFrame(10);
			projectile.timeLeft = 2;
			Vector2 TargetPos = Parent.Top - new Vector2(0, 20);
			projectile.rotation = projectile.velocity.X * 0.05f;

			int fadeTime = 35;
			if (WeaverNPC.AiState == StarWeaverNPC.STATE_STARGLOOP) //Fade out head and do stargloop dust if parent is doing stargloop attack
			{
				for (int i = 0; i < 5; i++)
					Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Circular(15, 15), ModContent.DustType<Dusts.EnemyStargoopDustFastDissipate>(), 
						Vector2.UnitY * Main.rand.NextFloat(-4.5f, -0.5f) + (projectile.velocity.RotatedByRandom(MathHelper.PiOver4) / 5), Scale: 3);

				projectile.scale = Math.Max(projectile.scale - 0.33f / fadeTime, 0.66f);
				projectile.alpha = Math.Min(projectile.alpha + 255 / fadeTime, 255);

				//Shoot after fully fading out, calculated based on star weaver's stargloop attack length, and how many times it shoots
				int ShotTimer = (int)(WeaverNPC.AiTimer - fadeTime);
				int TimeBetweenShots = ((StarWeaverNPC.STARGLOOP_TIME - fadeTime) / StarWeaverNPC.STARGLOOP_NUMSHOTS);
				if (WeaverNPC.AiTimer > fadeTime && ShotTimer % TimeBetweenShots == 0)
				{
					Vector2 velocity = projectile.DirectionTo(Main.player[Parent.target].Center)
						.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver4, MathHelper.PiOver2) * (Main.rand.NextBool() ? -1 : 1)) * Main.rand.NextFloat(4, 6);
					projectile.velocity -= velocity * 1.33f;

					int p = Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<WeaverStargloopChaser>(), projectile.damage, 1f, Main.myPlayer, Parent.target);

					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
				}
			}
			else
			{
				if (projectile.scale >= 1)
					projectile.scale = MathHelper.Lerp(1, 1.2f, TrailProgress);
				else
					projectile.scale = Math.Min(projectile.scale + 0.33f / fadeTime, 1);

				projectile.alpha = Math.Max(projectile.alpha - 255 / fadeTime, 0);
			}

			//projectile.alpha = Math.Max(projectile.alpha - 255 / fadeTime, 0);

			switch (AiState)
			{
				case STATE_HOVERTOPARENT:
					AiTimer = 0;
					projectile.extraUpdates = 0;
					TrailProgress = Math.Max(TrailProgress - 0.05f, 0);
					projectile.spriteDirection = Parent.spriteDirection;
					if (Vector2.Distance(projectile.Center, TargetPos) > RETURNTHRESHOLD)
					{
						AiState = STATE_RETURNTOPARENT;
						projectile.netUpdate = true;
						break;
					}
					projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Lerp(projectile.Center, TargetPos, 0.2f) - projectile.Center, 0.12f);

					break;

				case STATE_RETURNTOPARENT:
					AiTimer++;
					projectile.extraUpdates = 1;
					TrailProgress = Math.Min(TrailProgress + 0.05f, 1);

					if(AiTimer == 1)
					{
						Projectile.NewProjectileDirect(projectile.Center, (TargetPos - projectile.position) / 20, ModContent.ProjectileType<WeaverHeadTelegraph>(), 0, 0, Main.myPlayer);
					}

					if (AiTimer < ANTICIPATIONTIME)
						projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionFrom(TargetPos) * 6, 0.06f);

					else if (AiTimer == ANTICIPATIONTIME)
					{
						projectile.netUpdate = true;
						projectile.velocity = projectile.DirectionTo(TargetPos) * 10;
					}

					else
					{
						projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(TargetPos) * 20, 0.06f);
						if (Vector2.Distance(projectile.Center, TargetPos) < HOVERTHRESHOLD)
						{
							AiState = STATE_HOVERTOPARENT;
							projectile.netUpdate = true;
							break;
						}

						if (Main.rand.NextBool(3) && !Main.dedServ)
							ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.Pink, Color.Red, Main.rand.NextFloat(0.1f, 0.2f) * projectile.scale, 25));
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

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (WeaverNPC == null)
				return false;

			Texture2D ripple = mod.GetTexture("Effects/Ripple");

			//Draw trail when returning to npc
			for(int i = 0; i < TrailLength; i++)
			{
				Vector2 position = projectile.oldPos[i] + projectile.Size / 2;
				float rotation = projectile.oldRot[i];
				float scale = (TrailLength - i) / (float)TrailLength;
				float opacity = TrailProgress * scale;

				spriteBatch.Draw(ripple, position - Main.screenPosition, null, projectile.GetAlpha(Color.Red) * TrailProgress, 0, ripple.Size() / 2, 1.5f * scale, SpriteEffects.None, 0);

				spriteBatch.Draw(Main.projectileTexture[projectile.type], position - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f)) * opacity, 
					rotation, projectile.DrawFrame().Size() / 2, projectile.scale * scale, SpriteEffects.None, 0);

				spriteBatch.Draw(ModContent.GetTexture(Texture + "_glowRed"), position - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(Color.White) * opacity,
					rotation, projectile.DrawFrame().Size() / 2, projectile.scale * scale, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(ripple, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.Red) * TrailProgress, 0, ripple.Size() / 2, 1.5f, SpriteEffects.None, 0);

			Color bloomColor = Color.White;
			bloomColor.A = 0;
			float AttackProgress = StarWeaverNPC.GetBloomIntensity(WeaverNPC);

			//Glowy bloom during parent's starburst attacks
			spriteBatch.Draw(ripple, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.Gold) * (1 - TrailProgress) * AttackProgress, 0, 
				ripple.Size() / 2, 1.5f, SpriteEffects.None, 0);

			SpriteEffects flip = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			//Bloom glowmask
			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 12, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				spriteBatch.Draw(ModContent.GetTexture(Texture + "_glow"), projectile.Center - Main.screenPosition + posOffset, projectile.DrawFrame(),
					projectile.GetAlpha(bloomColor) * (1 - TrailProgress) * AttackProgress * opacityMod,
					projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, flip, 0);
			});

			projectile.QuickDraw(spriteBatch, drawColor: projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f)));

			Color glowmaskColor = Color.Lerp(Color.White, bloomColor, AttackProgress);
			projectile.QuickDrawGlow(spriteBatch, projectile.GetAlpha(glowmaskColor) * (1 - TrailProgress));
			spriteBatch.Draw(ModContent.GetTexture(Texture + "_glowRed"), projectile.Center - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(Color.White) * TrailProgress,
				projectile.rotation, projectile.DrawFrame().Size() / 2, projectile.scale, flip, 0);
			return false;
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(ModContent.GetTexture(Texture + "_mask"), projectile.Center + offset - Main.screenPosition - projectile.velocity * (float)i * trailLengthModifier, 
					projectile.DrawFrame(), color, projectile.rotation, projectile.DrawFrame().Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
	}
}