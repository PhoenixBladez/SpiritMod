using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;

namespace SpiritMod.Prim
{
	public class RipperPrimTrail : PrimTrail
	{
		public RipperPrimTrail(Projectile projectile)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
		}

		public override void SetDefaults()
		{
			AlphaValue = 0.9f;
			Cap = 8;
			Color = Color.White;
			Width = 5;
			Pixellated = true;
		}

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
			float colorSin = (float)Math.Sin(_counter / 3f);
			Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
			float widthVar = (float)Math.Sqrt(_points.Count) * _width;
			DrawBasicTrail(c1, widthVar);*/

			if (PointCount <= 6)
				return;

			for (int i = 0; i < Points.Count; i++)
				// float widthVar;
				if (i == 0) {
					/*widthVar = _width;
					Vector2 normalAhead = CurveNormal(_points, i + 1);
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;
					//AddVertex(_points[i], _color * _alphaValue, Vector2.Zero);
					//AddVertex(secondUp, _color * _alphaValue, Vector2.Zero);
					//AddVertex(secondDown, _color * _alphaValue, Vector2.Zero);*/
				}
				else {
					if (i == Points.Count - 1)
						continue;

					float widthVar = Width;

					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, Color * AlphaValue, new Vector2(i / (float) Points.Count, 1));
					AddVertex(firstUp, Color * AlphaValue, new Vector2(i / (float) Points.Count, 0));
					AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / (float) Points.Count, 1));

					AddVertex(secondUp, Color * AlphaValue, new Vector2((i + 1) / (float) Points.Count, 0));
					AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / (float) Points.Count, 1));
					AddVertex(firstUp, Color * AlphaValue, new Vector2(i / (float) Points.Count, 0));
				}
		}

		public override void SetShaders() =>
			PrepareShader(SpiritMod.RipperSlugShader, "MainPS", Counter);

		public override void OnUpdate()
		{
			if (!(Entity is Projectile projectile))
				return;

			Counter++;
			PointCount = Points.Count * 6;

			if (Cap < PointCount / 6)
				Points.RemoveAt(0);

			if (!projectile.active || Destroyed)
				OnDestroy();
			else
				Points.Add(projectile.Center);
		}

		public override void OnDestroy()
		{
			Destroyed = true;

			if (Points.Count < 3)
				Dispose();
			else
				Points.RemoveAt(0);
		}
	}
}