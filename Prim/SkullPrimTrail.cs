using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using System.Reflection;

namespace SpiritMod.Prim
{
    class SkullPrimTrail : PrimTrail
    {
        public SkullPrimTrail(Projectile projectile, Color color, int width = 12, int cap = 20) : base()
        {
            _projectile = projectile;
			_entitytype = _projectile.type;
			_drawtype = PrimTrailManager.DrawProjectile;
			_color = color;
            _width = width;
            _cap = cap;
        }
        public override void SetDefaults()
        {
            _alphaValue = 1f;
        }
        public override void PrimStructure(SpriteBatch spriteBatch)
        {
            /*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
            float colorSin = (float)Math.Sin(_counter / 3f);
            Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
            float widthVar = (float)Math.Sqrt(_points.Count) * _width;
            DrawBasicTrail(c1, widthVar);*/

            if (_noOfPoints <= 6) return;
            float widthVar;
            for (int i = 0; i < _points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = _width;
                    Vector2 normalAhead = CurveNormal(_points, i + 1);
                    Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;
                    AddVertex(_points[i], _color * _alphaValue, Vector2.Zero);
                    AddVertex(secondUp, _color * _alphaValue, Vector2.Zero);
                    AddVertex(secondDown, _color * _alphaValue, Vector2.Zero);
                }
                else
                {
                    if (i != _points.Count - 1)
                    {
                        widthVar = _width;
                        Vector2 normal = CurveNormal(_points, i);
                        Vector2 normalAhead = CurveNormal(_points, i + 1);
                        Vector2 firstUp = _points[i] - normal * widthVar;
                        Vector2 firstDown = _points[i] + normal * widthVar;
                        Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

                        AddVertex(firstDown, _color * _alphaValue, new Vector2((i / ((float)_points.Count)), 1));
                        AddVertex(firstUp, _color * _alphaValue, new Vector2((i / ((float)_points.Count)), 0));
                        AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / ((float)_points.Count), 1));

                        AddVertex(secondUp, _color * _alphaValue, new Vector2((i + 1) / ((float)_points.Count), 0));
                        AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / ((float)_points.Count), 1));
                        AddVertex(firstUp, _color * _alphaValue, new Vector2((i / ((float)_points.Count)), 0));
                    }
                    else
                    {

                    }
                }
            }
        }
        public override void SetShaders()
        {
            PrepareShader(SpiritMod.ScreamingSkullTrail, "MainPS", _counter / 2);
        }
        public override void OnUpdate()
        {
            _counter++;
            _noOfPoints = _points.Count() * 6;
            if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
            if ((!_projectile.active && _projectile != null) || _destroyed)
            {
                OnDestroy();
            }
            else
            {
                _points.Add(_projectile.Center);
            }
        }
        public override void OnDestroy()
        {
            _destroyed = true;
            _width *= 0.8f;
            _width += ((float)Math.Sin(_counter * 2) * 0.3f);
            if (_width < 0.05f)
            {
                Dispose();
            }
        }
    }
}