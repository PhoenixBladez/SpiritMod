using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	public class RuneCircle
	{
		private readonly Texture2D _runeTex;
		private float _rotation;
		private float _rotationSpeed;
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
			_runeTex = ModContent.Request<Texture2D>("SpiritMod/Textures/Runes", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			_maxRadius = MaxRadius;
			_minRadius = MinRadius;
			_radius = MaxRadius;
			_speedCap = SpeedCap;
			_runeAmount = Math.Max(RuneAmount, 1);
			_opacity = 0f;
		}

		public void Update(float Speed, int direction, float maxOpacity, bool fadeOut = false)
		{
			float UpdateRatio = Math.Min(Speed / _speedCap, 1); //cap the effect of the input, and make its effect a ratio relative to the cap
			_rotationSpeed = MathHelper.Lerp(_rotationSpeed, direction * BaseRotationSpeed * MathHelper.Lerp(1, 2, UpdateRatio), 0.1f); //rotate more with a higher speed
			_rotation += _rotationSpeed;
			_radius = MathHelper.Lerp(_radius, MathHelper.Lerp(_maxRadius, _minRadius, UpdateRatio), 0.08f); //reduce radius with a higher speed

			_opacity = fadeOut ? Math.Max(_opacity - 0.05f, 0) : Math.Min(_opacity + 0.05f, maxOpacity);

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

		public struct RuneDrawInfo
		{
			public Texture2D texture;
			public Color color;
			public float? scale;
			public Rectangle? drawFrame;
			public float rotation;
			public SpriteEffects? effects;

			public RuneDrawInfo(Texture2D Texture, Color Color, Rectangle? DrawFrame = null, SpriteEffects? Effects = null, float Rotation = 0, float? ScaleMod = null)
			{
				texture = Texture;
				color = Color;
				scale = ScaleMod;
				drawFrame = DrawFrame;
				effects = Effects;
				rotation = Rotation;
			}
		}

		public delegate RuneDrawInfo RuneDelegateDrawInfo(int runeNumber);

		public void DelegateDraw(SpriteBatch sB, Vector2 center, float baseScale, RuneDelegateDrawInfo drawInfo)
		{
			for (int i = 0; i < _runeAmount; i++)
			{
				RuneDrawInfo info = drawInfo.Invoke(i);
				Vector2 drawPosition = Vector2.UnitX.RotatedBy(_rotation + ((i / _runeAmount) * MathHelper.TwoPi)) * _radius;
				Rectangle drawFrame = info.drawFrame ?? new Rectangle(0, (i % 4) * _runeTex.Height / 4, _runeTex.Width, _runeTex.Height / 4);
				float scaleMod = info.scale ?? (float)(Math.Sin(i % 3) / 5f) + 0.8f;
				SpriteEffects effects = info.effects ?? (i % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				sB.Draw(info.texture, center + drawPosition - Main.screenPosition, drawFrame, info.color * _opacity, 0, drawFrame.Size() / 2, baseScale * scaleMod, effects, 0);
			}
		}
	}
}