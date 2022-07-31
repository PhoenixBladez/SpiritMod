using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.BoonSystem.HecateBoon
{
	public class HecateBoonProj : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Hecate");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}


		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 18;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.scale = 1.5f;
			Projectile.hide = true;
			Projectile.ignoreWater = true;
			Projectile.direction = Main.rand.NextBool() ? -1 : 1;
		}

		private ref float AiState => ref Projectile.localAI[0];
		private ref float AiTimer => ref Projectile.localAI[1];

		private const int STATE_SPIN = 0; //Spins in place
		private const int STATE_ANTICIPATION = 1; //Briefly moves away from target
		private const int STATE_LAUNCH = 2; //Launches to target, accelerating over time
		private const int STATE_FADE = 3; //Fades out and slows down


		private const int SPIN_TIME = 30;
		private const int ANTICIPATION_TIME = 12;
		private const int LAUNCH_TIME = 70;
		private const int FADE_TIME = 20;

		private const float SPEED_MIN = 10;
		private const float SPEED_MAX = 20;
		private const float ROTATION_SPEED_MAX = 0.35f;

		public override void AI()
		{
			Player player = Main.player[(int)Projectile.ai[0]];
			//Die if target is dead
			if((player.dead || !player.active || player == null) && AiState != STATE_FADE)
			{
				AiState = STATE_FADE;
				AiTimer = 0;
			}
			switch(AiState)
			{
				case STATE_SPIN:
					float progress = AiTimer / SPIN_TIME;
					Projectile.rotation += ROTATION_SPEED_MAX * progress * Projectile.direction;
					if(AiTimer >= SPIN_TIME)
					{
						AiTimer = 0;
						AiState = STATE_ANTICIPATION;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_ANTICIPATION:
					Projectile.rotation += ROTATION_SPEED_MAX * Projectile.direction;
					Projectile.velocity = Projectile.DirectionFrom(player.Center) * SPEED_MIN / 2;
					if(AiTimer >= ANTICIPATION_TIME)
					{
						AiTimer = 0;
						AiState = STATE_LAUNCH;
						Projectile.velocity = Projectile.DirectionTo(player.Center) * SPEED_MIN;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_LAUNCH:
					progress = AiTimer / LAUNCH_TIME;
					Projectile.rotation += ROTATION_SPEED_MAX * Projectile.direction;
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Lerp(SPEED_MIN, SPEED_MAX, progress);
					if(AiTimer >= LAUNCH_TIME)
					{
						AiTimer = 0;
						AiState = STATE_FADE;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_FADE:
					progress = AiTimer / FADE_TIME;
					Projectile.alpha = (int)(255 * progress);
					Projectile.rotation += ROTATION_SPEED_MAX * Projectile.direction * (1 - progress);
					Projectile.velocity *= 0.98f;
					if(AiTimer >= FADE_TIME)
						Projectile.Kill();
					break;
			}
			AiTimer++;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AiState);
			writer.Write(AiTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AiState = reader.ReadSingle();
			AiTimer = reader.ReadSingle();
		}

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos)
		{
			int trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			for (int i = 0; i < trailLength; i++)
			{
				float progress = i / (float)trailLength;
				float opacity = 1 - progress;
				opacity *= 0.66f;

				float scale = 0.25f;

				Vector2 drawPos = Projectile.oldPos[i] + (Projectile.Size / 2) - Main.screenPosition;
				sB.Draw(bloom, drawPos, null, Color.Purple * opacity * Projectile.Opacity, 0, bloom.Size() / 2, scale, SpriteEffects.None, 0);
			}
			Projectile.QuickDrawTrail(sB, drawColor: Color.White);
			Projectile.QuickDraw(sB, drawColor: Color.White);
		}
	}
}
