using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Linq;
using System;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
    class ThrustVisual : PrimTrail
    {
		private Player _owner;
		private float _width = 0;
		private readonly float _maxWidth;
		private readonly Vector2 _direction;
		private readonly Color _color;
		private float _length = 0;
		private readonly float _maxLength;
		private readonly int _maxTime;
		private float _opacity = 0;
        public ThrustVisual(Player owner, Vector2 direction, Color color, float width, float length, int maxTime)
        {
			_owner = owner;
			_direction = direction;
			_maxWidth = width;
			_color = color;
			_maxLength = length;
			_maxTime = maxTime;
			PointCount = 6;
			DrawType = PrimTrailManager.DrawProjectile;
        }

        public override void PrimStructure(SpriteBatch spriteBatch)
        {
			Vector2 tip = _owner.MountedCenter + (_direction * (Destroyed ? _maxLength : _length));
			Vector2 leftbase = _owner.MountedCenter + (_direction.RotatedBy(3f * MathHelper.PiOver2) * _width) + (Destroyed ? (_direction * (_maxLength - _length)) : Vector2.Zero);
			Vector2 rightbase = _owner.MountedCenter + (_direction.RotatedBy(MathHelper.PiOver2) * _width) + (Destroyed ? (_direction * (_maxLength - _length)) : Vector2.Zero);

			AddVertex(leftbase, _color * (float)Math.Pow(_opacity, 2), new Vector2(0f, 0));
			AddVertex(tip, _color * (float)Math.Pow(_opacity, 2), new Vector2(0.5f, 1f));
			AddVertex(rightbase, _color * (float)Math.Pow(_opacity, 2), new Vector2(1f, 0f));
		}

        public override void SetShaders()
        {
			PrepareShader(SpiritMod.ShaderDict["SlashEffectShaders"], "ThrustPS", Counter / 10f);
		}

        public override void OnUpdate()
        {
			if (_owner.dead || !_owner.active || _owner.frozen)
				Dispose();

            if (++Counter > _maxTime)
                OnDestroy();

			else
			{
				//make width and length rapidly increase, up until they reach their maximum, and fade in
				_width = Math.Min(_width + ((1.5f * _maxWidth) / _maxTime), _maxWidth);
				_length = Math.Min(_length + ((1.5f * _maxLength) / _maxTime), _maxLength);
				_opacity = Math.Min(_opacity + (1f / _maxTime), 1);
			}

        }

        public override void OnDestroy()
        {
			//reduce width and length, and make it fade out
            Destroyed = true;
			_opacity -= 0.6f / _maxTime;
			_width -= 0.6f * _maxWidth / _maxTime;
			_length -= 0.6f * _maxLength / _maxTime;
            if (_opacity <= 0)
                 Dispose();
        }
    }
}