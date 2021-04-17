using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace SpiritMod.Prim
{
	public class ArclashPrimTrail : PrimTrail
	{
		private float _arcProgress;

		public ArclashPrimTrail(Projectile projectile)
		{
			_projectile = projectile;
			_entitytype = _projectile.type;
			_drawtype = PrimTrailManager.DrawProjectile;
		}

		public override void SetDefaults()
		{
			_alphaValue = 0.7f;
			_width = 4;
		}

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
			float colorSin = (float)Math.Sin(_counter / 3f);
			Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
			float widthVar = (float)Math.Sqrt(_points.Count) * _width;
			DrawBasicTrail(c1, widthVar);*/

			if (_noOfPoints <= 6 || _destroyed) 
				return;

			for (int i = 0; i < _points.Count; i++) {
				float widthVar;

				if (i == 0) {
					widthVar = _width;

					Color color = Color.White;

					Vector2 normalAhead = CurveNormal(_points, i + 1);
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

					AddVertex(_points[i], color * _alphaValue, new Vector2(0, 0.5f));
					AddVertex(secondUp, color * _alphaValue, new Vector2(1 / (float) _points.Count, 0));
					AddVertex(secondDown, color * _alphaValue, new Vector2(1 / (float) _points.Count, 1));
				}
				else {
					if (i == _points.Count - 1)
						continue;

					widthVar = _width;

					Color color = Color.Cyan;

					Vector2 normal = CurveNormal(_points, i);
					Vector2 normalAhead = CurveNormal(_points, i + 1);

					float j = (_points.Count + (float) Math.Sin(_counter / 10f) * 1 - i * 0.1f) / _points.Count;
					widthVar *= j;

					Vector2 firstUp = _points[i] - normal * widthVar;
					Vector2 firstDown = _points[i] + normal * widthVar;
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, color * _alphaValue, new Vector2(i / (float) _points.Count, 1));
					AddVertex(firstUp, color * _alphaValue, new Vector2(i / (float) _points.Count, 0));
					AddVertex(secondDown, color * _alphaValue, new Vector2((i + 1) / (float) _points.Count, 1));

					AddVertex(secondUp, color * _alphaValue, new Vector2((i + 1) / (float) _points.Count, 0));
					AddVertex(secondDown, color * _alphaValue, new Vector2((i + 1) / (float) _points.Count, 1));
					AddVertex(firstUp, color * _alphaValue, new Vector2(i / (float) _points.Count, 0));
				}
			}
		}

		public override void SetShaders() =>
			PrepareShader(SpiritMod.ArcLashShader, "MainPS", _arcProgress / 50, Color.Red);

		public override void OnUpdate()
		{
			_arcProgress = _projectile.ai[0];

			Vector2 direction = Vector2.UnitX * 8;
			direction = direction.RotatedBy(_projectile.ai[1]);

			Vector2 position = Main.player[_projectile.owner].MountedCenter + direction * (7 + _arcProgress / 30);
			Vector2 c1 = position + direction.RotatedBy(-0.3f - _arcProgress / 250f) * _arcProgress;
			Vector2 c2 = position + direction.RotatedBy(0.3f + _arcProgress / 250f) * _arcProgress;

			_counter++;
			_noOfPoints = _points.Count * 6;

			if (!_projectile.active && _projectile != null || _destroyed)
				OnDestroy();
			else {
				_points.Clear();

				foreach (Vector2 point in Helpers.GetBezier(position, position, c1, c2, _arcProgress + 6)) {
					Vector2 point2 = point;
					point2.X += Main.rand.Next(-2, 2);
					point2.Y += Main.rand.Next(-2, 2);
					_points.Add(point2);
				}
			}
		}

		public override void OnDestroy()
		{
			_destroyed = true;
			Dispose();
		}
	}
}