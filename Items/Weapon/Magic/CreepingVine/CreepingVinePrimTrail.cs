
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using SpiritMod.Prim;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using System.Reflection;

namespace SpiritMod.Items.Weapon.Magic.CreepingVine
{
    public class CreepingVinePrimTrail : PrimTrail
    {
        public CreepingVinePrimTrail(Projectile projectile)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
		}
        public override void SetDefaults()
        {
            Width = 9;
            AlphaValue = 1f;
            Cap = 100;
            Color = Color.White;
        }
        public bool _addPoints = true;
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
			/*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(_counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(_points.Count) * _width;
            DrawBasicTrail(c1, widthVar);*/

			if (PointCount <= 6) return;
			float widthVar;
            double UVX = 0;
            double UVXNext = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = Width;
                    Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    //AddVertex(_points[i], _color * _alphaValue, Vector2.Zero);
                    //AddVertex(secondUp, _color * _alphaValue, Vector2.Zero);
                    //AddVertex(secondDown, _color * _alphaValue, Vector2.Zero);
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        Vector2 dir = Points[i] - Points[i + 1];
                        UVXNext+= dir.Length() / (_length * 34);
                        
                        widthVar = Width;
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, Color * AlphaValue, new Vector2((float)UVX, 1));
                        AddVertex(firstUp, Color * AlphaValue, new Vector2((float)UVX, 0));
                        AddVertex(secondDown, Color * AlphaValue, new Vector2((float)UVXNext, 1));

                        AddVertex(secondUp, Color * AlphaValue, new Vector2((float)UVXNext, 0));
                        AddVertex(secondDown, Color * AlphaValue, new Vector2((float)UVXNext, 1));
                        AddVertex(firstUp, Color * AlphaValue, new Vector2((float)UVX, 0));
                        UVX = UVXNext;
                    }
                    else
                    {

                    }
                }
            }
        }
       public override void SetShaders()
        {
			Effect effect = SpiritMod.ThyrsusShader;
			if (effect.HasParameter("vineTexture"))
			{
				effect.Parameters["vineTexture"].SetValue(GetInstance<SpiritMod>().GetTexture("Textures/CreepingVine"));
			}
			PrepareShader(effect,"MainPS", (float)_length);
        }
        double _length = 1;
        public override void OnUpdate()
        {
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
                Points.Add(Entity.Center);
            }
            _length = 0;
            for (int i = 0; i < Points.Count() - 2; i++)
            {
                Vector2 dir = Points[i] - Points[i+1];
                _length += dir.Length();
            }
            _length /= 34.0;
        }
        public override void OnDestroy()
        {
            Destroyed = true;
            AlphaValue -= 0.1f;
            if (AlphaValue <= 0.05f)
                Dispose();
        }
	}
}