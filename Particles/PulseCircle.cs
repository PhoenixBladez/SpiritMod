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
		private Entity entity;

		public enum MovementType
		{
			Outwards,
			Inwards,
			OutwardsQuadratic,
			InwardsQuadratic
		}

		private MovementType moveType;

		public PulseCircle(Vector2 position, Color color, float maxRadius, int maxTime, MovementType MovementStyle = MovementType.Outwards)
		{
			Position = position;
			glowColor = color;
			MaxRadius = maxRadius;
			MaxTime = maxTime;
			moveType = MovementStyle;
		}

		public PulseCircle(Entity attatchedEntity, Color color, float maxRadius, int maxTime, MovementType MovementStyle = MovementType.Outwards)
		{
			entity = attatchedEntity;
			Position = entity.Center;
			glowColor = color;
			MaxRadius = maxRadius;
			MaxTime = maxTime;
			moveType = MovementStyle;
		}

		public override void Update()
		{

			if (entity != null)
			{
				if (!entity.active)
				{
					Kill();
					return;
				}
				Position = entity.Center;
			}


			float Progress = 1f;
			switch (moveType)
			{
				case MovementType.Outwards:
					Progress = (TimeActive / (float)MaxTime);
					break;
				case MovementType.Inwards:
					Progress = 1 - (TimeActive / (float)MaxTime);
					break;
				case MovementType.OutwardsQuadratic:
					Progress = (float)Math.Pow(TimeActive / (float)MaxTime, 2);
					break;
				case MovementType.InwardsQuadratic:
					Progress = (float)Math.Pow(1 - (TimeActive / (float)MaxTime), 2);
					break;
			}
			Scale = (MaxRadius / (float)ParticleHandler.GetTexture(Type).Width) * Progress;
			float Opacity = Math.Min(2 * (1 - Progress), 1f);
			Color = glowColor * Opacity;

			if (TimeActive > MaxTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch) => spriteBatch.Draw(ParticleHandler.GetTexture(Type), Position - Main.screenPosition, null, Color, 0, ParticleHandler.GetTexture(Type).Size() / 2, Scale, SpriteEffects.None, 0);
	}
}
