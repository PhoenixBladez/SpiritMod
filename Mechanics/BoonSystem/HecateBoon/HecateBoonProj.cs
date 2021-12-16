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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}


		public override void SetDefaults()
		{
			projectile.width = projectile.height = 18;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.scale = 1.5f;
			projectile.hide = true;
			projectile.ignoreWater = true;
			projectile.direction = Main.rand.NextBool() ? -1 : 1;
		}

		private ref float AiState => ref projectile.localAI[0];
		private ref float AiTimer => ref projectile.localAI[1];

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
			Player player = Main.player[(int)projectile.ai[0]];
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
					projectile.rotation += ROTATION_SPEED_MAX * progress * projectile.direction;
					if(AiTimer >= SPIN_TIME)
					{
						AiTimer = 0;
						AiState = STATE_ANTICIPATION;
						projectile.netUpdate = true;
					}
					break;

				case STATE_ANTICIPATION:
					projectile.rotation += ROTATION_SPEED_MAX * projectile.direction;
					projectile.velocity = projectile.DirectionFrom(player.Center) * SPEED_MIN / 2;
					if(AiTimer >= ANTICIPATION_TIME)
					{
						AiTimer = 0;
						AiState = STATE_LAUNCH;
						projectile.velocity = projectile.DirectionTo(player.Center) * SPEED_MIN;
						projectile.netUpdate = true;
					}
					break;

				case STATE_LAUNCH:
					progress = AiTimer / LAUNCH_TIME;
					projectile.rotation += ROTATION_SPEED_MAX * projectile.direction;
					projectile.velocity = Vector2.Normalize(projectile.velocity) * MathHelper.Lerp(SPEED_MIN, SPEED_MAX, progress);
					if(AiTimer >= LAUNCH_TIME)
					{
						AiTimer = 0;
						AiState = STATE_FADE;
						projectile.netUpdate = true;
					}
					break;

				case STATE_FADE:
					progress = AiTimer / FADE_TIME;
					projectile.alpha = (int)(255 * progress);
					projectile.rotation += ROTATION_SPEED_MAX * projectile.direction * (1 - progress);
					projectile.velocity *= 0.98f;
					if(AiTimer >= FADE_TIME)
						projectile.Kill();
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

		public void AdditiveCall(SpriteBatch sB)
		{
			int trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			for (int i = 0; i < trailLength; i++)
			{
				float progress = i / (float)trailLength;
				float opacity = 1 - progress;
				opacity *= 0.66f;

				float scale = 0.25f;

				Vector2 drawPos = projectile.oldPos[i] + (projectile.Size / 2) - Main.screenPosition;
				sB.Draw(bloom, drawPos, null, Color.Purple * opacity * projectile.Opacity, 0, bloom.Size() / 2, scale, SpriteEffects.None, 0);
			}
			projectile.QuickDrawTrail(sB, drawColor: Color.White);
			projectile.QuickDraw(sB, drawColor: Color.White);
		}
	}
}
