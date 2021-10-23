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

namespace SpiritMod.Skies
{
	public class StarjinxBGStar
	{
		private Texture2D Texture = SpiritMod.instance.GetTexture("Effects/Masks/Star");
		private Texture2D BloomTexture = SpiritMod.instance.GetTexture("Effects/Masks/CircleGradient");
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

		private float Amplitude;
		private int Period;
		private int PeriodOffset;


		private const int FADETIME = 60;

		public StarjinxBGStar(Vector2 Pos)
		{
			Position = Pos;

			Parallax = Main.rand.NextFloat(0.33f, 1f);
			Velocity = Vector2.UnitX * Main.rand.NextFloat(4, 6f) * Parallax;

			MaxScale = Parallax * Main.rand.NextFloat(17f, 20f) / Texture.Width;

			Color = Main.rand.NextBool() ? Color.White : (Main.rand.NextBool() ? Color.Cyan : new Color(249, 98, 255));
			Color = Color.Lerp(Color, Color.Black, 1 - Parallax);

			MaxTime = (int)(600 / Parallax);

			Period = (int)Main.rand.NextFloat(MaxTime / 2, MaxTime);
			Amplitude = Main.rand.NextFloat(0.5f, 1f) * MathHelper.Pi / 16;
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
			sB.Draw(BloomTexture, Position, null, Color * Opacity * 0.5f, 0, BloomTexture.Size() / 2, Scale * 0.75f, SpriteEffects.None, 0);
			sB.Draw(Texture, Position, null, Color * Opacity, 0, Texture.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}

	public class StarjinxSky : CustomSky
	{
		private Texture2D _bgTexture;

		private bool _isActive;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			_bgTexture = SpiritMod.instance.GetTexture("Skies/StarjinxSky");
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

		public override void Update(GameTime gameTime)
		{
			if (_isActive) {
				_fadeOpacity = Math.Min(1f, 0.025f + _fadeOpacity);

				if (Main.rand.NextBool(10) && !Main.gamePaused) //spaw a stars on the left of the screen
					Stars.Add(new StarjinxBGStar(new Vector2(Main.rand.NextFloat(Main.screenWidth), Main.rand.NextFloat(Main.screenHeight))));

			}
			else {
				_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.025f);
			}

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
				spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), 
					Color.Lerp(Color.White, Color.Black, 0.2f) * _fadeOpacity);

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
}
