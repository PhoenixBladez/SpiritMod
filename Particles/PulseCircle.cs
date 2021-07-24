using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class PulseCircle : Particle
	{
		private Color glowColor;
		public int MaxTime;
		public float MaxRadius;

		public PulseCircle(Vector2 position, Color color, float maxRadius, int maxTime)
		{
			Position = position;
			glowColor = color;
			MaxRadius = maxRadius;
			MaxTime = maxTime;
		}

		public override void Update()
		{
			float Opacity = Math.Min(2 * (MaxTime - TimeActive) / (float)MaxTime, 1f);

			Color = glowColor * Opacity;

			Scale = (MaxRadius / (float)ParticleHandler.GetTexture(Type).Width) * (TimeActive / (float)MaxTime);

			if (TimeActive > MaxTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch) => spriteBatch.Draw(ParticleHandler.GetTexture(Type), Position - Main.screenPosition, null, Color, 0, ParticleHandler.GetTexture(Type).Size() / 2, Scale, SpriteEffects.None, 0);
	}
}
