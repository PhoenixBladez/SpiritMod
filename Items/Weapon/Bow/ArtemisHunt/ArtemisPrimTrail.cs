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

namespace SpiritMod.Items.Weapon.Bow.ArtemisHunt
{
    class ArtemisPrimTrail : PrimTrail
    {
        public ArtemisPrimTrail(Projectile projectile, Vector2 start, Vector2 mid, Vector2 mid2, Vector2 end, bool reverse)
        {
            Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			for (float i = 0; i <= 1; i += 0.01f)
                Points.Add(Helpers.TraverseBezier(end, start, mid, mid2, i));
            _reverse = reverse;
        }
        bool _reverse;
        public override void SetDefaults()
        {
            AlphaValue = 1f;
            Width = 60;
            Cap = 100;
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
            float colorSin = (float)Math.Sin(Counter / 3f);
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = (float)Math.Sqrt(Points.Count) * Width;
                    Color c1 = Color.Lerp(Color.White, SpiritMod.StarjinxColor(Main.GlobalTime * 2), colorSin);
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    //AddVertex(Points[i], c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    // AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    //AddVertex(secondDown, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        float dist = Math.Abs((Points.Count - i) - Counter * 3);
                        widthVar = (float)Math.Sin(i * (3.14f / Points.Count)) * Width * (i) / 100f;
                        float widthVar2 = (float)Math.Sin((i + 1) * (3.14f / Points.Count)) * Width * ((i + 1)) / 100f;
                        Color c = Color.White * ((100 - dist) * 0.01f) * (Counter / 10f);
                        Color CBT = Color.White * ((100 - dist) * 0.01f) * (Counter / 10f);
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar2;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar2;
                        if (!_reverse)
                        {
                            AddVertex(firstDown, c * AlphaValue, new Vector2((i / (float)Cap), 0));
                            AddVertex(firstUp, c * AlphaValue, new Vector2((i / (float)Cap), 1));
                            AddVertex(secondDown, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 0));

                            AddVertex(secondUp, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 1));
                            AddVertex(secondDown, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 0));
                            AddVertex(firstUp, c * AlphaValue, new Vector2((i / (float)Cap), 1));
                        }
                        else
                        {
                            AddVertex(firstDown, c * AlphaValue, new Vector2((i / (float)Cap), 1));
                            AddVertex(firstUp, c * AlphaValue, new Vector2((i / (float)Cap), 0));
                            AddVertex(secondDown, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 1));

                            AddVertex(secondUp, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 0));
                            AddVertex(secondDown, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 1));
                            AddVertex(firstUp, c * AlphaValue, new Vector2((i / (float)Cap), 0));
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
        public override void SetShaders()
        {
            PrepareShader(SpiritMod.GSaber, "MainPS", Counter / 6f, Color.MediumSpringGreen);
        }
        public override void OnUpdate()
        {
            Counter += 2;
            PointCount = Points.Count() * 6;
            try
            {
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
                Points.RemoveAt(Points.Count() - 1);
            }
            catch (System.Exception)
            {

            }
            if (Destroyed || !Entity.active)
            {
                OnDestroy();
            }
        }
        public override void OnDestroy()
        {
            Destroyed = true;
            if (Points.Count() < 9)
            {
                 Dispose();
            }
        }
    }
}