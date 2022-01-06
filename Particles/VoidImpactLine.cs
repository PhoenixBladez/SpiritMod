using Microsoft.Xna.Framework;
using System;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Effects.Stargoop;

namespace SpiritMod.Particles
{
	public class VoidImpactLine : Particle, IMetaball
	{
		private readonly Entity _ent = null;

		private Color _color;
		private Vector2 _scaleMod;
		private Vector2 _offset;
		private int _timeLeft;

		public VoidImpactLine(Vector2 position, Vector2 velocity, Color color, Vector2 scale, int timeLeft, Entity attatchedEntity = null)
		{
			Position = position;
			Velocity = velocity;
			_color = color;
			_scaleMod = scale;
			_timeLeft = timeLeft;
			_ent = attatchedEntity;

			if(_ent != null)
				_offset = Position - _ent.Center;

			Attach(SpiritMod.Metaballs.NebulaLayer);
		}

		protected void Attach(StargoopLayer layer)
		{
			if (!layer.Metaballs.Contains(this))
				layer.Metaballs.Add(this);
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
			{
				SpiritMod.Metaballs.NebulaLayer.Metaballs.Remove(this);
				Kill();
			}
		}

		public override bool UseCustomDraw => true;


		public void DrawOnMetaballLayer(SpriteBatch spriteBatch)
		{
			float progress = (float)Math.Sin((TimeActive / (float)_timeLeft) * MathHelper.Pi);
			Vector2 scale = new Vector2(0.5f, progress) * _scaleMod;
			Vector2 offset = Vector2.Zero;
			Vector2 origin = new Vector2(ParticleHandler.GetTexture(Type).Width / 2, ParticleHandler.GetTexture(Type).Height);

			if (TimeActive > _timeLeft / 2)
			{
				offset = Vector2.UnitX.RotatedBy(Rotation - MathHelper.PiOver2) * ParticleHandler.GetTexture(Type).Height * scale.Y;
				origin.Y = 0;
			}

			spriteBatch.Draw(ParticleHandler.GetTexture(Type), (Position + offset - Main.screenPosition) / 2, null, Color.White, Rotation, origin, scale / 2, SpriteEffects.None, 0);
		}
		public override void CustomDraw(SpriteBatch spriteBatch)
		{

		}
	}
}
