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

namespace SpiritMod.Items.Sets.GreatswordSubclass
{
    class SolarSwordPrimTrail : PrimTrail
    {
        public SolarSwordPrimTrail(Projectile projectile)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
		}
        public override void SetDefaults()
        {
            AlphaValue = 0.7f;
            Width = 80;
            Cap = 300;
        }
        public int _direction = 1;
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
			var proj = Entity as Projectile;
			if (proj.ModProjectile is RagingSunbladeProj modProj)
			{
				Vector2 primCenter = modProj.primCenter;
				if (!proj.active)
					primCenter = Main.player[proj.owner].Center;
				for (int i = 0; i < Points.Count; i++)
				{
					if (i == 0)
					{
						widthVar = (float)Math.Sqrt(Points.Count) * Width;
						Color c1 = Color.Lerp(Color.White, SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 2), colorSin);
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
							widthVar = (float)Math.Sin(i * (3.14f / Points.Count)) * Width * (i) / 300f;
							float widthVar2 = (float)Math.Sin((i + 1) * (3.14f / Points.Count)) * Width * ((i + 1)) / 300f;
							Color c = Color.White * ((100 - dist) * 0.01f) * (Counter / 10f);
							Color CBT = Color.White * ((100 - dist) * 0.01f) * (Counter / 10f);
							Vector2 normal = CurveNormal(Points, i);
							Vector2 normalAhead = CurveNormal(Points, i + 1);
							Vector2 firstUp = Points[i] - normal * widthVar;
							Vector2 firstDown = Points[i] + normal * widthVar;
							Vector2 secondUp = Points[i + 1] - normalAhead * widthVar2;
							Vector2 secondDown = Points[i + 1] + normalAhead * widthVar2;
							if (_direction == 1)
							{
								AddVertex(firstDown + primCenter, c * AlphaValue, new Vector2((i / (float)Cap), 1));
								AddVertex(firstUp + primCenter, c * AlphaValue, new Vector2((i / (float)Cap), 0));
								AddVertex(secondDown + primCenter, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 1));

								AddVertex(secondUp + primCenter, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 0));
								AddVertex(secondDown + primCenter, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 1));
								AddVertex(firstUp + primCenter, c * AlphaValue, new Vector2((i / (float)Cap), 0));
							}
							else
							{
								AddVertex(firstDown + primCenter, c * AlphaValue, new Vector2((i / (float)Cap), 0));
								AddVertex(firstUp + primCenter, c * AlphaValue, new Vector2((i / (float)Cap), 1));
								AddVertex(secondDown + primCenter, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 0));

								AddVertex(secondUp + primCenter, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 1));
								AddVertex(secondDown + primCenter, CBT * AlphaValue, new Vector2((i + 1) / (float)Cap, 0));
								AddVertex(firstUp + primCenter, c * AlphaValue, new Vector2((i / (float)Cap), 1));
							}
						}
						else
						{

						}
					}
				}
			}
        }
        public override void SetShaders()
        {
			Effect effect = ModContent.Request<Effect>("Effects/HeliosPrims");
			effect.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<SpiritMod>().GetTexture("Textures/Trails/Trail_3"));
			PrepareShader(effect, "Edge2", Counter / 7.5f, new Color(233, 99, 28));
        }
        public override void OnUpdate()
        {
            Counter = 10;
            PointCount = Points.Count() * 6;
            while (Points.Count() > Cap)
            {
                Points.RemoveAt(0);
            }
            if (Destroyed || !Entity.active)
            {
                OnDestroy();
            }
        }
        public override void OnDestroy()
        {
            Destroyed = true;
            if (Points.Count() < 10)
            {
                 Dispose();
            }
            else
            {
                Points.RemoveAt(0);
                Points.RemoveAt(0);
                Points.RemoveAt(0);
                Points.RemoveAt(0);
                Points.RemoveAt(0);
                Points.RemoveAt(0);
            }
        }
    }
}