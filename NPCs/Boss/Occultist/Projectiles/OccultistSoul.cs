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

		private Chain _chain = null;
		private float _sinCounter;
		private float _sinAmplitude = 0;

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

					_sinAmplitude = MathHelper.Lerp(_sinAmplitude, 12f, 0.1f);
					_sinCounter += 0.3f;
					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.1f));
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * 3, 0.02f);
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

					_sinCounter += 0.4f;
					_sinAmplitude = MathHelper.Lerp(_sinAmplitude, 8f, 0.1f);
					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Target.Center) * projectile.velocity.Length(), 0.02f));

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

			if (_chain == null)
				_chain = new Chain(3, 18, projectile.Center, new ChainPhysics(0.8f, 0f, 0f), true, false, 7);
			else
				_chain.Update(projectile.Center, projectile.Center);
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
			if (_chain != null)
			{
				Effect effect = SpiritMod.ShaderDict["PrimitiveTextureMap"];
				effect.Parameters["uTexture"].SetValue(mod.GetTexture("NPCs/Boss/Occultist/SoulTrail"));

				Vector2[] vertices = _chain.VerticesArray();
				vertices.IterateArray(delegate (ref Vector2 vertex, int index, float progress) { IterateVerticesSine(ref vertex, progress); });
				var strip = new PrimitiveStrip
				{
					Color = Color.White * projectile.Opacity,
					Width = 13 * projectile.scale,
					PositionArray = vertices,
					TaperingType = StripTaperType.None,
				};
				PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

				Texture2D projTexture = Main.projectileTexture[projectile.type];
				var origin = new Vector2(projectile.DrawFrame().Width / 2, projectile.DrawFrame().Height + 2);
				spriteBatch.Draw(projTexture, _chain.StartPosition - Main.screenPosition, projectile.DrawFrame(), projectile.GetAlpha(Color.White), _chain.StartRotation + MathHelper.PiOver2, origin, projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			if (_chain != null)
			{
				Vector2[] vertices = _chain.VerticesArray();
				vertices.IterateArray(delegate (ref Vector2 vertex, int index, float progress) { IterateVerticesSine(ref vertex, progress); });
				Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");

				for (int i = 0; i < vertices.Length; i++)
				{
					float progress = i / (float)vertices.Length;
					float scale = MathHelper.Lerp(0.25f, 0f, progress);
					sB.Draw(bloom, vertices[i] - Main.screenPosition, null, Color.Red * Math.Min(1.2f * projectile.Opacity, 0.6f), 0, bloom.Size() / 2, scale, SpriteEffects.None, 0);
				}
				sB.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Red * Math.Min(1.2f * projectile.Opacity, 0.6f) * projectile.Opacity, 0, bloom.Size() / 2, 0.3f, SpriteEffects.None, 0);
			}
		}

		private void IterateVerticesSine(ref Vector2 vertex, float progress)
		{
			var DirectionUnit = Vector2.Normalize(projectile.position - vertex);
			DirectionUnit = DirectionUnit.RotatedBy(MathHelper.PiOver2);
			float numwaves = 1;
			vertex += DirectionUnit * (float)Math.Sin(_sinCounter + progress * MathHelper.TwoPi * numwaves) * progress * _sinAmplitude;
		}
	}
}