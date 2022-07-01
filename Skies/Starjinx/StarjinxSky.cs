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
			_bgTexture = ModContent.Request<Texture2D>("Skies/Starjinx/StarjinxSky", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			_planet = ModContent.Request<Texture2D>("Skies/Starjinx/StarjinxPlanet", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			_planetGlow = ModContent.Request<Texture2D>("Skies/Starjinx/StarjinxPlanet_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			_planetBloom = ModContent.Request<Texture2D>("Effects/Ripple", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
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
			//calculates average distance between planet parallax values, then divides
			float scaleVariance = MathHelper.Lerp(PLANET_PARALLAX_CLOSE - PLANET_PARALLAX_MID, PLANET_PARALLAX_MID - PLANET_PARALLAX_FARTHEST, 0.5f);
			scaleVariance /= 6;

			bool frontLayer = Main.rand.NextBool(3);
			float distFromScreen = frontLayer ?
				MathHelper.Lerp(PLANET_PARALLAX_CLOSE, PLANET_PARALLAX_MID, 0.5f) + Main.rand.NextFloat(-scaleVariance, scaleVariance) :
				MathHelper.Lerp(PLANET_PARALLAX_FARTHEST, PLANET_PARALLAX_MID, 0.5f) + Main.rand.NextFloat(-scaleVariance, scaleVariance);

			Vector2 velocity = Vector2.UnitX * (frontLayer ?
				Main.rand.NextFloat(8, 12) :
				-Main.rand.NextFloat(6, 8));

			Stars.Add(new StarjinxBGStar(
				new Vector2(Main.rand.NextFloat(Main.screenWidth), Main.rand.NextFloat(Main.screenHeight)), //Position
				velocity, //Velocity
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

		public static float GetTimeDarkness()
		{
			float Time = (float)Main.time;
			if (!Main.dayTime)
				Time += (float)Main.dayLength;

			float value = Time / (float)(Main.dayLength + Main.nightLength);
			value = MathHelper.Clamp((float)-Math.Sin(-(MathHelper.Pi / 8) + (value * MathHelper.TwoPi)), 0.25f, 0.34f);
			return value;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f) {
				float TimeLerp = GetTimeDarkness();

				//Draw the background sky gradient
				spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), 
					Color.Lerp(Color.LightBlue, Color.Black, TimeLerp * 2f) * _fadeOpacity);

				Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
				Vector2 Parallax = Main.LocalPlayer.GetModPlayer<NPCs.StarjinxEvent.StarjinxPlayer>().StarjinxPosition - Main.screenPosition - screenCenter;

				//Method for drawing the planets and their glowmasks
				void DrawPlanet(Vector2 position, float distFromScreen, float timerOffset, float rotation, SpriteEffects effects)
				{
					float Scale = 4f * distFromScreen;
					rotation += (float)Math.Sin(Main.GlobalTimeWrappedHourly/2 + timerOffset) * 0.03f;
					Vector2 verticalOffset = Vector2.UnitY * (float)Math.Sin((Main.GlobalTimeWrappedHourly / 2) + timerOffset) * 40 * distFromScreen;
					Color PlanetColor = Color.Lerp(Color.White, Color.Black, MathHelper.Clamp(1.25f * TimeLerp + (1 - distFromScreen) * 0.6f, 0, 1));
					Color GlowColor = Color.Lerp(PlanetColor, Color.White, 0.5f);
					spriteBatch.Draw(_planetBloom, position + verticalOffset + Parallax * distFromScreen, null, GlowColor.MultiplyRGB(Color.HotPink) * 2 * _fadeOpacity, rotation, _planetBloom.Size() / 2, Scale * 10f, SpriteEffects.None, 0);
					spriteBatch.Draw(_planet, position + verticalOffset + Parallax * distFromScreen, null, PlanetColor * _fadeOpacity, rotation, _planet.Size() / 2, Scale, effects, 0);
					for (int i = 0; i < 6; i++)
					{
						float timer = (float)Math.Sin(timerOffset + Main.GlobalTimeWrappedHourly * 2) / 2 + 0.5f;
						Vector2 offset = Vector2.UnitX.RotatedBy(rotation + MathHelper.TwoPi * i / 6f) * 20 * distFromScreen * timer;
						spriteBatch.Draw(_planetGlow, position + verticalOffset + offset + Parallax * distFromScreen, null, GlowColor * _fadeOpacity * (1 - timer), rotation, _planet.Size() / 2, Scale, effects, 0);
					}
					spriteBatch.Draw(_planetGlow, position + verticalOffset + Parallax * distFromScreen, null, GlowColor * _fadeOpacity, rotation, _planet.Size() / 2, Scale, effects, 0);
				}

				//Draw furthest planet
				DrawPlanet(screenCenter - new Vector2(-600, 500), PLANET_PARALLAX_FARTHEST, 7, -MathHelper.Pi / 8, SpriteEffects.None);


				//Draw stars between furthest planet and middle planet

				DrawStars(spriteBatch, Stars.Where(x => x.Parallax < PLANET_PARALLAX_MID).ToList(), Parallax);

				//Draw middle planet
				DrawPlanet(screenCenter - new Vector2(700, 300), PLANET_PARALLAX_MID, 3, MathHelper.Pi / 9f, SpriteEffects.FlipHorizontally);

				//Draw stars between middle planet and closest planet

				DrawStars(spriteBatch, Stars.Where(x => x.Parallax >= PLANET_PARALLAX_MID).ToList(), Parallax);

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
		private readonly Texture2D Texture = ModContent.Request<Texture2D>("Effects/Masks/Star", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		private readonly Texture2D BloomTexture = ModContent.Request<Texture2D>("Effects/Masks/CircleGradient", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
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
			float TimeLerp = StarjinxSky.GetTimeDarkness();

			Color starColor = Color.Lerp(Color, Color.White, 0.33f) * Opacity;
			Color bloomColor = Color * Opacity * MathHelper.Lerp(0.7f, 1f, TimeLerp);
			starColor.A = 0;
			bloomColor.A = 0;

			sB.Draw(BloomTexture, Position + ParallaxOffset * Parallax, null, bloomColor, 0, BloomTexture.Size() / 2, Scale * 0.85f, SpriteEffects.None, 0);
			sB.Draw(Texture, Position + ParallaxOffset * Parallax, null, starColor, 0, Texture.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
