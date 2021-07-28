using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	public class RuneCircle
	{
		private readonly Texture2D _runeTex;
		private float _rotation;
		private readonly float _maxRadius;
		private readonly float _minRadius;
		private float _radius;
		private readonly float _speedCap;
		private readonly float _runeAmount;
		private float _opacity;
		public bool Dead { get; set; }

		private const float BaseRotationSpeed = MathHelper.Pi / 90;

		public RuneCircle(float MaxRadius, float MinRadius, float SpeedCap, float RuneAmount)
		{
			_runeTex = SpiritMod.instance.GetTexture("Effects/Runes");
			_maxRadius = MaxRadius;
			_minRadius = MinRadius;
			_radius = MaxRadius;
			_speedCap = SpeedCap;
			_runeAmount = Math.Max(RuneAmount, 1);
			_opacity = 0f;
		}

		public void Update(float Speed, int direction, bool fadeOut = false)
		{
			float UpdateRatio = Math.Min(Speed / _speedCap, 1); //cap the effect of the input, and make its effect a ratio relative to the cap
			_rotation += direction * BaseRotationSpeed * MathHelper.Lerp(1, 2, UpdateRatio); //rotate more with a higher speed
			_radius = MathHelper.Lerp(_radius, MathHelper.Lerp(_maxRadius, _minRadius, UpdateRatio), 0.08f); //reduce radius with a higher speed

			_opacity = fadeOut ? Math.Max(_opacity - 0.05f, 0) : Math.Min(_opacity + 0.05f, 1);
			Dead = fadeOut && (_opacity <= 0);
		}

		public void Draw(SpriteBatch sB, Vector2 center, Color color, float scale = 1f)
		{
			for(int i = 0; i < _runeAmount; i++)
			{
				Vector2 drawPosition = Vector2.UnitX.RotatedBy(_rotation + ((i / _runeAmount) * MathHelper.TwoPi)) * _radius;
				int frame = i % 4;
				Rectangle drawFrame = new Rectangle(0, frame * _runeTex.Height / 4, _runeTex.Width, _runeTex.Height / 4);
				float scaleMod = (float)(Math.Sin(i % 3) / 5f) + 0.8f;
				SpriteEffects effects = i % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				sB.Draw(_runeTex, center + drawPosition - Main.screenPosition, drawFrame, color * _opacity, 0, drawFrame.Size() / 2, scale * scaleMod, effects, 0);
			}
		}
	}
}