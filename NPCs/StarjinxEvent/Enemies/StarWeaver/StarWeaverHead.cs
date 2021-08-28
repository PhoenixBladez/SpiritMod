using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Particles;

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
		}

		private NPC Parent => Main.npc[(int)projectile.ai[0]];
		private StarWeaverNPC WeaverNPC => Parent.modNPC as StarWeaverNPC;

		private ref float AiTimer => ref projectile.localAI[0];
		private ref float AiState => ref projectile.ai[1];

		private ref float TrailProgress => ref projectile.localAI[1];

		private const float RETURNTHRESHOLD = 400;
		private const float HOVERTHRESHOLD = 40;
		private const int ANTICIPATIONTIME = 30;

		private const int STATE_HOVERTOPARENT = 0;
		private const int STATE_RETURNTOPARENT = 1;

		public override bool CanDamage() => TrailProgress == 1;

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
			for(int i = 0; i < TrailLength; i++)
			{
				Vector2 position = projectile.oldPos[i] + projectile.Size / 2;
				float rotation = projectile.oldRot[i];
				float scale = (TrailLength - i) / (float)TrailLength;
				float opacity = TrailProgress * scale;

				spriteBatch.Draw(ripple, position - Main.screenPosition, null, Color.Red * TrailProgress, 0, ripple.Size() / 2, 1.5f * scale, SpriteEffects.None, 0);

				spriteBatch.Draw(Main.projectileTexture[projectile.type], position - Main.screenPosition, projectile.DrawFrame(), Color.White * opacity, 
					rotation, projectile.DrawFrame().Size() / 2, projectile.scale * scale, SpriteEffects.None, 0);

				spriteBatch.Draw(ModContent.GetTexture(Texture + "_glow"), position - Main.screenPosition, projectile.DrawFrame(), Color.White * opacity,
					rotation, projectile.DrawFrame().Size() / 2, projectile.scale * scale, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(ripple, projectile.Center - Main.screenPosition, null, Color.Red * TrailProgress, 0, ripple.Size() / 2, 1.5f, SpriteEffects.None, 0);
			if(WeaverNPC.AiState == StarWeaverNPC.STATE_STARBURST)
			{
				spriteBatch.Draw(ripple, projectile.Center - Main.screenPosition, null, Color.Gold * (1 - TrailProgress), 0, ripple.Size() / 2, 1.5f, SpriteEffects.None, 0);

				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				float num366 = num395 + 1.1f;
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.5f, new Color(255, 234, 0), new Color(255, 234, 0) * .3f, 0.45f * (1 - TrailProgress), num366, .65f);
			}
			projectile.QuickDraw(spriteBatch, drawColor: Color.White);
			projectile.QuickDrawGlow(spriteBatch, Color.White * TrailProgress);
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