using Microsoft.Xna.Framework;
using System;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Particles
{
	public class ImpactLine : Particle
	{
		private readonly Entity _ent = null;

		private Color _color;
		private Vector2 _scaleMod;
		private Vector2 _offset;
		private int _timeLeft;

		public ImpactLine(Vector2 position, Vector2 velocity, Color color, Vector2 scale, int timeLeft, Entity attatchedEntity = null)
		{
			Position = position;
			Velocity = velocity;
			_color = color;
			_scaleMod = scale;
			_timeLeft = timeLeft;
			_ent = attatchedEntity;

			if(_ent != null)
				_offset = Position - _ent.Center;
		}

		public override void Update()
		{
			float opacity = (float)Math.Sin((TimeActive / (float)_timeLeft) * MathHelper.Pi);
			Color = _color * opacity;
			Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Position, Color.ToVector3() / 2f);
			if(_ent != null)
			{
				if (!_ent.active)
				{
					Kill();
					return;
				}
				Position = _ent.Center + _offset;
				_offset += Velocity;
			}

			if (TimeActive > _timeLeft)
				Kill();
		}

		public override bool UseCustomDraw => true;
		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			float progress = (float)Math.Sin((TimeActive / (float)_timeLeft) * MathHelper.Pi);
			Vector2 scale = new Vector2(0.5f, progress) * _scaleMod;
			Vector2 offset = Vector2.Zero;
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Vector2 origin = new Vector2(tex.Width / 2, tex.Height);

			if (TimeActive > _timeLeft / 2)
			{
				offset = Vector2.UnitX.RotatedBy(Rotation - MathHelper.PiOver2) * tex.Height * scale.Y;
				origin.Y = 0;
			}

			spriteBatch.Draw(tex, Position + offset - Main.screenPosition, null, Color * ((progress / 5) + 0.8f), Rotation, origin, scale, SpriteEffects.None, 0);
		}
	}
}
