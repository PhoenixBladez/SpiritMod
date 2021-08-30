using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Linq;
using System;
using SpiritMod.Effects.Stargoop;
using SpiritMod.Prim;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
    class RealityQuillPrimTrail : PrimTrail, IGalaxySprite
    {
        public RealityQuillPrimTrail(Projectile projectile)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
		}

        public override void SetDefaults()
        {
            Width = 14;
            AlphaValue = 1f;
            Cap= 100;
            Pixellated = false;
            Disabled = true;
            Color = new Color(0,255,0);

            SpiritMod.Metaballs.NebulaLayer.Sprites.Add(this);
        }

        public bool _addPoints = true;
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (PointCount <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/

            if (PointCount <= 6) return;
            for (int i = 0; i < Points.Count; i++)
            {
                float widthVar = Width - ((Math.Abs(i - (Points.Count / 2f)) / (float)Points.Count) * Width * 2);
                if (i == 0)
                {
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    AddVertex(Points[i], Color * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondUp, Color * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondDown, Color * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, Color * AlphaValue, new Vector2((i / Cap), 1));
                        AddVertex(firstUp, Color * AlphaValue, new Vector2((i / Cap), 0));
                        AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / Cap, 1));

                        AddVertex(secondUp, Color * AlphaValue, new Vector2((i + 1) / Cap, 0));
                        AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / Cap, 1));
                        AddVertex(firstUp, Color * AlphaValue, new Vector2((i / Cap), 0));
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
            else if (_addPoints)
            {
                Points.Add(Entity.Center + new Vector2(Main.rand.Next(-20,20), Main.rand.Next(-20,20)));
            }
        }
        public override void OnDestroy()
        {
			Destroyed = true;
            Width *= 0.75f;
            if (Width < 1)
            {
                Dispose();
                SpiritMod.Metaballs.NebulaLayer.Sprites.Remove(this);
            }
        }

		public void DrawGalaxyMappedSprite(SpriteBatch sB)
		{
            Draw();
		}
	}
}