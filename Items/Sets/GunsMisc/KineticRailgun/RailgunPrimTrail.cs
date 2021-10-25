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

namespace SpiritMod.Items.Sets.GunsMisc.KineticRailgun
{
    class RailgunPrimTrail : PrimTrail
    {
        public RailgunPrimTrail(Projectile projectile, NPC npc)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;

			_target = npc;
        }

        NPC _target;
        public override void SetDefaults()
        {
            Pixellated = true;
            Width = 50;
            Cap = 34;
        }
        float _bezierOffset = 0f;
        float _bezierAcc = 0f;
        float _bezierReset = 30;
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (Points <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(Counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(Points.Count) * Width;
            DrawBasicTrail(c1, widthVar);*/
            if (PointCount <= 6) return;
            float widthVar;
            for (int i = 0; i < Points.Count; i++)
            {
                widthVar = Width;
                if (i == 0)
                {
                    Color c1 = Color.White;
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    AddVertex(Points[i], c1 * AlphaValue, new Vector2(0, 0.5f));
                    AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 0));
					AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 1));
				}
                else
                {
                    if (i != Points.Count - 1)
                    {
                        Color c = Color.White;
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, c * AlphaValue, new Vector2((float)(i / (float)Cap), 1));
                        AddVertex(firstUp, c * AlphaValue, new Vector2((float)(i / (float)Cap), 0));
                        AddVertex(secondDown, c * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 1));

                        AddVertex(secondUp, c * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 0));
                        AddVertex(secondDown, c * AlphaValue, new Vector2((float)(i + 1) / (float)Cap, 1));
                        AddVertex(firstUp, c * AlphaValue, new Vector2((float)(i / (float)Cap), 0));
                    }
                    else
                    {

                    }
                }
            }
        }
       public override void SetShaders()
        {
			Effect effect = SpiritMod.TeslaShader;
				effect.Parameters["baseTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/GlowTrail"));
				effect.Parameters["pnoise"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/noise"));
				effect.Parameters["vnoise"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/vnoise"));
				effect.Parameters["wnoise"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/wnoise"));
			PrepareShader(effect, "MainPS", Counter * 0.1f);
		}
        int _seed = 503240;
        public override void OnUpdate()
        {
            _bezierOffset += _bezierAcc;
            Counter++;
            if (Counter % 2 == 0)
            {
                _seed = Main.rand.Next(100000);
            }
            if (Counter % _bezierReset == 1)
            {
                _bezierAcc = Main.rand.NextFloat(-0.015f,0.015f);
                _bezierOffset = Main.rand.NextFloat(-0.15f,0.15f);
                _bezierReset = Main.rand.Next(30,40);
            }
            var rnd = new Random(_seed);
            PointCount = Points.Count() * 6;
            
            if ((!Entity.active && Entity != null) || Destroyed || !_target.active || ((Entity as Projectile).modProjectile is KineticRailgunProj modItem2 && !modItem2.targets.Contains(_target)))
            {
                OnDestroy();
            }
            else
            {
                if ((Entity as Projectile).modProjectile is KineticRailgunProj modItem3)
                {
                    List<Vector2> points = new List<Vector2>();
                    Vector2 distance = _target.Center - Entity.position;
                    Vector2 dir = modItem3.direction;
                    dir.Normalize();
                    dir = dir.RotatedBy(_bezierOffset);
                    dir *= distance.Length();
                    for (float i = 0; i < 1; i += 0.03f)
                    {
                        
                        Vector2 toAdd = Helpers.TraverseBezier(_target.Center, Entity.position, Entity.position + dir, i);
                        Vector2 toNext = Helpers.TraverseBezier(_target.Center, Entity.position, Entity.position + dir, i + 0.03f) - toAdd;
                        toNext.Normalize();
                        //toAdd += toNext.RotatedBy(1.57f) * 4 * Main.rand.NextFloat(-1,1);
                        points.Add(toAdd);
                    }
                    Points = points;
                }
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