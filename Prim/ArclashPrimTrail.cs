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
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
		}

		public override void SetDefaults()
		{
			AlphaValue = 0.7f;
			Width = 4;
		}

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
			float colorSin = (float)Math.Sin(_counter / 3f);
			Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
			float widthVar = (float)Math.Sqrt(_points.Count) * _width;
			DrawBasicTrail(c1, widthVar);*/

			if (PointCount <= 6 || Destroyed) 
				return;

			for (int i = 0; i < Points.Count; i++) {
				float widthVar;

				if (i == 0) {
					widthVar = Width;

					Color color = Color.White;

					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(Points[i], color * AlphaValue, new Vector2(0, 0.5f));
					AddVertex(secondUp, color * AlphaValue, new Vector2(1 / (float) Points.Count, 0));
					AddVertex(secondDown, color * AlphaValue, new Vector2(1 / (float) Points.Count, 1));
				}
				else {
					if (i == Points.Count - 1)
						continue;

					widthVar = Width;

					Color color = Color.Cyan;

					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);

					float j = (Points.Count + (float) Math.Sin(Counter / 10f) * 1 - i * 0.1f) / Points.Count;
					widthVar *= j;

					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, color * AlphaValue, new Vector2(i / (float) Points.Count, 1));
					AddVertex(firstUp, color * AlphaValue, new Vector2(i / (float) Points.Count, 0));
					AddVertex(secondDown, color * AlphaValue, new Vector2((i + 1) / (float) Points.Count, 1));

					AddVertex(secondUp, color * AlphaValue, new Vector2((i + 1) / (float) Points.Count, 0));
					AddVertex(secondDown, color * AlphaValue, new Vector2((i + 1) / (float) Points.Count, 1));
					AddVertex(firstUp, color * AlphaValue, new Vector2(i / (float) Points.Count, 0));
				}
			}
		}

		public override void SetShaders() =>
			PrepareShader(SpiritMod.ShaderDict["ArcLashShader"], "MainPS", _arcProgress / 50, Color.Red);

		public override void OnUpdate()
		{
			if (!(Entity is Projectile projectile))
				return;

			_arcProgress = projectile.ai[0];

			Vector2 direction = Vector2.UnitX * 8;
			direction = direction.RotatedBy(projectile.ai[1]);

			Vector2 position = Main.player[projectile.owner].MountedCenter + direction * (7 + _arcProgress / 30);
			Vector2 c1 = position + direction.RotatedBy(-0.3f - _arcProgress / 250f) * _arcProgress;
			Vector2 c2 = position + direction.RotatedBy(0.3f + _arcProgress / 250f) * _arcProgress;

			Counter++;
			PointCount = Points.Count * 6;

			if (!projectile.active || Destroyed)
				OnDestroy();
			else {
				Points.Clear();

				foreach (Vector2 point in Helpers.GetBezier(position, position, c1, c2, _arcProgress + 6)) {
					Vector2 point2 = point;
					point2.X += Main.rand.Next(-2, 2);
					point2.Y += Main.rand.Next(-2, 2);
					Points.Add(point2);
				}
			}
		}

		public override void OnDestroy()
		{
			Destroyed = true;
			Dispose();
		}
	}
}