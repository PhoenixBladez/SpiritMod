using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MeteorMagus_Circle : Particle
	{
		private readonly float _angle = 0; //Rotation of the mesh
		private float _rotation; //Rotation of the texture
		public readonly float _zRotation = 0; //Psuedo-3d effect rotation of mesh into z plane

		private float _opacity;
		private NPC _parent;
		private Vector2 _offset;

		private readonly int _maxTime = 0;
		private readonly int _fadeTime = 0;
		private readonly float _rotationVelocity = 0;

		public delegate void UpdateAction(Particle p);
		private UpdateAction _action = null;

		public MeteorMagus_Circle(NPC parent, Vector2 offset, Color color, float scale, int maxTime, int fadeTime, float zRotation, float angle, float rotationalVelocity, Vector2? velocity = null, UpdateAction action = null)
		{
			_parent = parent;
			_offset = offset;
			Position = _parent.Center + offset;
			Velocity = velocity.GetValueOrDefault();
			Color = color;
			Scale = scale;
			_maxTime = maxTime;
			_fadeTime = fadeTime;
			_zRotation = zRotation;
			_angle = angle;
			_rotationVelocity = rotationalVelocity;
			_action = action;
		}

		public override void Update()
		{
			if(!_parent.active || _parent.type != ModContent.NPCType<MeteorMagus_NPC>())
			{
				Kill();
				return;
			}

			_offset += Velocity;
			Position = _parent.Center + _offset;
			_rotation += _rotationVelocity;

			if (_action != null)
				_action.Invoke(this);

			int fadeOutStartTime = (_maxTime - _fadeTime);
			//Fade in after spawning
			if (TimeActive < _fadeTime)
				_opacity = TimeActive / (float)_fadeTime;

			//Fade out before dying
			else if (TimeActive > fadeOutStartTime)
				_opacity = 1 - (TimeActive - fadeOutStartTime) / (float)_fadeTime;

			//Otherwise full opacity
			else
				_opacity = 1;

			if (TimeActive > _maxTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override bool UseAdditiveBlend => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			//Draw a bloom underneath the circle
			Texture2D bloom = ModContent.Request<Texture2D>("Effects/Masks/CircleGradient");
			Vector2 drawCenter = Position - Main.screenPosition;
			Vector2 bloomScale = new Vector2(Scale, Scale * (1 - _zRotation));
			bloomScale *= 2.5f;
			Rectangle destRect = new Rectangle((int)(drawCenter.X - (bloomScale.X / 2)), (int)(drawCenter.Y - (bloomScale.Y / 2)), (int)bloomScale.X, (int)bloomScale.Y);
			spriteBatch.Draw(bloom, destRect, Color.Lerp(Color, Color.White, 0.25f) * _opacity * 0.4f);

			//Draw the circle itself
			Effect effect = ModContent.Request<Effect>("Effects/MagicCircle");
			effect.Parameters["rotation"].SetValue(_rotation);
			Color additiveColor = Color;
			additiveColor.A = 0;
			effect.Parameters["uTexture"].SetValue(ParticleHandler.GetTexture(Type));

			List<SquarePrimitive> primList = new List<SquarePrimitive>();

			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 6, 6, delegate (Vector2 posOffset, float bloomOpacity)
			{
				primList.Add(new SquarePrimitive
				{
					Color = additiveColor * _opacity * bloomOpacity * 0.25f,
					Height = Scale,
					Length = Scale * (1 - _zRotation),
					Position = Position - Main.screenPosition + posOffset,
					Rotation = _angle,
					ColorXCoordMod = 1 - _zRotation
				});
			});

			primList.Add(new SquarePrimitive
			{
				Color = additiveColor * _opacity,
				Height = Scale,
				Length = Scale * (1 - _zRotation),
				Position = Position - Main.screenPosition,
				Rotation = _angle,
				ColorXCoordMod = 1 - _zRotation
			});

			PrimitiveRenderer.DrawPrimitiveShapeBatched(primList.ToArray(), effect);

		}
	}
}
