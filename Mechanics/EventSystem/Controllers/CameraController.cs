using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using SpiritMod.Utilities;

using Terraria;

namespace SpiritMod.Mechanics.EventSystem.Controllers
{
	public class CameraController : EventController
	{
		private List<CameraPoint> _points;
		private int _current = 0;
		private CameraPoint CurrentPoint => _points[_current];
		private CameraPoint NextPoint => _points[_current + 1];
		private CameraPointData _nextData;
		private bool _hasCalculatedBefore = false;
		private bool _completed;
		private float _time;

		public CameraPointData CurrentData => _nextData;

		public CameraController(float start, IValueGrabber<CameraPointData> startPoint) : base(start)
		{
			_points = new List<CameraPoint>();
			AddPoint(StartTime, startPoint, EaseFunction.Linear);

			AmICompleted = () => _completed;
		}

		public CameraController AddPoint(float time, IValueGrabber<CameraPointData> point, EaseFunction easing)
		{
			_points.Add(new CameraPoint() { Time = time, Easing = easing, Data = point });
			_points = _points.OrderBy(c => c.Time).ToList();
			return this;
		}

		public CameraController RepeatPoint(float time)
		{
			_points.Add(new CameraPoint(_points.Last()) { Time = time });
			_points = _points.OrderBy(c => c.Time).ToList();
			return this;
		}

		public override void Update(float time)
		{
			_time = time;
		}

		public override void Reset()
		{
			base.Reset();

			_points.Clear();
			_current = 0;
		}

		public override void Activate()
		{
			base.Activate();
			EventPlayer.OnModifyScreenPosition += CutscenePlayer_OnModifyScreenPosition;
			SpiritMod.OnModifyTransformMatrix += SpiritMod_OnModifyTransformMatrix;
		}

		public override void Deactivate()
		{
			base.Deactivate();
			EventPlayer.OnModifyScreenPosition -= CutscenePlayer_OnModifyScreenPosition;
			SpiritMod.OnModifyTransformMatrix -= SpiritMod_OnModifyTransformMatrix;
		}

		private void SpiritMod_OnModifyTransformMatrix(Terraria.Graphics.SpriteViewMatrix obj)
		{
			if (!_hasCalculatedBefore) return;
			if (_nextData.Zoom.X < 0f || _nextData.Zoom.Y < 0f) return;

			Vector2 z = _nextData.Zoom;
			z.X = MathHelper.Clamp(z.X, 1f, 999f);
			z.Y = MathHelper.Clamp(z.Y, 1f, 999f);
			obj.Zoom = z;
		}

		private void CutscenePlayer_OnModifyScreenPosition()
		{
			if (_time > NextPoint.Time)
			{
				_current++;
				if (_current >= _points.Count - 1)
				{
					_completed = true;
					_current--;
				}
			}

			float between = NextPoint.Time - CurrentPoint.Time;
			float progress = _time - CurrentPoint.Time;
			float div = progress / between;
			if (div > 1f) div = 1f;
			float val = NextPoint.Easing.Ease(div);
			_hasCalculatedBefore = true;

			_nextData = CameraPointData.Lerp(CurrentPoint.Data.Method(), NextPoint.Data.Method(), val);

			Main.screenPosition = _nextData.Position - new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
			// TODO: Magically make the camera rotate? manually modify the existing thing? idk
		}

		public struct CameraPoint
		{
			public float Time { get; set; }
			public EaseFunction Easing { get; set; }
			public IValueGrabber<CameraPointData> Data { get; set; }
			public CameraPoint(CameraPoint point)
			{
				Time = point.Time;
				Easing = point.Easing;
				Data = point.Data;
			}
		}

		public struct CameraPointData
		{
			public Vector2 Position;
			//[Obsolete]
			//public float Rotation;
			public Vector2 Zoom;

			public CameraPointData(Vector2 vector)
			{
				Position = vector;
				//Rotation = 0f;
				Zoom = -Vector2.One;
			}

			public CameraPointData(Vector2 vector, float rot, float zoom)
			{
				Position = vector;
				//Rotation = rot;
				Zoom = new Vector2(zoom);
			}

			public CameraPointData(Vector2 vector, float rot, Vector2 zoom)
			{
				Position = vector;
				//Rotation = rot;
				Zoom = zoom;
			}

			public static CameraPointData Lerp(CameraPointData d1, CameraPointData d2, float amount)
			{
				return new CameraPointData(Vector2.Lerp(d1.Position, d2.Position, amount), MathHelper.Lerp(0f, 0f, amount), Vector2.Lerp(d1.Zoom, d2.Zoom, amount));
			}                                                                           // MathHelper.Lerp(d1.Rotation, d2.Rotation, amount)
		}

		#region Grabbers

		public interface IValueGrabber<T>
		{
			Func<T> Method { get; }
		}

		public class StaticPoint : IValueGrabber<CameraPointData>
		{
			private CameraPointData _data;

			public Func<CameraPointData> Method => () => _data;

			public StaticPoint(Vector2 position)
			{
				_data = new CameraPointData(position);
			}

			public StaticPoint(Vector2 position, float zoom)
			{
				_data = new CameraPointData(position, 0f, zoom);
			}

			public StaticPoint(CameraPointData data)
			{
				_data = data;
			}
		}

		public class EntityRelativePoint : IValueGrabber<CameraPointData>
		{
			private Entity _entity;
			private Vector2 _offset;
			private float _rotation;
			
			public Vector2 Zoom { get; set; }
			public bool UseEntityRotation { get; set; }

			public Func<CameraPointData> Method => () => new CameraPointData(_entity.Center + _offset, GetRotation(), Zoom);

			public EntityRelativePoint(Entity entity, Vector2 offset)
			{
				_offset = offset;
				_entity = entity;
				Zoom = Vector2.One;
			}

			public EntityRelativePoint(Entity entity, Vector2 offset, float staticRotation) : this(entity, offset)
			{
				_rotation = staticRotation;
			}

			private float GetRotation()
			{
				if (!UseEntityRotation) return _rotation;

				if (_entity is Player player)
				{
					return player.fullRotation;
				}

				if (_entity is Projectile projectile)
				{
					return projectile.rotation;
				}

				if (_entity is NPC npc)
				{
					return npc.rotation;
				}

				return _rotation;
			}
		}

		#endregion
	}
}
