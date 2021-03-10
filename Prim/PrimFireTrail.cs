using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Linq;
using System;

namespace SpiritMod.Prim
{
    class PrimFireTrail : PrimTrail
    {
		int widthcap;
        public PrimFireTrail(Entity entity, Color color, int width = 8, int cap = 20) : base()
        {
			if (entity is NPC) {
				_npc = (NPC)entity;
				_entitytype = _npc.type;
				_drawtype = PrimTrailManager.DrawNPC;
			}
			if(entity is Projectile) {
				_projectile = (Projectile)entity;
				_entitytype = _projectile.type;
				_drawtype = PrimTrailManager.DrawProjectile;
			} 
            _color = color;
            widthcap = width;
			_cap = cap;
        }
        public override void SetDefaults()
        {
            _alphaValue = 0.9f;
			_width = 0;
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
            for (int i = 1; i < _points.Count; i++)
            {
                if (i != _points.Count - 1)
                {
                    widthVar = _width * (((float)i + (_cap - _points.Count))/_cap);
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
			if (_drawtype == PrimTrailManager.DrawNPC) {
				_width = widthcap;
				if ((!_npc.active) || _destroyed || (_npc.type != _entitytype) || _npc.ai[2] == 0 || _npc.ai[0] == 0) {
					OnDestroy();
				}
				else {
					_points.Add(_npc.Center);
				}
			}
			if (_drawtype == PrimTrailManager.DrawProjectile) {
				if (_width < widthcap && !_destroyed)
					_width = MathHelper.Lerp(_width, widthcap, 0.08f);

				if ((!_projectile.active) || (_projectile.type != _entitytype)|| _destroyed) {
					OnDestroy();
				}
				else {
					_points.Add(_projectile.Center);
				}
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