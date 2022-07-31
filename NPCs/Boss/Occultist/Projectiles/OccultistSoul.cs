using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.VerletChains;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistSoul : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Soul");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = PASSIVETIME + LOCKONTIME + 2 * CIRCLETIME + ACCELLIFETIME + FADETIME;
			Projectile.hostile = true;
			Projectile.height = 24;
			Projectile.width = 24;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.scale = Main.rand.NextFloat(0.7f, 0.8f);
			Projectile.penetrate = -1;
			Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
		}

		private ref float AiState => ref Projectile.localAI[0];

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

		private ref float AiTimer => ref Projectile.localAI[1];

		private Player Target => Main.player[(int)Projectile.ai[1]];
		private int RotationDirection => (int)Projectile.ai[0];

		private Vector2[] _posArray = new Vector2[10];

		public override void AI()
		{
			if (Target.dead || !Target.active)
			{
				Projectile.Kill();
				return;
			}

			int FADEINTIME = PASSIVETIME + CIRCLETIME;
			float rotationmodifier = 0.5f + (float)Math.Sin(MathHelper.Pi * AiTimer / CIRCLETIME);
			switch (AiState)
			{
				case STATE_PASSIVEMOVEMENT: //briefly move in a straight line
					Projectile.alpha = Math.Max(Projectile.alpha - 255 / FADEINTIME, 0);
					Projectile.velocity *= 0.97f;

					if (AiTimer > PASSIVETIME)
					{
						AiState = STATE_CIRCLING;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_CIRCLING: //then do 1 1/2 circle
					Projectile.alpha = Math.Max(Projectile.alpha - 255 / FADEINTIME, 0);
					Projectile.velocity = Projectile.velocity.RotatedBy(RotationDirection * rotationmodifier * (MathHelper.TwoPi + MathHelper.Pi) / CIRCLETIME) * 0.997f;
					if (AiTimer > CIRCLETIME)
					{
						AiState = STATE_CIRCLINGCC;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_CIRCLINGCC: //then do a full circle in the other direction
					Projectile.alpha = Math.Max(Projectile.alpha - 255 / FADEINTIME, 0);
					Projectile.velocity = Projectile.velocity.RotatedBy(RotationDirection * -MathHelper.TwoPi / CIRCLETIME) * 0.997f;
					if (AiTimer > CIRCLETIME)
					{
						AiState = STATE_LOCKEDON;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_LOCKEDON: //briefly aim towards the player slowly
					if (Projectile.velocity.Length() > 3)
						Projectile.velocity *= 0.97f;

					Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Target.Center) * Projectile.velocity.Length(), 0.1f));
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Target.Center) * 3, 0.02f);

					float sinePeriod = LOCKONTIME;
					float sineStrength = MathHelper.PiOver4;
					Projectile.position += (Projectile.velocity.RotatedBy(sineStrength * Math.Sin(MathHelper.TwoPi * AiTimer / sinePeriod)) / 4) - (Projectile.velocity/4);
					if (AiTimer > LOCKONTIME)
					{
						AiState = STATE_ACCELERATE;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_ACCELERATE: //then accelerate with weak homing
					if (Projectile.velocity.Length() < 22)
						Projectile.velocity *= 1.035f;

					Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Target.Center) * Projectile.velocity.Length(), 0.02f));
					
					sinePeriod = ACCELLIFETIME / 4;
					sineStrength = MathHelper.Pi / 6;
					Projectile.position += (Projectile.velocity.RotatedBy(sineStrength * Math.Sin(MathHelper.TwoPi * AiTimer / sinePeriod)) / 2) - (Projectile.velocity / 2);

					if (AiTimer > ACCELLIFETIME)
					{
						AiState = STATE_FADEOUT;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_FADEOUT: //slow down and fade away
					Projectile.velocity *= 0.95f;
					Projectile.alpha += 255 / FADETIME;
					if (Projectile.alpha >= 255)
						Projectile.Kill();
					break;
			}
			++AiTimer;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Lighting.AddLight(Projectile.Center, Color.Magenta.ToVector3() * Projectile.Opacity * 1.5f);

			if (Main.rand.NextBool(4) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new GlowParticle(Projectile.Center, Projectile.velocity * Main.rand.NextFloat(0.2f), Color.Red * Projectile.Opacity * 1.5f, Main.rand.NextFloat(0.04f, 0.06f) * Projectile.scale, 40));

			if (AiTimer == 1 && AiState == STATE_PASSIVEMOVEMENT) //initialize on first tick
				for (int i = 0; i < _posArray.Length; i++)
					_posArray[i] = Projectile.Center;

			//Old position array for drawing tail
			for (int i = _posArray.Length - 1; i > 0; i--)
				_posArray[i] = _posArray[i - 1];

			_posArray[0] = Projectile.Center;
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

		public override bool PreDraw(ref Color lightColor)
		{
			Effect effect = SpiritMod.ShaderDict["PrimitiveTextureMap"];
			effect.Parameters["uTexture"].SetValue(Mod.Assets.Request<Texture2D>("NPCs/Boss/Occultist/SoulTrail").Value);

			Vector2[] vertices = _posArray;
			var strip = new PrimitiveStrip
			{
				Color = Color.White * Projectile.Opacity,
				Width = 13 * Projectile.scale,
				PositionArray = vertices,
				TaperingType = StripTaperType.None,
			};
			PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			var origin = new Vector2(Projectile.DrawFrame().Width / 2, Projectile.DrawFrame().Height);
			float headRotation = (_posArray[0] - _posArray[1]).ToRotation() + MathHelper.PiOver2;
			Main.spriteBatch.Draw(projTexture, _posArray[0] - Main.screenPosition, Projectile.DrawFrame(), Projectile.GetAlpha(Color.White), headRotation, origin, Projectile.scale, SpriteEffects.None, 0);
			
			return false;
		}

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos)
		{
			Vector2[] vertices = _posArray;
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;

			for (int i = 1; i < vertices.Length; i++)
			{
				float progress = i / (float)vertices.Length;
				float scale = MathHelper.Lerp(0.25f, 0f, progress);
				float dist = Vector2.Distance(vertices[i], vertices[i - 1]);
				float rot = (vertices[i] - vertices[i - 1]).ToRotation();
				Vector2 scaleVec = scale * new Vector2(dist / 20, 3);
				sB.Draw(bloom, vertices[i] - screenPos, null, Color.Red * Math.Min(1.2f * Projectile.Opacity, 0.6f), rot + MathHelper.PiOver2, bloom.Size() / 2, scaleVec, SpriteEffects.None, 0);
			}
			float headRotation = (_posArray[0] - _posArray[1]).ToRotation() + MathHelper.PiOver2;
			sB.Draw(bloom, Projectile.Center - new Vector2(0, Projectile.DrawFrame().Height / 4).RotatedBy(headRotation) - screenPos, null, Color.Red * Math.Min(1.2f * Projectile.Opacity, 0.6f) * Projectile.Opacity, 0, bloom.Size() / 2, 0.3f, SpriteEffects.None, 0);
		}
	}
}