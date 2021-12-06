using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Prim
{
	public class SkullPrimTrail : PrimTrail
	{
		public SkullPrimTrail(Projectile projectile, Color color, int width = 12, int cap = 20)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color = color;
			Width = width;
			Cap = cap;
		}

		public override void SetDefaults() => AlphaValue = 1f;

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
			float colorSin = (float)Math.Sin(_counter / 3f);
			Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
			float widthVar = (float)Math.Sqrt(_points.Count) * _width;
			DrawBasicTrail(c1, widthVar);*/

			if (PointCount <= 6) 
				return;

			for (int i = 0; i < Points.Count; i++) {
				float widthVar;

				if (i == 0) {
					widthVar = Width;

					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(Points[i], Color * AlphaValue, new Vector2(0, 0.5f));
					AddVertex(secondUp, Color * AlphaValue * ((i + 1) / (float)Points.Count), new Vector2((float)(i + 1) / (float)Points.Count, 0));
					AddVertex(secondUp, Color * AlphaValue * ((i + 1) / (float)Points.Count), new Vector2((float)(i + 1) / (float)Points.Count, 1));
				}
				else {
					if (i == Points.Count - 1)
						continue;

					widthVar = Width;

					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, Color * AlphaValue * ((i) / (float)Points.Count), new Vector2(i / (float) Points.Count, 1));
					AddVertex(firstUp, Color * AlphaValue * ((i) / (float)Points.Count), new Vector2(i / (float) Points.Count, 0));
					AddVertex(secondDown, Color * AlphaValue * ((i + 1) / (float)Points.Count), new Vector2((i + 1) / (float) Points.Count, 1));

					AddVertex(secondUp, Color * AlphaValue * ((i + 1) / (float)Points.Count), new Vector2((i + 1) / (float) Points.Count, 0));
					AddVertex(secondDown, Color * AlphaValue * ((i + 1) / (float)Points.Count), new Vector2((i + 1) / (float) Points.Count, 1));
					AddVertex(firstUp, Color * AlphaValue * ((i) / (float)Points.Count), new Vector2(i / (float) Points.Count, 0));
				}
			}
		}

		public override void SetShaders()
		{
			Effect effect = SpiritMod.PrimitiveTextureMap;
			effect.Parameters["uTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/FlameTrail"));
			effect.Parameters["additive"].SetValue(true);
			effect.Parameters["repeats"].SetValue(1);
			effect.Parameters["intensify"].SetValue(true);
			effect.Parameters["scroll"].SetValue(Counter * 0.05f);
			PrepareShader(effect, "MainPS", Counter);
		}

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
			Width *= 0.8f;
			Width += (float) Math.Sin(Counter * 2) * 0.3f;

			if (Width < 0.05f)
				Dispose();
		}
	}
}