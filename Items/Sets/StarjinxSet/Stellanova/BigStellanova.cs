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
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class BigStellanova : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override string Texture => "Terraria/Projectile_1";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Starfire");

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = 0;
			projectile.tileCollide = true;
			projectile.timeLeft = 720;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
			projectile.alpha = 155;
			projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
		}

		public Vector2 InitialVelocity = Vector2.Zero;
		public Vector2 TargetVelocity = Vector2.Zero;

		private const int VelocityLerpTime = 12;

		public float Amplitude = MathHelper.Pi / 16;
		private const float Period = 80;

		private ref float AiTimer => ref projectile.ai[0];
		private Chain _chain = null;
		private float _sinAmplitude;
		private float _sinCounter;

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;

			if (projectile.alpha > 0)
				projectile.alpha = Math.Max(projectile.alpha - 5, 0);

			if (++AiTimer <= VelocityLerpTime) //lerping to desired vel
				projectile.velocity = Vector2.Lerp(InitialVelocity, TargetVelocity, (float)Math.Pow(AiTimer / VelocityLerpTime, 1.5f));

			else //behavior afterwards
			{
				projectile.velocity = TargetVelocity.RotatedBy((float)Math.Sin(MathHelper.TwoPi * (AiTimer - VelocityLerpTime) / Period) * Amplitude);
				_sinAmplitude = MathHelper.Lerp(_sinAmplitude, Amplitude * 100, 0.03f);
			}

			if (Main.rand.NextBool(8) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.4f), Color.White * 0.5f,
					SpiritMod.StarjinxColor(Main.GlobalTime - 1) * 0.5f, Main.rand.NextFloat(0.1f, 0.2f), 25));

			_sinCounter += 0.18f;
			if (_chain == null)
				_chain = new Chain(8, 12, projectile.Center, new ChainPhysics(0.8f, 0f, 0f), true, false, 5);
			else
				_chain.Update(projectile.Center, projectile.Center);

			for (int i = 0; i < Main.maxProjectiles; ++i) //Gravitate StellanovaStarfire projectiles to this proj
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.DistanceSQ(projectile.Center) < 400 * 400 && p.type == ModContent.ProjectileType<StellanovaStarfire>())
				{
					p.velocity = p.velocity.Length() * Vector2.Normalize(Vector2.Lerp(p.velocity, p.DirectionTo(projectile.Center) * p.velocity.Length(), 0.1f)) * 0.95f;
					p.velocity += Vector2.Lerp(p.velocity, p.DirectionTo(projectile.Center) * 20, 0.1f) * 0.05f;
				}
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

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime, 1, 0.66f), new RoundCap(), new DefaultTrailPosition(), 50f * projectile.scale, 240f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.02f, 1f, 1f));

		public void AdditiveCall(SpriteBatch sB)
		{
			#region primitives and blooms
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

			//chain and blooms
			if (_chain != null)
			{
				//set the parameters for the shader
				Color startColor = new Color(242, 240, 134);
				Color endColor = new Color(255, 69, 187);
				Effect effect = SpiritMod.ShaderDict["FlameTrail"];
				effect.Parameters["uTexture"].SetValue(mod.GetTexture("Textures/Trails/Trail_3"));
				effect.Parameters["Progress"].SetValue(Main.GlobalTime);
				effect.Parameters["StartColor"].SetValue(startColor.ToVector4());
				effect.Parameters["EndColor"].SetValue(endColor.ToVector4());

				//projectile's center needs to manually be added to the start of the array
				var temp = new List<Vector2> { projectile.Center };
				temp.AddRange(_chain.VerticesArray());
				Vector2[] vertices = temp.ToArray();
				vertices.IterateArray(delegate (ref Vector2 vertex, int index, float progress) { IterateVerticesSine(ref vertex, progress); });
				//draw the strip with the given array
				var strip = new PrimitiveStrip
				{
					Color = Color.White * projectile.Opacity,
					Width = 25 * projectile.scale,
					PositionArray = vertices,
					TaperingType = StripTaperType.TaperEnd,
				};
				PrimitiveRenderer.DrawPrimitiveShape(strip, effect);

				var circle = new CirclePrimitive
				{
					Color = Color.White * projectile.Opacity,
					Radius = 30f * projectile.scale,
					Position = projectile.Center - Main.screenPosition,
					Rotation = _chain.StartRotation,
					MaxRadians = MathHelper.Pi
				};
				PrimitiveRenderer.DrawPrimitiveShape(circle, effect);

				//draw blooms at each position
				Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");

				for (int i = 0; i < vertices.Length; i++)
				{
					float progress = i / (float)vertices.Length;
					float scale = MathHelper.Lerp(0.4f, 0f, progress);
					Color color = Color.Lerp(startColor, endColor, progress);
					sB.Draw(bloom, vertices[i] - Main.screenPosition, null, color * Math.Min(1f * projectile.Opacity, 0.6f), 0, bloom.Size() / 2, scale, SpriteEffects.None, 0);
				}
				sB.Draw(bloom, projectile.Center - Main.screenPosition, null, startColor * Math.Min(1f * projectile.Opacity, 0.6f) * projectile.Opacity, 0, bloom.Size() / 2, 0.5f, SpriteEffects.None, 0);

			}
			#endregion

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

		private void IterateVerticesSine(ref Vector2 vertex, float progress)
		{
			var DirectionUnit = Vector2.Normalize(projectile.position - vertex);
			DirectionUnit = DirectionUnit.RotatedBy(MathHelper.PiOver2);
			float numwaves = 1f;
			vertex += DirectionUnit * (float)Math.Sin(_sinCounter + progress * MathHelper.TwoPi * numwaves) * ((progress / 4) + 0.75f) * _sinAmplitude;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

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
				Vector2 velnormal = Vector2.Normalize(projectile.velocity) * 4f;

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