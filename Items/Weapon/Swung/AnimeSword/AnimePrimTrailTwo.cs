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

namespace SpiritMod.Items.Weapon.Swung.AnimeSword
{
    class AnimePrimTrailTwo : PrimTrail
    {
		public AnimePrimTrailTwo(NPC npc)
		{
			Entity = npc;
			EntityType = npc.type;
			DrawType = PrimTrailManager.DrawProjectile;
			angle = Main.rand.NextFloat(6.28f);
			Points.Add(Entity.Center + (angle.ToRotationVector2() * Entity.height * 1.5f));
		}

		public override void SetDefaults()
        {
            AlphaValue = 0.9f;
            Cap = 80;
            Width = 5;
            Pixellated = true;
        }
        float angle;
        NPC _npc;
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (PointCount <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/

            if (PointCount <= 6) return;
            float widthVar;
            float colorSin = (float)Math.Sin(Counter / 3f);
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = Width;
                    Color c1 = Color.Lerp(Color.White, Color.White, colorSin);
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    AddVertex(Points[i], c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondDown, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        widthVar = Width;
                        Color c = Color.Lerp(Color.White, Color.White, colorSin);
                        Color CBT = Color.Lerp(Color.White, Color.White, colorSin);
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, c * AlphaValue, new Vector2((i / Cap), 1));
                        AddVertex(firstUp, c * AlphaValue, new Vector2((i / Cap), 0));
                        AddVertex(secondDown, CBT * AlphaValue, new Vector2((i + 1) / Cap, 1));

                        AddVertex(secondUp, CBT * AlphaValue, new Vector2((i + 1) / Cap, 0));
                        AddVertex(secondDown, CBT * AlphaValue, new Vector2((i + 1) / Cap, 1));
                        AddVertex(firstUp, c * AlphaValue, new Vector2((i / Cap), 0));
                    }
                    else
                    {

                    }
                }
            }
        }
       public override void SetShaders()
        {
            PrepareBasicShader();
        }
        public override void OnUpdate()
        {
            if (Counter == 1)
            {
                 Points.Add(Entity.Center + ((angle + 3.14f).ToRotationVector2() * Entity.height * 1.5f));
            }
            Counter++;
            PointCount = Points.Count() * 6;
            OnDestroy();
        }
        public override void OnDestroy()
        {
            Destroyed = true;
            Width *= 0.85f;
            if (Width < 0.05f)
            {
                Dispose();
            }
        }
    }
}