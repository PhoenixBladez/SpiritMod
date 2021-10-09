using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using SpiritMod.Particles;
using System.IO;
using SpiritMod.Prim;
using SpiritMod.VerletChains;
using System.Collections.Generic;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaStarfire : ModProjectile, ITrailProjectile, IDrawAdditive
    {
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Starfire");

		public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.timeLeft = 80;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.scale = Main.rand.NextFloat(1f, 1.2f);
        }

		public Vector2 InitialVelocity = Vector2.Zero;
		public Vector2 TargetVelocity = Vector2.Zero;

		private const int VelocityLerpTime = 12;

		public float Amplitude = MathHelper.Pi / 12;
		private const float Period = 25;

		private const float MaxSpeed = 20;

		private ref float AiTimer => ref projectile.ai[0];
		private readonly List<Chain> _chains = new List<Chain>();
		private float _sinAmplitude;
		private float _sinCounter;

		public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			_sinAmplitude = MathHelper.Lerp(_sinAmplitude, Amplitude * 30, 0.06f);
			_sinCounter += 0.22f;

			if (projectile.alpha > 0)
				projectile.alpha = Math.Max(projectile.alpha - 10, 0);

			if (++AiTimer <= VelocityLerpTime) //lerping to desired vel
				projectile.velocity = Vector2.Lerp(InitialVelocity, TargetVelocity, (float)Math.Pow(AiTimer / VelocityLerpTime, 1.5f));

			else //behavior afterwards
			{
				if (TargetVelocity.Length() < MaxSpeed)
					TargetVelocity *= 1.025f;

				projectile.velocity = TargetVelocity.RotatedBy((float)Math.Sin(MathHelper.TwoPi * (AiTimer - VelocityLerpTime) / Period) * Amplitude);
			}

			if (Main.rand.NextBool(6) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.4f), Color.White * 0.5f,
					SpiritMod.StarjinxColor(Main.GlobalTime - 1) * 0.5f, Main.rand.NextFloat(0.1f, 0.2f), 25));

			if (!(_chains.Count > 0))
			{
				int chainamount = 2;
				for (int i = 0; i < chainamount; i++)
				{
					float length = MathHelper.Lerp(4f, 3f, i / (float)chainamount);
					_chains.Add(new Chain(length, 12, projectile.Center, new ChainPhysics(0.8f, 0f, 0f), true, false, 7));
				}
			}
			else
				for(int i = 0; i < _chains.Count; i++)
				{
					float amplitude = MathHelper.Lerp(_sinAmplitude, -_sinAmplitude * 1.25f, i / (float)_chains.Count);
					UpdateChain(_chains[i], amplitude);
				}
		}

		private void UpdateChain(Chain chain, float amplitude)
		{
			chain.Update(projectile.Center, projectile.Center);
			for (int i = 0; i < chain.Vertices.Count; i++)
			{
				ChainVertex vertex = chain.Vertices[i];
				float progress = i / (float)(chain.Vertices.Count);
				Vector2 lastPos = projectile.position;
				if (i > 0)
					lastPos = chain.Vertices[i - 1].Position;

				var DirectionUnit = Vector2.Normalize(lastPos - vertex.Position);
				DirectionUnit = DirectionUnit.RotatedBy(MathHelper.PiOver2);
				float numwaves = 1f;
				vertex.Position += DirectionUnit * (float)Math.Sin(_sinCounter + progress * MathHelper.TwoPi * numwaves) * amplitude;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(TargetVelocity);
			writer.WriteVector2(InitialVelocity);
			writer.Write(Amplitude);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			TargetVelocity = reader.ReadVector2();
			InitialVelocity = reader.ReadVector2();
			Amplitude = reader.ReadSingle();
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime, 1, 0.8f), new RoundCap(), new DefaultTrailPosition(), 50f * projectile.scale, 250f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.02f, 1f, 1f));

		public override bool PreDraw(SpriteBatch sB, Color lightColor)
		{
			#region primitives

			//chain
			if (_chains.Count > 0)
			{
				//set the parameters for the shader
				Color startColor = new Color(242, 240, 134);
				Color endColor = new Color(255, 69, 187);
				Effect effect = SpiritMod.ShaderDict["FlameTrail"];
				effect.Parameters["uTexture"].SetValue(mod.GetTexture("Textures/Trails/Trail_3"));
				effect.Parameters["uTexture2"].SetValue(mod.GetTexture("Textures/Trails/Trail_4"));
				effect.Parameters["Progress"].SetValue(Main.GlobalTime);
				effect.Parameters["StartColor"].SetValue(startColor.ToVector4());
				effect.Parameters["EndColor"].SetValue(endColor.ToVector4());

				for(int i = 0; i < _chains.Count; i++)
				{
					//projectile's center needs to manually be added to the start of the array
					var temp = new List<Vector2> { projectile.Center };
					temp.AddRange(_chains[i].VerticesArray());
					Vector2[] vertices = temp.ToArray();
					//draw the strip with the given array
					float width = MathHelper.Lerp(22, 12, i / (float)_chains.Count);
					var strip = new PrimitiveStrip
					{
						Color = Color.White * projectile.Opacity,
						Width = width * projectile.scale,
						PositionArray = vertices,
						TaperingType = StripTaperType.TaperEndQuadratic,
					};
					PrimitiveRenderer.DrawPrimitiveShape(strip, effect);
				}

				var circle = new CirclePrimitive
				{
					Color = Color.White * projectile.Opacity,
					Radius = 30f * projectile.scale,
					Position = projectile.Center - Main.screenPosition,
					MaxRadians = MathHelper.TwoPi
				};
				PrimitiveRenderer.DrawPrimitiveShape(circle, effect);

				//star
				StarPrimitive star = new StarPrimitive
				{
					Color = Color.White * projectile.Opacity,
					TriangleHeight = 9 * projectile.scale,
					TriangleWidth = 3 * projectile.scale,
					Position = projectile.Center - Main.screenPosition,
					Rotation = projectile.rotation
				};
				PrimitiveRenderer.DrawPrimitiveShape(star);
			}
			#endregion

			return false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			#region blur lines

			Texture2D tex = Main.extraTexture[89];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 8f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.25f, 2f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.25f, 4f) * Timer;
			Color blurcolor = Color.Lerp(SpiritMod.StarjinxColor(Main.GlobalTime - 1), Color.White, 0.33f) * projectile.Opacity;
			sB.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			sB.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);

			#endregion
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffects();

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffects();

		private void HitEffects()
		{
			if (!Main.dedServ)
			{
				for (int i = 0; i < 5; i++)
				{
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, projectile.velocity.RotatedByRandom(3 * MathHelper.Pi / 4) * Main.rand.NextFloat(0.5f, 0.75f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5) * 0.5f, Main.rand.NextFloat(0.5f, 0.7f), 15, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.4f) * 0.85f;
						}));
				}

				for (int i = 0; i < 2; i++)
				{
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(0.15f, 0.3f), Color.White,
						SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.2f, 0.4f), 25));
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!Main.dedServ)
			{
				Vector2 velnormal = Vector2.Normalize(projectile.velocity);
				velnormal *= 4;

				for (int i = 0; i < 2; i++) //weak burst of particles in direction of movement
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5), Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
						}));

				for (int i = 0; i < 3; i++) //wide burst of slower moving particles in opposite direction
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -velnormal.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(1f, 1.5f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5), Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.1f) * 0.98f;
						}));


				for (int i = 0; i < 2; i++) //narrow burst of faster, bigger particles
					ParticleHandler.SpawnParticle(new FireParticle(projectile.Center, -velnormal.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 2.5f),
						SpiritMod.StarjinxColor(Main.GlobalTime), SpiritMod.StarjinxColor(Main.GlobalTime + 5), Main.rand.NextFloat(0.5f, 0.7f), 25, delegate (Particle p)
						{
							p.Velocity = p.Velocity.RotatedByRandom(0.15f) * 0.98f;
							p.Velocity.Y += 0.25f;
						}));

				for (int i = 0; i < 4; i++)
				{
					ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, oldVelocity.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(0.6f), Color.White,
						SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.2f, 0.4f), 25));
				}

				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithVolume(0.35f).WithPitchVariance(0.3f), projectile.Center);
			}
			return true;
		}
    }
}