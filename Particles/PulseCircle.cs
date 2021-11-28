using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;

namespace SpiritMod.Particles
{
	public class PulseCircle : Particle
	{
		private Color baseColor;

		private Color? _ringColor = null;
		/// <summary>
		/// The secondary brighter color of the circle, if not set to a value, uses an interpolation between the base color and white
		/// </summary>
		public Color? RingColor
		{
			get => _ringColor;
			set => _ringColor = value;
		}

		private float _angle = 0;
		/// <summary>
		/// The rotation along the 2D plane of the particle, defaults to 0
		/// </summary>
		public float Angle
		{
			get => _angle;
			set => _angle = value;
		}

		private float _zRotation = 0;
		/// <summary>
		/// The rotation into the 3D plane(makes the particle thin out and become darker the "further" it is from the camera for a pseudo 3D effect)<br />
		/// Goes from 0-1, 0 being default, and 1 being a 0 pixel thin line
		/// </summary>
		public float ZRotation
		{
			get => _zRotation;
			set => _zRotation = value;
		}

		private float _opacity;

		private int MaxTime;
		private float MaxRadius;
		private Entity entity;

		public enum MovementType
		{
			Outwards,
			Inwards,
			OutwardsQuadratic,
			InwardsQuadratic,
			OutwardsSquareRooted
		}

		private MovementType moveType;

		public PulseCircle(Vector2 position, Color color, float maxRadius, int maxTime, MovementType MovementStyle = MovementType.Outwards)
		{
			Position = position;
			baseColor = color;
			MaxRadius = maxRadius;
			MaxTime = maxTime;
			moveType = MovementStyle;
		}

		public PulseCircle(Entity attatchedEntity, Color color, float maxRadius, int maxTime, MovementType MovementStyle = MovementType.Outwards, Vector2? startingPosition = null)
		{
			entity = attatchedEntity;
			Position = entity.Center;
			_offset = startingPosition != null ? startingPosition.Value - entity.Center : Vector2.Zero;
			baseColor = color;
			MaxRadius = maxRadius;
			MaxTime = maxTime;
			moveType = MovementStyle;
		}

		private Vector2 _offset = Vector2.Zero;
		public override void Update()
		{
			if (entity != null)
			{
				if (!entity.active)
				{
					Kill();
					return;
				}
				_offset += Velocity;
				Position = entity.Center + _offset;
			}
			else
				Position += Velocity;

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
				case MovementType.OutwardsSquareRooted:
					Progress = (float)Math.Pow(TimeActive / (float)MaxTime, 0.5f);
					break;
				case MovementType.InwardsQuadratic:
					Progress = (float)Math.Pow(1 - (TimeActive / (float)MaxTime), 2);
					break;
			}
			Scale = MaxRadius * Progress;
			_opacity = Math.Min(2 * (1 - Progress), 1f);

			if (TimeActive > MaxTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Effect effect = SpiritMod.ShaderDict["PulseCircle"];
			Color rColor = _ringColor ?? Color.Lerp(baseColor, Color.White, 0.5f);
			effect.Parameters["BaseColor"].SetValue(baseColor.ToVector4());
			effect.Parameters["RingColor"].SetValue(rColor.ToVector4());
			var square = new Prim.SquarePrimitive
			{
				Color = Color.White * _opacity,
				Height = Scale,
				Length = Scale * (1 - ZRotation),
				Position = Position - Main.screenPosition,
				Rotation = Angle + MathHelper.Pi,
				ColorXCoordMod = 1 - ZRotation
			};
			Prim.PrimitiveRenderer.DrawPrimitiveShape(square, effect);
		}
	}
}
