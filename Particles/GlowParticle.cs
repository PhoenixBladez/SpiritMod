using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class GlowParticle : Particle
	{
		private float opacity;
		private Color glowColor;
		public int MaxTime;

		private int FadeTime => 30;

		private float FadeRate => 1f / FadeTime;

		private float MinimumOpacity => 0.6f;

		public override bool UseAdditiveBlend => true;

		public GlowParticle(Vector2 position, Vector2 velocity, Color color, float scale, int maxTime)
		{
			Position = position;
			Velocity = velocity;
			glowColor = color;
			Scale = scale;
			MaxTime = maxTime;
		}

		public override void Update()
		{
			if (TimeActive >= MaxTime)
				opacity -= FadeRate;
			else if (TimeActive < FadeTime && opacity < MinimumOpacity)
				opacity += FadeRate;
			else
				opacity = MinimumOpacity + (float)Math.Sin((TimeActive - FadeTime) / 30f) * 0.3f;

			Color = glowColor * opacity;
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);
			Velocity = Velocity.RotatedByRandom(0.03f) * 0.99f;

			if (opacity <= 0f)
				Kill();
		}
	}
}
