using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;
using SpiritMod.Mechanics.EventSystem.Controllers;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Boss.SteamRaider;

namespace SpiritMod.Mechanics.EventSystem.Events
{
	public class StarplateBeaconIntroEvent : Event
	{
		private const float BUILDUP_LENGTH = 4f;
		private const float PAUSE_TIME = 0.5f;
		private const float BURST_LENGTH = 2f;
		private const float BURST_RADIUS = 4096f;
		private const float G = 6.6743e-11f;

		private BeaconShaderData _ripContrastData;
		private BeaconShaderData _burstData;
		private Vector2 _center;

		private EaseBuilder _screenRipStrength;
		private EaseBuilder _contrastBubbleStrength;
		private EaseBuilder _burstRadius;
		private EaseBuilder _burstStrength;
		private EaseBuilder _overlayOpacity;
		private EaseBuilder _particleMass;
		private EaseBuilder _particleSpawnCount;
		private EaseBuilder _flashOpacity;
		private float _gravEquationTop;
		private List<Particle> _particles;

		public StarplateBeaconIntroEvent(Vector2 center)
		{
			_center = center;

			// set up the screen shaders
			_ripContrastData = new BeaconShaderData(new Ref<Effect>(SpiritMod.Instance.GetEffect("Effects/EventShaders")), "BeaconStartScreen");
			_burstData = new BeaconShaderData(new Ref<Effect>(SpiritMod.Instance.GetEffect("Effects/EventShaders")), "BeaconBurstScreen");
			Filters.Scene["SpiritMod:Event-BeaconDistortion"] = new Filter(_ripContrastData, (EffectPriority)20);
			Filters.Scene["SpiritMod:Event-BeaconBurst"] = new Filter(_burstData, (EffectPriority)35);

			const float BURST_START = BUILDUP_LENGTH + PAUSE_TIME;

			// screen rip animation
			_screenRipStrength = new EaseBuilder();
			_screenRipStrength.AddPoint(0f, 0f, EaseFunction.Linear);
			_screenRipStrength.AddPoint(BUILDUP_LENGTH * 0.9f, 1f, EaseFunction.EaseCubicIn);
			_screenRipStrength.AddPoint(BURST_START, 1f, EaseFunction.EaseCubicIn);
			_screenRipStrength.AddPoint(BURST_START + BURST_LENGTH, 0f, EaseFunction.EaseCubicOut);

			// contrast animation
			_contrastBubbleStrength = new EaseBuilder();
			_contrastBubbleStrength.AddPoint(0f, 0f, EaseFunction.Linear);
			_contrastBubbleStrength.AddPoint(BUILDUP_LENGTH, 1f, new PolynomialEase(x => x * x * x * x * x * x * x));
			_contrastBubbleStrength.AddPoint(BURST_START, 1f, EaseFunction.Linear);
			_contrastBubbleStrength.AddPoint(BURST_START + BURST_LENGTH, 0f, EaseFunction.EaseQuadOut);

			// burst radius animation
			_burstRadius = new EaseBuilder();
			_burstRadius.AddPoint(0f, 0f, EaseFunction.Linear);
			_burstRadius.AddPoint(BURST_START, 0f, EaseFunction.Linear);
			_burstRadius.AddPoint(BURST_START + BURST_LENGTH, BURST_RADIUS, EaseFunction.EaseQuadOut);

			// burst strength animation
			_burstStrength = new EaseBuilder();
			_burstStrength.AddPoint(0f, 0f, EaseFunction.Linear);
			_burstStrength.AddPoint(BURST_START - 0.01f, 0f, EaseFunction.Linear);
			_burstStrength.AddPoint(BURST_START, 1f, EaseFunction.Linear);
			_burstStrength.AddPoint(BURST_START + BURST_LENGTH, 0f, EaseFunction.EaseQuadOut);

			// overlay opacity animation
			_overlayOpacity = new EaseBuilder();
			_overlayOpacity.AddPoint(0f, 0f, EaseFunction.Linear);
			_overlayOpacity.AddPoint(BURST_START, 1f, EaseFunction.EaseCubicIn);
			_overlayOpacity.AddPoint(BURST_START + 0.2f, 0f, EaseFunction.Linear);

			// particle mass animation
			_particleMass = new EaseBuilder();
			_particleMass.AddPoint(0f, 1f, EaseFunction.Linear);
			_particleMass.AddPoint(BUILDUP_LENGTH, 5000000000000000f, EaseFunction.EaseQuadIn);

			// particle spawn count animation
			_particleSpawnCount = new EaseBuilder();
			_particleSpawnCount.AddPoint(0f, 7.5f, EaseFunction.Linear);
			_particleSpawnCount.AddPoint(BUILDUP_LENGTH, 6f, EaseFunction.EaseQuarticIn);

			// flash opacity animation
			_flashOpacity = new EaseBuilder();
			_flashOpacity.AddPoint(0f, 0f, EaseFunction.Linear);
			_flashOpacity.AddPoint(BURST_START - 0.01f, 0f, EaseFunction.Linear);
			_flashOpacity.AddPoint(BURST_START + 0.04f, 1f, EaseFunction.Linear);
			_flashOpacity.AddPoint(BURST_START + 0.2f, 0f, EaseFunction.EaseQuadOut);

			_particles = new List<Particle>();

			// add a screen shake event to the queue
			AddToQueue(new ExpressionController(0, (int frame) =>
			{
				if(Main.netMode != NetmodeID.Server)
					Main.PlaySound(SpiritMod.instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/StarplateBlast"), center);
			}));

			// add a screen shake event to the queue
			AddToQueue(new ExpressionController(BUILDUP_LENGTH + PAUSE_TIME, (int frame) =>
			{
				EventManager.PlayEvent(new ScreenShake(20f, 0.6f));
			}));

			// add the summoning to the queue
			AddToQueue(new ExpressionController(BUILDUP_LENGTH + PAUSE_TIME + BURST_LENGTH - 0.5f, (int frame) =>
			{
				Player player = Main.LocalPlayer;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Main.NewText("The Starplate Voyager has awoken!", 175, 75, 255, true);
					int npcID = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<SteamRaiderHead>());
					BossTitles.SetNPCType(ModContent.NPCType<SteamRaiderHead>());
					Main.npc[npcID].Center = player.Center - new Vector2(600, 600);
					Main.npc[npcID].netUpdate2 = true;
				}
				else
				{
					SpiritMod.WriteToPacket(SpiritMod.instance.GetPacket(), (byte)MessageType.BossSpawnFromClient, (byte)player.whoAmI, (int)ModContent.NPCType<SteamRaiderHead>(), "The Starplate Voyager has awoken!", (int)player.Center.X - 600, (int)player.Center.Y - 600).Send(-1);
				}

			}));
		}

