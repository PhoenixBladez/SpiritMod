using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using System.Reflection;
using SpiritMod.NPCs.Hell;

namespace SpiritMod.Prim
{
    class PrimFireTrail : PrimTrail
    {
        public PrimFireTrail(NPC npc, Color color, int width = 8) : base()
        {
            _npc = npc;
            _color = color;
            _width = width;
			drawtype = PrimTrailManager.DrawNPC;
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.9f;
            _cap = 20;
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
            PrepareShader(SpiritMod.StarfirePrims,"NoiseTrail", _counter);
        }
        public override void OnUpdate()
        {
            _counter++;
            _noOfPoints = _points.Count() * 6;
			if (_cap < _noOfPoints / 6)
            {
                _points.RemoveAt(0);
            }
            if ((!_npc.active && _npc != null) || _destroyed || _npc.ai[2] == 0 || _npc.ai[0] == 0)
            {
                OnDestroy();
            }
            else
            {
                _points.Add(_npc.Center);
            }
        }
        public override void OnDestroy()
        {
            _destroyed = true;
            _width *= 0.8f;
            _width += ((float)Math.Sin(_counter * 2) * 0.3f);
			_cap--;
            if (_cap < 5)
            {
                Dispose();
            }
        }
    }
}