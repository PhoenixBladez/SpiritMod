using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace SpiritMod.Mechanics.Trails
{
	public class Trail : BaseTrail
	{
		public float DissolveSpeed { get; set; }

		private readonly ITrailCap _trailCap;
		private readonly ITrailColor _trailColor;
		private readonly ITrailPosition _trailPosition;
		private readonly ITrailShader _trailShader;
		private float _widthStart;

		private float _currentLength;
		private float _maxLength;

		private readonly List<Vector2> _points;

		private readonly float _originalMaxLength;
		private readonly float _originalWidth;

		public Trail(Projectile projectile, ITrailColor type, ITrailCap cap, ITrailPosition position, ITrailShader shader, TrailLayer layer, float widthAtFront, float maxLength, float dissolveSpeed) : base(projectile, layer)
		{
			_trailCap = cap;
			_trailColor = type;
			_trailPosition = position;
			_trailShader = shader;
			_maxLength = maxLength;
			_widthStart = widthAtFront;
			DissolveSpeed = dissolveSpeed == -1 ? _maxLength / 10 : dissolveSpeed;
			_originalMaxLength = maxLength;
			_originalWidth = _widthStart;

			_points = new List<Vector2>();
		}

		public override void Dissolve()
		{
			_maxLength -= DissolveSpeed;
			_widthStart = _maxLength / _originalMaxLength * _originalWidth;
			if (_maxLength <= 0f)
			{
				Dead = true;
				return;
			}

			TrimToLength(_maxLength);
		}

		public override void Update()
		{
			Vector2 thisPoint = _trailPosition.GetNextTrailPosition(MyProjectile);

			if (_points.Count == 0)
			{
				_points.Add(thisPoint);
				return;
			}

			float distance = Vector2.Distance(thisPoint, _points[0]);
			_points.Insert(0, thisPoint);

			//If adding the next point is too much
			if (_currentLength + distance > _maxLength)
			{
				TrimToLength(_maxLength);
			}
			else
			{
				_currentLength += distance;
			}
		}

		private void TrimToLength(float length)
		{
			if (_points.Count == 0) return;

			_currentLength = length;

			int firstPointOver = -1;
			float newLength = 0;

			for (int i = 1; i < _points.Count; i++)
			{
				newLength += Vector2.Distance(_points[i], _points[i - 1]);
				if (newLength > length)
				{
					firstPointOver = i;
					break;
				}
			}

			if (firstPointOver == -1) return;

			//get new end point based on remaining distance
			float leftOverLength = newLength - length;
			Vector2 between = _points[firstPointOver] - _points[firstPointOver - 1];
			float newPointDistance = between.Length() - leftOverLength;
			between.Normalize();

			int toRemove = _points.Count - firstPointOver;
			_points.RemoveRange(firstPointOver, toRemove);

			_points.Add(_points.Last() + between * newPointDistance);
		}

		public override void Draw(Effect effect, GraphicsDevice device)
		{
			if (Dead || _points.Count <= 1) 
				return;

			//calculate trail's length
			float trailLength = 0f;
			for (int i = 1; i < _points.Count; i++)
			{
				trailLength += Vector2.Distance(_points[i - 1], _points[i]);
			}

			//Create vertice array, needs to be equal to the number of quads * 6 (each quad has two tris, which are 3 vertices)
			int currentIndex = 0;
			var vertices = new VertexPositionColorTexture[(_points.Count - 1) * 6 + _trailCap.ExtraTris * 3];

			//method to make it look less horrible
			void AddVertex(Vector2 position, Color color, Vector2 uv)
			{
				vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
			}

			float currentDistance = 0f;
			float halfWidth = _widthStart * 0.5f;

			Vector2 startNormal = CurveNormal(_points, 0);
			Vector2 prevClockwise = _points[0] + startNormal * halfWidth;
			Vector2 prevCClockwise = _points[0] - startNormal * halfWidth;

			Color previousColor = _trailColor.GetColourAt(0f, trailLength, _points);

			_trailCap.AddCap(vertices, ref currentIndex, previousColor, _points[0], startNormal, _widthStart);
			for (int i = 1; i < _points.Count; i++)
			{
				currentDistance += Vector2.Distance(_points[i - 1], _points[i]);

				float thisPointsWidth = halfWidth * (1f - i / (float)(_points.Count - 1));

				Vector2 normal = CurveNormal(_points, i);
				Vector2 clockwise = _points[i] + normal * thisPointsWidth;
				Vector2 cclockwise = _points[i] - normal * thisPointsWidth;
				Color color = _trailColor.GetColourAt(currentDistance, trailLength, _points);

				AddVertex(clockwise, color, Vector2.UnitX * i);
				AddVertex(prevClockwise, previousColor, Vector2.UnitX * (i - 1));
				AddVertex(prevCClockwise, previousColor, new Vector2(i - 1, 1f));

				AddVertex(clockwise, color, Vector2.UnitX * i);
				AddVertex(prevCClockwise, previousColor, new Vector2(i - 1, 1f));
				AddVertex(cclockwise, color, new Vector2(i, 1f));

				prevClockwise = clockwise;
				prevCClockwise = cclockwise;
				previousColor = color;
			}

			//set effect parameter for matrix (todo: try have this only calculated when screen size changes?)
			int width = device.Viewport.Width;
			int height = device.Viewport.Height;
			Vector2 zoom = Main.GameViewMatrix.Zoom;
			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
			var projection = Matrix.CreateOrthographic(width, height, 0, 1000);
			effect.Parameters["WorldViewProjection"].SetValue(view * projection);
			//effect.Parameters["WorldViewProjection"].SetValue(Main.GameViewMatrix.TransformationMatrix * Main.GameViewMatrix.ZoomMatrix);

			//apply this trail's shader pass and draw
			_trailShader.ApplyShader(effect, this, _points);
			device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, (_points.Count - 1) * 2 + _trailCap.ExtraTris);
		}

		//Helper methods
		private Vector2 CurveNormal(List<Vector2> points, int index)
		{
			if (points.Count == 1) return points[0];

			if (index == 0)
			{
				return Clockwise90(Vector2.Normalize(points[1] - points[0]));
			}
			if (index == points.Count - 1)
			{
				return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
			}
			return Clockwise90(Vector2.Normalize(points[index + 1] - points[index - 1]));
		}

		private Vector2 Clockwise90(Vector2 vector)
		{
			return new Vector2(-vector.Y, vector.X);
		}
	}
}