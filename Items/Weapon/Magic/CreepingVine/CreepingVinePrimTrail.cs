
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
				if (i != 0)
				{
					if (i != Points.Count - 1)
					{
						Vector2 dir = Points[i] - Points[i + 1];
						UVXNext += dir.Length() / (_length * 34);

						widthVar = Width;
						Vector2 normal = CurveNormal(Points, i);
						Vector2 normalAhead = CurveNormal(Points, i + 1);
						Vector2 firstUp = Points[i] - normal * widthVar;
						Vector2 firstDown = Points[i] + normal * widthVar;
						Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
						Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

						AddVertex(firstDown, Lighting.GetColor((int)firstDown.X / 16, (int)firstDown.Y / 16) * AlphaValue, new Vector2((float)UVX, 1));
						AddVertex(firstUp, Lighting.GetColor((int)firstUp.X / 16, (int)firstUp.Y / 16) * AlphaValue, new Vector2((float)UVX, 0));
						AddVertex(secondDown, Lighting.GetColor((int)secondDown.X / 16, (int)secondDown.Y / 16) * AlphaValue, new Vector2((float)UVXNext, 1));

						AddVertex(secondUp, Lighting.GetColor((int)secondUp.X / 16, (int)secondUp.Y / 16) * AlphaValue, new Vector2((float)UVXNext, 0));
						AddVertex(secondDown, Lighting.GetColor((int)secondDown.X / 16, (int)secondDown.Y / 16) * AlphaValue, new Vector2((float)UVXNext, 1));
						AddVertex(firstUp, Lighting.GetColor((int)firstUp.X / 16, (int)firstUp.Y / 16) * AlphaValue, new Vector2((float)UVX, 0));
						UVX = UVXNext;
					}
				}
			}
		}

		public override void SetShaders()
		{
			Effect effect = SpiritMod.ThyrsusShader;
			if (effect.HasParameter("vineTexture"))
				effect.Parameters["vineTexture"].SetValue(ModContent.Request<Texture2D>("Textures/CreepingVine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			PrepareShader(effect, "MainPS", (float)_length);
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
				Vector2 dir = Points[i] - Points[i + 1];
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