using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Skies.Starjinx
{
	public class StarjinxSky : CustomSky
	{
		private Texture2D _bgTexture;
		private Texture2D _planet;
		private Texture2D _planetGlow;
		private Texture2D _planetBloom;

		private const float PLANET_PARALLAX_FARTHEST = 0.1f;
		private const float PLANET_PARALLAX_MID = 0.22f;
		private const float PLANET_PARALLAX_CLOSE = 0.4f;

		private bool _isActive;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			_bgTexture = SpiritMod.Instance.GetTexture("Skies/Starjinx/StarjinxSky");
			_planet = SpiritMod.Instance.GetTexture("Skies/Starjinx/StarjinxPlanet");
			_planetGlow = SpiritMod.Instance.GetTexture("Skies/Starjinx/StarjinxPlanet_glow");
			_planetBloom = SpiritMod.Instance.GetTexture("Effects/Ripple");
		}

		private List<StarjinxBGStar> Stars = new List<StarjinxBGStar>();

		private void ClearStars(ref List<StarjinxBGStar> starGroup)
		{
			List<StarjinxBGStar> dummy = new List<StarjinxBGStar>();
			foreach (StarjinxBGStar star in starGroup)
				if (star.Opacity >= 0)
					dummy.Add(star);

			starGroup = dummy;
		}

		private void DrawStars(SpriteBatch sB, List<StarjinxBGStar> starGroup, Vector2 ParallaxOffset)
		{
			foreach (StarjinxBGStar star in starGroup.OrderBy(x => x.Parallax))
				star.Draw(sB, ParallaxOffset);
		}

		private void AddBGStar()
		{
			//calculates average distance between planet parallax values, then divides by 4
			float scaleVariance = MathHelper.Lerp(PLANET_PARALLAX_CLOSE - PLANET_PARALLAX_MID, PLANET_PARALLAX_MID - PLANET_PARALLAX_FARTHEST, 0.5f);
			scaleVariance /= 4;

			float distFromScreen = Main.rand.NextBool(4) ?
				MathHelper.Lerp(PLANET_PARALLAX_CLOSE, PLANET_PARALLAX_MID, 0.5f) + Main.rand.NextFloat(-scaleVariance, scaleVariance) :
				MathHelper.Lerp(PLANET_PARALLAX_FARTHEST, PLANET_PARALLAX_MID, 0.5f) + Main.rand.NextFloat(-scaleVariance, scaleVariance);

			Stars.Add(new StarjinxBGStar(
				new Vector2(Main.rand.NextFloat(Main.screenWidth), Main.rand.NextFloat(Main.screenHeight)), //Position
				Vector2.UnitX * Main.rand.NextFloat(8, 12), //Velocity
				Main.rand.NextBool() ? Color.White : (Main.rand.NextBool() ? new Color(0, 247, 255) : new Color(255, 56, 205)), // Color
				distFromScreen)); //Parallax
		}

		public override void Update(GameTime gameTime)
		{
			if (!Main.gamePaused) //spaw a stars on the left of the screen when unpaused
			{
				while (Stars.Count < 60) //if too little onscreen, spawn more
					AddBGStar();

				if (Main.rand.NextBool(8))
					AddBGStar();
			}

			if (_isActive) 
				_fadeOpacity = Math.Min(1f, 0.025f + _fadeOpacity);

			else 
				_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.025f);
			

			if (Stars.Any() && !Main.gamePaused)
			{
				foreach (StarjinxBGStar star in Stars)
					star.Update();
				ClearStars(ref Stars);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {

				float Time = (float)Main.time;
				if (!Main.dayTime)
					Time += (float)Main.dayLength;

				float TimeLerp = Time / (float)(Main.dayLength + Main.nightLength);
				float minLerpStrength = 0.22f;
				TimeLerp = MathHelper.Clamp((float)-Math.Sin(-(MathHelper.Pi / 8) + (TimeLerp * MathHelper.TwoPi)), minLerpStrength, 0.3f);

				//Draw the background sky gradient
				spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), 
					Color.Lerp(Color.LightBlue, Color.Black, TimeLerp * 2f) * _fadeOpacity);

				//Method for drawing the planets and their glowmasks
				Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
				Vector2 Parallax = Main.LocalPlayer.GetModPlayer<NPCs.StarjinxEvent.StarjinxPlayer>().StarjinxPosition - Main.screenPosition - screenCenter;

				void DrawPlanet(Vector2 position, float distFromScreen, float timerOffset, float rotation, SpriteEffects effects)
				{
					float Scale = 4f * distFromScreen;
					rotation += (float)Math.Sin(Main.GlobalTime/2 + timerOffset) * 0.03f;
					Vector2 verticalOffset = Vector2.UnitY * (float)Math.Sin(Main.GlobalTime + timerOffset) * 20 * distFromScreen;
					Color PlanetColor = Color.Lerp(Color.White, Color.Black, MathHelper.Clamp(1.25f * TimeLerp + (1 - distFromScreen) * 0.6f, 0, 1));
					Color GlowColor = Color.Lerp(PlanetColor, Color.White, 0.5f);
					spriteBatch.Draw(_planetBloom, position + verticalOffset + Parallax * distFromScreen, null, GlowColor.MultiplyRGB(Color.HotPink) * 2 * _fadeOpacity, rotation, _planetBloom.Size() / 2, Scale * 10f, SpriteEffects.None, 0);
					spriteBatch.Draw(_planet, position + verticalOffset + Parallax * distFromScreen, null, PlanetColor * _fadeOpacity, rotation, _planet.Size() / 2, Scale, effects, 0);
					for (int i = 0; i < 6; i++)
					{
						float timer = (float)Math.Sin(timerOffset + Main.GlobalTime * 2) / 2 + 0.5f;
						Vector2 offset = Vector2.UnitX.RotatedBy(rotation + MathHelper.TwoPi * i / 6f) * 20 * distFromScreen * timer;
						spriteBatch.Draw(_planetGlow, position + verticalOffset + offset + Parallax * distFromScreen, null, GlowColor * _fadeOpacity * (1 - timer), rotation, _planet.Size() / 2, Scale, effects, 0);
					}
					spriteBatch.Draw(_planetGlow, position + verticalOffset + Parallax * distFromScreen, null, GlowColor * _fadeOpacity, rotation, _planet.Size() / 2, Scale, effects, 0);
				}

				//Draw furthest planet
				DrawPlanet(screenCenter - new Vector2(-600, 500), PLANET_PARALLAX_FARTHEST, 7, -MathHelper.Pi / 8, SpriteEffects.None);


				//Draw stars between furthest planet and middle planet
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, RasterizerState.CullNone);

				DrawStars(spriteBatch, Stars.Where(x => x.Parallax < PLANET_PARALLAX_MID).ToList(), Parallax);

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone);

				//Draw middle planet
				DrawPlanet(screenCenter - new Vector2(700, 300), PLANET_PARALLAX_MID, 3, MathHelper.Pi / 9f, SpriteEffects.FlipHorizontally);

				//Draw stars between middle planet and closest planet
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, RasterizerState.CullNone);

				DrawStars(spriteBatch, Stars.Where(x => x.Parallax >= PLANET_PARALLAX_MID).ToList(), Parallax);

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone);

				//Closest planet
				DrawPlanet(screenCenter - new Vector2(0 , -150), PLANET_PARALLAX_CLOSE, 0, 0, SpriteEffects.None);
			}
		}

		public override float GetCloudAlpha() => (1f - _fadeOpacity);

		public override void Activate(Vector2 position, params object[] args) => _isActive = true;
		public override void Deactivate(params object[] args) => _isActive = false;

		public override void Reset() => _isActive = false;

		public override bool IsActive()
		{
			if (!_isActive) 
				return _fadeOpacity > 0.001f;

			return true;
		}
	}

	public class StarjinxBGStar
	{
		private readonly Texture2D Texture = SpiritMod.Instance.GetTexture("Effects/Masks/Star");
		private readonly Texture2D BloomTexture = SpiritMod.Instance.GetTexture("Effects/Masks/CircleGradient");
		public Vector2 Position;
		public Vector2 Velocity;
		public float Scale = 0;
		public float MaxScale;
		public Color Color;
		public float Parallax;
		public int Timer = 0;
		public int MaxTime;
		private bool Dying = false;
		public float Opacity = 0;

		private readonly float Amplitude;
		private readonly int Period;
		private readonly int PeriodOffset;


		private const int FADETIME = 60;

		public StarjinxBGStar(Vector2 Pos, Vector2 Vel, Color color, float ParallaxStrength)
		{
			Position = Pos;

			Parallax = ParallaxStrength;
			Velocity = Vel * Parallax;

			MaxScale = Parallax * 40f / Texture.Width;

			Color = color;
			Color = Color.Lerp(Color, Color.Black, 1 - Parallax);

			MaxTime = (int)(600 / Parallax);

			Period = (int)(Main.rand.NextFloat(MaxTime / 2, MaxTime) * Parallax);
			Amplitude = Main.rand.NextFloat(0.5f, 1f) * MathHelper.Pi / 32;
			PeriodOffset = Main.rand.Next(Period);
		}

		public void Update()
		{
			if (Timer++ < FADETIME) // Fade in and grow at start of lifetime
			{
				Opacity += 0.66f / FADETIME;
				Scale += MaxScale / FADETIME;
			}

			// Fadeout and shrink when close to dying, or when starjinx sky is inactive
			else if (Timer > MaxTime - FADETIME || !Main.LocalPlayer.GetModPlayer<NPCs.StarjinxEvent.StarjinxPlayer>().zoneStarjinxEvent || Dying)
			{
				Opacity -= 0.66f / FADETIME;
				Dying = true;
				Scale -= MaxScale / FADETIME;
			}

			// Flicker
			else
				Scale = MaxScale * (1 + (float)Math.Sin((Timer - FADETIME + PeriodOffset) / 5) / 10);

			Position += Velocity.RotatedBy(Amplitude * Math.Sin(MathHelper.TwoPi * (Timer + PeriodOffset) / Period)); // Sine wave movement

			//screen wrapping horizontally
			Position.X %= Main.screenWidth;
			while (Position.X < 0)
				Position.X += Main.screenWidth;
		}

		public void Draw(SpriteBatch sB, Vector2 ParallaxOffset)
		{
			float Time = (float)Main.time;
			if (!Main.dayTime)
				Time += (float)Main.dayLength;

			float TimeLerp = Time / (float)(Main.dayLength + Main.nightLength);
			TimeLerp = MathHelper.Clamp((float)-Math.Sin(-(MathHelper.Pi / 8) + (TimeLerp * MathHelper.TwoPi)), 0.22f, 0.3f) * 2f;

			sB.Draw(BloomTexture, Position + ParallaxOffset * Parallax, null, Color * Opacity * MathHelper.Lerp(0.7f, 1f, TimeLerp), 0, BloomTexture.Size() / 2, Scale * 0.85f, SpriteEffects.None, 0);
			sB.Draw(Texture, Position + ParallaxOffset * Parallax, null, Color.Lerp(Color, Color.White, 0.66f) * Opacity, 0, Texture.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