		public override void Activate()
		{
			// activate all filters
			Filters.Scene.Activate("SpiritMod:Event-BeaconDistortion", _center, null);
			Filters.Scene.Activate("SpiritMod:Event-BeaconBurst", _center, null);
			Filters.Scene.Activate("SpiritMod:Glitch", _center, null);
			base.Activate();
		}

		public override void Deactivate()
		{
			// deactivate all filters
			Filters.Scene["SpiritMod:Event-BeaconDistortion"].Deactivate(null);
			Filters.Scene["SpiritMod:Event-BeaconBurst"].Deactivate(null);
			Filters.Scene["SpiritMod:Glitch"].Deactivate(null);
			Main.LocalPlayer.GetSpiritPlayer().starplateGlitchEffect = false;
			base.Deactivate();
		}

		public override bool Update(float deltaTime)
		{
			base.Update(deltaTime);

			float endTime = (BUILDUP_LENGTH + PAUSE_TIME + BURST_LENGTH);

			float fadespeed = 2; //speed at which music fades out relative to the total amount of time taken, 2 would be fully fading out halfway through
			Main.musicFade[Main.curMusic] = Math.Max(1 - ((_currentTime * fadespeed) / endTime), 0);

			// update shader parameters
			_ripContrastData.Shader.Parameters["Time"].SetValue(_currentTime);
			_ripContrastData.Shader.Parameters["NoiseTexture"].SetValue(SpiritMod.Instance.GetTexture("Textures/Events/BigNoise"));
			_ripContrastData.Shader.Parameters["FieldCenter"].SetValue(_center);
			_ripContrastData.Shader.Parameters["ScreenRipStrength"].SetValue(_screenRipStrength.Ease(_currentTime));
			_ripContrastData.Shader.Parameters["ContrastBubbleStrength"].SetValue(_contrastBubbleStrength.Ease(_currentTime));
			_ripContrastData.Shader.Parameters["ScreenRipOffsetMultiplier"].SetValue((_currentTime < BUILDUP_LENGTH + PAUSE_TIME * 0.5f) ? 1f : -1f);
			_burstData.Shader.Parameters["FieldTexture"].SetValue(SpiritMod.Instance.GetTexture("Textures/Events/BeaconBurstTexture"));
			_burstData.Shader.Parameters["BurstRadius"].SetValue(_burstRadius.Ease(_currentTime));
			_burstData.Shader.Parameters["BurstStrength"].SetValue(_burstStrength.Ease(_currentTime));

			// glitch effect
			Main.LocalPlayer.GetSpiritPlayer().starplateGlitchEffect = true;

			// giltch effect paramaters
			float glitchMultiplier = _screenRipStrength.Ease(_currentTime);
			if (_currentTime >= BUILDUP_LENGTH)
			{
				glitchMultiplier *= Math.Max(1.0f - (_currentTime - BUILDUP_LENGTH) * 8f, 0f);
			}
			SpiritMod.glitchEffect.Parameters["Speed"].SetValue(glitchMultiplier * 0.3f); //0.4f is default
			SpiritMod.glitchScreenShader.UseIntensity(glitchMultiplier * 0.03f);

			// spawn new particles
			_gravEquationTop = G * _particleMass.Ease(_currentTime);
			int count = (int)_particleSpawnCount.Ease(_currentTime);
			if (_currentTime > BUILDUP_LENGTH) count = 0;
			for (int i = 0; i < count; i++)
			{
				float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
				float distance = Main.rand.NextFloat(128f, 1024f);
				_particles.Add(new Particle(_center + angle.ToRotationVector2() * distance, (angle + MathHelper.PiOver2).ToRotationVector2() * Main.rand.NextFloat(1f, 3f)));
			}

			// update existing particles (using gravitational equation, mass of 1 so F=V)
			for (int i = 0; i < _particles.Count; i++)
			{
				Particle particle = _particles[i];

				Vector2 newPos = particle.Position + particle.Velocity;
				particle.Position = newPos;

				Vector2 dir = _center - particle.Position;
				float r = dir.Length();
				float f = _gravEquationTop / (r * r);
				particle.Velocity += Vector2.Normalize(dir) * f;

				particle.Opacity = Math.Min(particle.Opacity, MathHelper.Clamp(EaseFunction.EaseQuadOut.Ease(r / 1024f), 0f, 1f));
				if (particle.Opacity <= 0f)
				{
					_particles.RemoveAt(i--);
					continue;
				}
				particle.Lifetime += deltaTime * 1.2f;
				if (particle.Lifetime > 1f) particle.Lifetime = 1f;

				_particles[i] = particle;
			}

			return (_currentTime >= endTime);
		}

