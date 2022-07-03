using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Linq;
using System;
using SpiritMod.Prim;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Sets.FlailsMisc.JetBrick
{
    class JetBrickPrimTrail : PrimTrail
    {
        public JetBrickPrimTrail(Projectile projectile)
        {
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
        }
        public override void SetDefaults()
        {
			Width = 16;
			Color = Color.White;
            AlphaValue= 1f;
			Cap = 15;
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
            for (int i = 0; i < Points.Count; i++)
            {
                if (i != 0)
                {
                    if (i != Points.Count - 1)
                    {
						widthVar = Width * (1 - ((float)(Points.Count - i) / (float)Points.Count));
                        Vector2 normal = CurveNormal(Points, i);
                        Vector2 normalAhead = CurveNormal(Points, i + 1);
                        Vector2 firstUp = Points[i] - normal * widthVar;
                        Vector2 firstDown = Points[i] + normal * widthVar;
                        Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
                        Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

						AddVertex(firstDown, Color * AlphaValue, new Vector2(i / (float)Points.Count, 1));
						AddVertex(firstUp, Color * AlphaValue, new Vector2(i / (float)Points.Count, 0));
						AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / (float)Points.Count, 1));

						AddVertex(secondUp, Color * AlphaValue, new Vector2((i + 1) / (float)Points.Count, 0));
						AddVertex(secondDown, Color * AlphaValue, new Vector2((i + 1) / (float)Points.Count, 1));
						AddVertex(firstUp, Color * AlphaValue, new Vector2(i / (float)Points.Count, 0));
					}
                }
            }
        }

		public float ColorCounter = 0;
       public override void SetShaders()
        {
			Effect effect = SpiritMod.JetbrickTrailShader;
			Color color = Color.Lerp(Color.Orange, Color.Red, Math.Min(ColorCounter / 120f, 1));

			if (effect.HasParameter("noise"))
				effect.Parameters["noise"].SetValue(ModContent.Request<Texture2D>("Textures/vnoise", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);

			if (effect.HasParameter("circle"))
				effect.Parameters["circle"].SetValue(ModContent.Request<Texture2D>("Effects/Masks/Extra_49", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			PrepareShader(effect, "MainPS", Counter / 30f, color);
		}
        public override void OnUpdate()
        {
			if (Entity is Projectile proj && proj.active)
				Counter = (int)proj.localAI[1];
			if (Entity is Projectile proj2 && proj2.active)
				ColorCounter = (int)proj2.localAI[0];
			PointCount = Points.Count() * 6;
            if (Cap < PointCount / 6)
            {
                Points.RemoveAt(0);
            }
            if ((!Entity.active && Entity != null) || Destroyed)
            {
                OnDestroy();
            }
            else
            {
                Points.Add(Entity.Center + (Entity.velocity * 2));
            }
        }
        public override void OnDestroy()
        {
			Destroyed = true;
			if (Points.Count < 2)
				Dispose();
			else
				Points.RemoveAt(0);
		}
    }
}