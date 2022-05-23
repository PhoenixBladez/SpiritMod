using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistSoul : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Soul");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.timeLeft = PASSIVETIME + LOCKONTIME + 2 * CIRCLETIME + ACCELLIFETIME + FADETIME;
			projectile.hostile = true;
			projectile.height = 24;
			projectile.width = 24;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.scale = Main.rand.NextFloat(0.7f, 0.8f);
			projectile.penetrate = -1;
			projectile.frame = Main.rand.Next(Main.projFrames[projectile.type]);
		}

		private ref float AiState => ref projectile.localAI[0];

		private const int STATE_PASSIVEMOVEMENT = 0;
		private const int STATE_CIRCLING = 1;
		private const int STATE_CIRCLINGCC = 2;
		private const int STATE_LOCKEDON = 3;
		private const int STATE_ACCELERATE = 4;
		private const int STATE_FADEOUT = 5;

		private const int PASSIVETIME = 10;
		private const int CIRCLETIME = 60;
		private const int LOCKONTIME = 30;
		private const int ACCELLIFETIME = 120;
		private const int FADETIME = 30;

		private ref float AiTimer => ref projectile.localAI[1];

		private Player Target => Main.player[(int)projectile.ai[1]];
		private int RotationDirection => (int)projectile.ai[0];

		private Vector2[] _posArray = new Vector2[10];

		public override void AI()
		{
			if (Target.dead || !Target.active)
			{
				projectile.Kill();
				return;
			}

			int FADEINTIME = PASSIVETIME + CIRCLETIME;
			float rotationmodifier = 0.5f + (float)Math.Sin(MathHelper.Pi * AiTimer / CIRCLETIME);
			switch (AiState)
			{
				case STATE_PASSIVEMOVEMENT: //briefly move in a straight line
					projectile.alpha = Math.Max(projectile.alpha - 255 / FADEINTIME, 0);
					projectile.velocity *= 0.97f;

					if (AiTimer > PASSIVETIME)
					{
						AiState = STATE_CIRCLING;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_CIRCLING: //then do 1 1/2 circle
					projectile.alpha = Math.Max(projectile.alpha - 255 / FADEINTIME, 0);
					projectile.velocity = projectile.velocity.RotatedBy(RotationDirection * rotationmodifier * (MathHelper.TwoPi + MathHelper.Pi) / CIRCLETIME) * 0.997f;
					if (AiTimer > CIRCLETIME)
					{
						AiState = STATE_CIRCLINGCC;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_CIRCLINGCC: //then do a full circle in the other direction
					projectile.alpha = Math.Max(projectile.alpha - 255 / FADEINTIME, 0);
					projectile.velocity = projectile.velocity.RotatedBy(RotationDirection * -MathHelper.TwoPi / CIRCLETIME) * 0.997f;
					if (AiTimer > CIRCLETIME)
					{
						AiState = STATE_LOCKEDON;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_LOCKEDON: //briefly aim towards the player slowly
					if (projectile.velocity.Length() > 3)
						projectile.velocity *= 0.97f;

					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.1f));
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * 3, 0.02f);

					float sinePeriod = LOCKONTIME;
					float sineStrength = MathHelper.PiOver4;
					projectile.position += (projectile.velocity.RotatedBy(sineStrength * Math.Sin(MathHelper.TwoPi * AiTimer / sinePeriod)) / 4) - (projectile.velocity/4);
					if (AiTimer > LOCKONTIME)
					{
						AiState = STATE_ACCELERATE;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_ACCELERATE: //then accelerate with weak homing
					if (projectile.velocity.Length() < 22)
						projectile.velocity *= 1.035f;

					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.02f));
					
					sinePeriod = ACCELLIFETIME / 4;
					sineStrength = MathHelper.Pi / 6;
					projectile.position += (projectile.velocity.RotatedBy(sineStrength * Math.Sin(MathHelper.TwoPi * AiTimer / sinePeriod)) / 2) - (projectile.velocity / 2);

					if (AiTimer > ACCELLIFETIME)
					{
						AiState = STATE_FADEOUT;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;

				case STATE_FADEOUT: //slow down and fade away
					projectile.velocity *= 0.95f;
					projectile.alpha += 255 / FADETIME;
					if (projectile.alpha >= 255)
						projectile.Kill();
					break;
			}
			++AiTimer;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Lighting.AddLight(projectile.Center, Color.Magenta.ToVector3() * projectile.Opacity * 1.5f);

			if (Main.rand.NextBool(4) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new GlowParticle(projectile.Center, projectile.velocity * Main.rand.NextFloat(0.2f), Color.Red * projectile.Opacity * 1.5f, Main.rand.NextFloat(0.04f, 0.06f) * projectile.scale, 40));

			if (AiTimer == 1 && AiState == STATE_PASSIVEMOVEMENT) //initialize on first tick
				for (int i = 0; i < _posArray.Length; i++)
					_posArray[i] = projectile.Center;

			//Old position array for drawing tail
			for (int i = _posArray.Length - 1; i > 0; i--)
				_posArray[i] = _posArray[i - 1];

			_posArray[0] = projectile.Center;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => base.Colliding(projHitbox, targetHitbox);

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

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Effect effect = SpiritMod.ShaderDict["PrimitiveTextureMap"];
			effect.Parameters["uTexture"].SetValue(mod.GetTexture("NPCs/Boss/Occultist/SoulTrail"));

			Vector2[] vertices = _posArray;
			var strip = new PrimitiveStrip
			{
				Color = Color.White * projectile.Opacity,
				Width = 13 * projectile.scale,
				PositionArray = vertices,
				TaperingType = StripTaperType.None,
			};
			PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

			Texture2D projTexture = Main.projectileTexture[projectile.type];
			var origin = new Vector2(projectile.DrawFrame().Width / 2, projectile.DrawFrame().Height);
			float headRotation = (_posArray[0] - _posArray[1]).ToRotation() + MathHelper.PiOver2;
			spriteBatch.Draw(projTexture, _posArray[0] - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(Color.White), headRotation, origin, projectile.scale, SpriteEffects.None, 0);
			
			return false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			Vector2[] vertices = _posArray;
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");

			for (int i = 1; i < vertices.Length; i++)
			{
				float progress = i / (float)vertices.Length;
				float scale = MathHelper.Lerp(0.25f, 0f, progress);
				float dist = Vector2.Distance(vertices[i], vertices[i - 1]);
				float rot = (vertices[i] - vertices[i - 1]).ToRotation();
				Vector2 scaleVec = scale * new Vector2(dist / 20, 3);
				sB.Draw(bloom, vertices[i] - Main.screenPosition, null, Color.Red * Math.Min(1.2f * projectile.Opacity, 0.6f), rot + MathHelper.PiOver2, bloom.Size() / 2, scaleVec, SpriteEffects.None, 0);
			}
			float headRotation = (_posArray[0] - _posArray[1]).ToRotation() + MathHelper.PiOver2;
			sB.Draw(bloom, projectile.Center - new Vector2(0, projectile.DrawFrame().Height / 4).RotatedBy(headRotation) - Main.screenPosition, null, Color.Red * Math.Min(1.2f * projectile.Opacity, 0.6f) * projectile.Opacity, 0, bloom.Size() / 2, 0.3f, SpriteEffects.None, 0);
		}
	}
}