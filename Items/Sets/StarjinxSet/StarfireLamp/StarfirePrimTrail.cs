using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using System.Reflection;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    class StarfirePrimTrail : PrimTrail
    {
        public StarfirePrimTrail(Projectile projectile, Color color, int width = 8)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;

			Color = color;
			Width = width;
		}
        public override void SetDefaults()
        {
            AlphaValue = 0.9f;
            Cap = 20;
            Pixellated = true;
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
                if (i == 0)
                {
                    widthVar = Width;
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    AddVertex(Points[i], Color * AlphaValue, Vector2.Zero);
                    AddVertex(secondUp, Color * AlphaValue, Vector2.Zero);
                    AddVertex(secondDown, Color * AlphaValue, Vector2.Zero);
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        widthVar = Width;
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, Color * AlphaValue, new Vector2((i / ((float)Points.Count)), 1));
                        AddVertex(firstUp, Color * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
                        AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));

                        AddVertex(secondUp, Color * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 0));
                        AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));
                        AddVertex(firstUp, Color * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
                    }
                    else
                    {

                    }
                }
            }
        }
        public override void SetShaders()
        {
            PrepareShader(SpiritMod.StarfirePrims,"MainPS", Counter);
        }
        public override void OnUpdate()
        {
            Counter++;
            PointCount = Points.Count() * 6;
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
            Width *= 0.8f;
            Width += ((float)Math.Sin(Counter * 2) * 0.3f);
            if (Width < 0.05f)
            {
                Dispose();
            }
        }
    }
}