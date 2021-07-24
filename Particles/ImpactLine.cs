using Microsoft.Xna.Framework;
using System;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Particles
{
	public class ImpactLine : Particle
	{
		private Color _color;
		public int _timeLeft;
		Vector2 _scaleMod;
		Entity _ent = null;
		Vector2 _offset;

		public ImpactLine(Vector2 position, Vector2 velocity, Color color, Vector2 scale, int timeLeft, Entity attatchedEntity = null)
		{
			Position = position;
			Velocity = velocity;
			_color = color;
			_scaleMod = scale;
			_timeLeft = timeLeft;
			_ent = attatchedEntity;
			if(_ent != null)
			{
				_offset = Position - _ent.Center;
			}
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

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Vector2 scale = new Vector2(0.5f, (float)Math.Sin((TimeActive / (float)_timeLeft) * MathHelper.Pi)) * _scaleMod;
			spriteBatch.Draw(ParticleHandler.GetTexture(Type), Position - Main.screenPosition, null, Color, Rotation, ParticleHandler.GetTexture(Type).Size() / 2, scale, SpriteEffects.None, 0);
		}
	}
}
