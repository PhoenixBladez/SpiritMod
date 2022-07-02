using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using System.Linq;
using System;
using static Terraria.ModLoader.ModContent;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.OlympiumSet.MarkOfZeus
{
    public class MarkOfZeusPrimTrailTwo : PrimTrail
    {
        public MarkOfZeusPrimTrailTwo(Projectile projectile, float width)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;

			Width= width;
        }
        public override void SetDefaults()
        {
            AlphaValue= 0.7f;
            Cap = 80;
        }
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
                    widthVar = (float)Math.Sqrt(Points.Count) * Width;
                    Color c1 = Color.Lerp(Color.White, Color.Gold, colorSin);
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
                        widthVar = (float)Math.Sqrt(Points.Count - i) * Width;
                        Color base1 = new Color(7, 86, 122);
                        Color base2 = new Color(255, 244, 173);
                        Color c = Color.Lerp(Color.White, Color.Gold, colorSin);
                        Color CBT = Color.Lerp(Color.White, Color.Gold, colorSin);
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        float j = (Cap + ((float)(Math.Sin(Counter / 10f)) * 1) - i * 0.1f) / Cap;
                        widthVar *= j;
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
            PointCount= Points.Count() * 6;
            if (Cap < PointCount / 6)
            {
                Points.RemoveAt(0);
            }
            if ((!Entity.active && Entity != null) || Destroyed)
            {
                OnDestroy();
            }
        }
		public void AddPoints()
		{
			switch (Counter % 3)
			{
				case 0:
					Points.Add(Entity.Center + (Main.rand.NextFloat(0.5f, 1) * (Entity.velocity.RotatedBy(1.57f)) / 2));
					break;
				case 1:
					Points.Add(Entity.Center);
					break;
				case 2:
					Points.Add(Entity.Center + (Main.rand.NextFloat(0.5f, 1) * (Entity.velocity.RotatedBy(-1.57f)) / 2));
					break;
			}
		}
		public override void OnDestroy()
        {
			Destroyed = true;
            Width *= 0.85f;
            if (Width < 0.05f)
            {
                Dispose();
            }
        }
    }
}