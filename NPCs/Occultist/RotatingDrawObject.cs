using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
namespace SpiritMod.NPCs.Occultist
{
	public class RotatingObjectManager
	{
		private List<RotatingDrawObject> _rotatingDrawObjects = new List<RotatingDrawObject>();

		public void AddObject(Texture2D Texture, float YPos, float Radius, float Scale = 1f, Color? Color = null, int MaxTime = -1, Rectangle? DrawFrame = null, float Period = 30, float StartingOffset = 0, float Rotation = 0) => _rotatingDrawObjects.Add(new RotatingDrawObject(Texture, YPos, Radius, Scale, Color, MaxTime, DrawFrame, Period, StartingOffset, Rotation));
	
		public void UpdateObjects()
		{
			for (int i = 0; i < _rotatingDrawObjects.Count; i++)
			{
				_rotatingDrawObjects[i].Update();
				if (_rotatingDrawObjects[i].Dead)
					_rotatingDrawObjects.Remove(_rotatingDrawObjects[i]);
			}
		}

		public void KillAllObjects()
		{
			foreach (RotatingDrawObject r in _rotatingDrawObjects)
				r.BeginFadeout();
		}

		public void DrawBack(SpriteBatch sB, Vector2 pos)
		{
			foreach (RotatingDrawObject r in _rotatingDrawObjects.Where(x => x.DrawBehind))
				r.Draw(sB, pos);
		}

		public void DrawFront(SpriteBatch sB, Vector2 pos)
		{
			foreach (RotatingDrawObject r in _rotatingDrawObjects.Where(x => !x.DrawBehind))
				r.Draw(sB, pos);
		}
	}

	internal class RotatingDrawObject
	{
		private readonly Texture2D _texture;
		public Vector3 Position;
		private Rectangle? _frame;
		private readonly float _radius;
		private readonly float _period;
		private float _counter;
		private float _opacity;
		private float _scale;
		private float _rotation;
		private Color? _color;
		private bool _dying;
		private int _timeLeft;

		public bool DrawBehind => Position.Z < 0;
		public bool Dead => _dying && _opacity == 0;

		public RotatingDrawObject(Texture2D Texture, float YPos, float Radius, float Scale, Color? Color, int MaxTime, Rectangle? DrawFrame, float Period, float StartingOffset, float Rotation)
		{
			_texture = Texture;
			_opacity = 0f;
			_radius = Radius;
			_frame = DrawFrame;
			_period = Period;
			_timeLeft = MaxTime;
			_counter = StartingOffset;
			_scale = Scale;
			_color = Color;
			_rotation = Rotation;
			Position.Y = YPos;
			GetPosition();
		}

		public void Update()
		{
			_counter++;
			if (_timeLeft == 0)
				_dying = true;

			if (_dying)
				_opacity = Math.Max(_opacity - 0.075f, 0);
			else
				_opacity = Math.Min(_opacity + 0.075f, 1);

			--_timeLeft;
			GetPosition();
		}

		public void BeginFadeout() => _dying = true;

		public void GetPosition()
		{
			Position.Z = (float)Math.Sin((_counter / _period) * MathHelper.Pi);
			Position.X = (float)Math.Cos((_counter / _period) * MathHelper.Pi) * _radius;
		}

		public void Draw(SpriteBatch sB, Vector2 pos)
		{
			Vector2 scale = (1f + (Position.Z / 4f)) * new Vector2(Math.Abs(Position.Z), 1);
			Color colorMod = (_color ?? Color.White) * _opacity;
			colorMod *= (Position.Z / 4f + 0.75f);

			sB.Draw(_texture, pos + new Vector2(Position.X, Position.Y) - Main.screenPosition, _frame, colorMod, _rotation, (_frame != null) ? _frame.Value.Size() / 2 : _texture.Size() / 2, scale * _scale, SpriteEffects.None, 0);
		}
	}
}