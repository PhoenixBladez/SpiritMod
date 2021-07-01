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

namespace SpiritMod.Items.Sets.GranitechSet.PulseOrbs
{
    class PulseOrbPrimTrail : PrimTrail
    {
        public PulseOrbPrimTrail(Projectile projectile, Projectile proj2)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;

			_projectile2 = proj2;
        }
        Projectile _projectile2;
        public override void SetDefaults()
        {
            Width= 1;
            AlphaValue= 0.7f;
            Cap = 100;
            Pixellated = false;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (PointCount <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/
            if (PointCount <= 6) return;
            float colorSin = (float)Math.Sin(Counter / 3f);
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == 0)
                {
                    Color c1 = Color.Lerp(Color.White, Color.Magenta, colorSin);
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * Width;
                    Vector2 secondDown = Points[i + 1] + normalAhead * Width;
                    AddVertex(Points[i], c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondDown, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        Color base1 = new Color(7, 86, 122);
                        Color base2 = new Color(255, 244, 173);
                        Color c = Color.Lerp(Color.White, Color.Magenta, colorSin);
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * Width;
                        Vector2 firstDown = Points[i] + normal * Width;
                        Vector2 secondUp = Points[i + 1] - normalAhead * Width;
                        Vector2 secondDown = Points[i + 1] + normalAhead * Width;

                        AddVertex(firstDown, c * AlphaValue, new Vector2((i / Cap), 1));
                        AddVertex(firstUp, c * AlphaValue, new Vector2((i / Cap), 0));
                        AddVertex(secondDown, c * AlphaValue, new Vector2((i + 1) / Cap, 1));

                        AddVertex(secondUp, c * AlphaValue, new Vector2((i + 1) / Cap, 0));
                        AddVertex(secondDown, c * AlphaValue, new Vector2((i + 1) / Cap, 1));
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
        int _seed = 503240;
        public override void OnUpdate()
        {
			Counter++;
            if (Counter % 2 == 0)
            {
                _seed = Main.rand.Next(100000);
            }
            var rnd = new Random(_seed);
            PointCount = Points.Count() * 6;
            if ((!Entity.active && Entity != null) || Destroyed || ((Entity as Projectile).modProjectile is PulseOrbProj modItem2 && !modItem2.channeling))
            {
                OnDestroy();
            }
            else
            {
                List<Vector2> points = new List<Vector2>();
                Vector2 dir = _projectile2.Center - Entity.Center;
                dir.Normalize();
                dir = dir.RotatedBy(1.57f);
                for (float i = 0; i < 1; i += 0.03f)
                {
                    Vector2 toAdd = Vector2.Lerp(_projectile2.Center, Entity.Center, i);
                    toAdd += dir * 4 * Main.rand.NextFloat(-1,1);
                    points.Add(toAdd);
                    if (Main.rand.Next(100) == 0)
                    {
                        Dust dust = Dust.NewDustPerfect(toAdd, 267, Vector2.Zero, 0, Color.Purple);
                        dust.noGravity = !dust.noGravity;
                    }

                }
                Points = points;
            }
        }
        public override void OnDestroy()
        {
            Destroyed = true;
            Width *= 0.75f;
            if (Width < 0.05f)
            {
                Dispose();
            }
        }
    }
}