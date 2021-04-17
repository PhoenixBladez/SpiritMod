using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System;

namespace SpiritMod.Prim
{
	public class PrimFireTrail : PrimTrail
	{
		private readonly int _widthCap;

		public PrimFireTrail(Entity entity, Color color, int width = 8, int cap = 20)
		{
			switch (entity)
			{
				case NPC npc:
					NPC = npc;
					EntityType = NPC.type;
					DrawType = PrimTrailManager.DrawNPC;
					break;

				case Projectile projectile:
					Projectile = projectile;
					EntityType = Projectile.type;
					DrawType = PrimTrailManager.DrawProjectile;
					break;
			}

			Color = color;
			_widthCap = width;
			Cap = cap;
		}

		public override void SetDefaults()
		{
			AlphaValue = 0.9f;
			Width = 0;
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

			for (int i = 1; i < Points.Count; i++)
				if (i != Points.Count - 1) {
					float widthVar = Width * (((float) i + (Cap - Points.Count)) / Cap);

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
			PrepareShader(SpiritMod.StarfirePrims, "NoiseTrail", Counter);

		public override void OnUpdate()
		{
			Counter++;
			PointCount = Points.Count * 6;

			if (Cap < PointCount / 6)
				Points.RemoveAt(0);

			if (DrawType == PrimTrailManager.DrawNPC) {
				Width = _widthCap;

				if (!NPC.active || Destroyed || NPC.type != EntityType || NPC.ai[2] == 0 || NPC.ai[0] == 0)
					OnDestroy();
				else
					Points.Add(NPC.Center);
			}

			if (DrawType != PrimTrailManager.DrawProjectile)
				return;

			if (Width < _widthCap && !Destroyed)
				Width = MathHelper.Lerp(Width, _widthCap, 0.08f);

			if (!Projectile.active || Projectile.type != EntityType || Destroyed)
				OnDestroy();
			else
				Points.Add(Projectile.Center);
		}

		public override void OnDestroy()
		{
			Destroyed = true;
			Width *= 0.8f;
			Width += (float) Math.Sin(Counter * 2) * 0.3f;
			Cap--;

			if (Cap < 5)
				Dispose();
		}
	}
}