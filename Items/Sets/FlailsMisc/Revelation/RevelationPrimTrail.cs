using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Linq;
using System;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.FlailsMisc.Revelation
{
    class RevelationPrimTrail : PrimTrail
    {
        public RevelationPrimTrail(Projectile projectile)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
        }
        public override void SetDefaults()
        {
			Width = 11;
			Color = Color.White;
            AlphaValue= 1f;
            Cap = 12;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (PointCount <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/

            if (PointCount <= 6) return;
            float widthVar;
            for (int i = 0; i < Points.Count; i++)
            {
                if (i != 0)
                {
                    if (i != Points.Count - 1)
                    {
                        widthVar = Width * (1 - ((float)(Points.Count - i) / (float)Points.Count));
						if (i == Points.Count - 2)
							widthVar = Width * 0.4f;
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

						AddVertex(firstDown, Color * AlphaValue, new Vector2(i / (float)Points.Count, 1));
						AddVertex(firstUp, Color * AlphaValue, new Vector2(i / (float)Points.Count, 0));
						AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / (float)Points.Count, 1));

						AddVertex(secondUp, Color * AlphaValue, new Vector2((i + 1) / (float)Points.Count, 0));
						AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / (float)Points.Count, 1));
						AddVertex(firstUp, Color * AlphaValue, new Vector2(i / (float)Points.Count, 0));
					}
                }
            }
        }
       public override void SetShaders()
        {
			Effect effect = SpiritMod.OutlinePrimShader;
			if (effect.HasParameter("outlineColor"))
				effect.Parameters["outlineColor"].SetValue(new Color(255, 0, 177).ToVector3());
			if (effect.HasParameter("baseColor"))
				effect.Parameters["baseColor"].SetValue(Color.White.ToVector3());
			PrepareShader(effect, "MainPS", Counter);
		}
        public override void OnUpdate()
        {
            Counter++;
            PointCount= Points.Count() * 6;
            if (Cap < PointCount / 6)
            {
                Points.RemoveAt(0);
            }
            if ((!Entity.active && Entity != null) || Destroyed)
            {
                OnDestroy();
            }
            else
            {
                Points.Add(Entity.Center);
            }
        }
        public override void OnDestroy()
        {
			Destroyed = true;
			if (Points.Count < 2)
				Dispose();
			else
				Points.RemoveAt(0);
		}
    }
}