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

		private bool _isActive;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			_bgTexture = SpiritMod.Instance.GetTexture("Skies/Starjinx/StarjinxSky");
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

		private void DrawStars(SpriteBatch sB, List<StarjinxBGStar> starGroup)
		{
			foreach (StarjinxBGStar star in starGroup.OrderBy(x => x.Parallax))
				star.Draw(sB);
		}

		private void AddBGStar()
		{
			Stars.Add(new StarjinxBGStar(
				new Vector2(Main.rand.NextFloat(Main.screenWidth), Main.rand.NextFloat(Main.screenHeight)), //Position
				Vector2.UnitX * Main.rand.NextFloat(8, 12), //Velocity
				Main.rand.NextBool() ? Color.White : (Main.rand.NextBool() ? new Color(0, 247, 255) : new Color(255, 56, 205)), // Color
				Main.rand.NextFloat(0.15f, 0.5f))); //Parallax
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
				TimeLerp = MathHelper.Clamp((float)-Math.Sin(-(MathHelper.Pi / 8) + (TimeLerp * MathHelper.TwoPi)), 0.22f, 0.3f) * 2f;
				spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), 
					Color.Lerp(Color.LightBlue, Color.Black, TimeLerp) * _fadeOpacity);

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, RasterizerState.CullNone);

				DrawStars(spriteBatch, Stars);

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone);
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

			MaxScale = Parallax * Main.rand.NextFloat(25f, 30f) / Texture.Width;

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

		public void Draw(SpriteBatch sB)
		{
			float Time = (float)Main.time;
			if (!Main.dayTime)
				Time += (float)Main.dayLength;

			float TimeLerp = Time / (float)(Main.dayLength + Main.nightLength);
			TimeLerp = MathHelper.Clamp((float)-Math.Sin(-(MathHelper.Pi / 8) + (TimeLerp * MathHelper.TwoPi)), 0.22f, 0.3f) * 2f;

			sB.Draw(BloomTexture, Position, null, Color * Opacity * MathHelper.Lerp(0.7f, 1f, TimeLerp), 0, BloomTexture.Size() / 2, Scale * 0.85f, SpriteEffects.None, 0);
			sB.Draw(Texture, Position, null, Color.Lerp(Color, Color.White, 0.5f) * Opacity, 0, Texture.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