		public override void DrawAtLayer(SpriteBatch spriteBatch, RenderLayers layer, bool beginSB)
		{
			// draw beacon overlay and particles
			if (layer == RenderLayers.TilesAndNPCs)
			{
				if (beginSB) spriteBatch.Begin();

				spriteBatch.Draw(SpiritMod.instance.GetTexture("Textures/Events/BeaconOverlay"), _center - new Vector2(16f, 10f) - Main.screenPosition, Color.White * _overlayOpacity.Ease(_currentTime));

				float overlayOpacity = 1.0f - _overlayOpacity.Ease(_currentTime);
				if (_currentTime < BUILDUP_LENGTH + PAUSE_TIME * 0.5f)
				{
					for (int i = 0; i < _particles.Count; i++)
					{
						Vector2 scale = _particles[i].Size * new Vector2(0.0625f);
						spriteBatch.Draw(Main.blackTileTexture, _particles[i].Position - Main.screenPosition, null, Color.White * _particles[i].Lifetime * _particles[i].Opacity * overlayOpacity, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
					}
				}

				if (beginSB) spriteBatch.End();
			}

			// draw screen flash
			if (layer == RenderLayers.All)
			{
				if (beginSB) spriteBatch.Begin();

				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.White * _flashOpacity.Ease(_currentTime));

				if (beginSB) spriteBatch.End();
			}
		}

		private struct Particle
		{
			public Vector2 Position;
			public Vector2 Velocity;
			public float Opacity;
			public float Lifetime;
			public float Size;
			public Particle(Vector2 p, Vector2 v) 
			{ 
				Position = p; 
				Velocity = v; 
				Opacity = 1f; 
				Lifetime = 0f;
				float r = Main.rand.NextFloat();
				Size = (r < 0.5f) ? 1f : (r < 0.8f ? 2f : 3f);
			}
		}

		public class BeaconShaderData : ScreenShaderData
		{
			public BeaconShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
			{
			}

			public override void Update(GameTime gameTime)
			{
			}

			public override void Apply()
			{
				// calculate screen related parameters right before applying.
				// matrix requires a half pixel offset
				var viewport = Main.graphics.GraphicsDevice.Viewport;
				Matrix m = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, -1);
				m.M41 += -0.5f * m.M11;
				m.M42 += -0.5f * m.M22;
				Shader.Parameters["MATRIX"].SetValue(m);
				Shader.Parameters["ScreenPosition"].SetValue(Main.screenPosition);
				Shader.Parameters["ScreenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));

				Shader.CurrentTechnique.Passes[_passName].Apply();
			}
		}
	}
}
