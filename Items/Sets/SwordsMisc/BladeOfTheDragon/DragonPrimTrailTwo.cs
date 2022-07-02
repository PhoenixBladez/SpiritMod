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

namespace SpiritMod.Items.Sets.SwordsMisc.BladeOfTheDragon
{
    class DragonPrimTrailTwo : PrimTrail
    {
		Vector2 start = Vector2.Zero;
		Vector2 end = Vector2.Zero;
		public DragonPrimTrailTwo(NPC npc)
		{
			Entity = npc;
			EntityType = npc.type;
			DrawType = PrimTrailManager.DrawProjectile;
			angle = Main.rand.NextFloat(6.28f);
			start = Entity.Center + (angle.ToRotationVector2() * Entity.height * 1.5f);
			end = Entity.Center + ((angle + 3.14f).ToRotationVector2() * Entity.height * 1.5f);
			for (float i = 0; i < 1; i += 0.025f)
				Points.Add(Vector2.Lerp(start, end, i));
		}

		public override void SetDefaults()
        {
            AlphaValue = 0.9f;
            Cap = 80;
            Width = 20;
        }
        float angle;
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
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == 0)
                {
                    widthVar = Width;
                    Color c1 = Color.LightGreen;
					Vector2 normalAhead = CurveNormal(Points, i + 1);
                    Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                    Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;
                    AddVertex(Points[i], c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondUp, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                    AddVertex(secondDown, c1 * AlphaValue, new Vector2((float)Math.Sin(Counter / 20f), (float)Math.Sin(Counter / 20f)));
                }
                else
                {
                    if (i != Points.Count - 1)
                    {
                        widthVar = Width;
                        Color c = Color.LightGreen;
						Color CBT = Color.LightGreen;
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
			Effect effect = SpiritMod.PrimitiveTextureMap;
			effect.Parameters["uTexture"].SetValue(ModContent.Request<Texture2D>("Textures/GlowTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			effect.Parameters["additive"].SetValue(true);
			effect.Parameters["intensify"].SetValue(true);
			PrepareShader(effect, "MainPS", Counter);
		}
		public override void OnUpdate()
        {
            Counter++;
            PointCount = Points.Count() * 6;
            OnDestroy();
        }
        public override void OnDestroy()
        {
            Destroyed = true;
            Width *= 0.85f;
            if (Width < 0.05f)
                Dispose();
        }
    }
}