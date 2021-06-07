
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
    class MoonLightningPrimTrail : PrimTrail
    {
		public MoonLightningPrimTrail(Projectile projectile)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Points.Add(projectile.Center - new Vector2(0, 1500));
			Points.Add(projectile.Center + new Vector2(0, 1500));
        }
        public override void SetDefaults()
        {
            Width= 2;
            AlphaValue= 1f;
            Cap = 2;
            Pixellated = false;
			Color = Color.White;
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
					Color c1 = Color;
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
						Color c = Color;
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
        public override void OnUpdate()
        {
            PointCount = Points.Count() * 6;
            if (!Entity.active || Destroyed || (Entity is Projectile proj && proj.timeLeft < 20))
            {
                OnDestroy();
            }
        }
        public override void OnDestroy()
        {
             Dispose();
        }
    }
}