using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class FireParticle : Particle
	{
		private Color _startColor;
		private Color _endColor;
		public int MaxTime;

		private const float FADETIME = 0.3f;

		public delegate void UpdateAction(Particle particle);

		private readonly UpdateAction _action;

		public override bool UseAdditiveBlend => true;

		public override bool UseCustomDraw => true;

		public FireParticle(Vector2 position, Vector2 velocity, Color startColor, Color endColor, float scale, int maxTime, UpdateAction action = null)
		{
			Position = position;
			Velocity = velocity;
			_startColor = startColor;
			_endColor = endColor;
			Scale = scale;
			MaxTime = maxTime;
			_action = action;
		}

		public override void Update()
		{
			float fadeintime = MaxTime * FADETIME;
			Color = Color.Lerp(_startColor, _endColor, TimeActive / (float)MaxTime);
			if (TimeActive < fadeintime)
				Color *= (TimeActive / fadeintime);
			else if(TimeActive > (MaxTime - fadeintime))
				Color *= ((MaxTime - TimeActive) / fadeintime);

			Lighting.AddLight(Position, Color.ToVector3() * Scale * 0.5f);

			if (_action != null)
				_action.Invoke(this);

			if (TimeActive > MaxTime)
				Kill();
		}

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Texture2D bloom = ModContent.Request<Texture2D>("Effects/Masks/CircleGradient");
			spriteBatch.Draw(bloom, Position - Main.screenPosition, null, Color * 0.6f, 0, bloom.Size() / 2, Scale / 5f, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, Position - Main.screenPosition, null, Color, 0, tex.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}
