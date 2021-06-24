using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace SpiritMod
{

    public static class StarDraw
    {
        //public delegate void ApplyParameters(Effect effect);
        public static void DrawStarBasic(BasicEffect effect, Vector2 position, float rotation, float scale, Color color)
        {
            if (Main.dedServ) return;
            VertexPositionColorTexture[] verticies = new VertexPositionColorTexture[30];

            CreateStarVerticies(ref verticies, position, rotation, scale, color);
            //PrepareBasicShader(ref effect);

            Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verticies, 0, 10);
        }

        // ------------------- helpers ------------------ \\
        private static void PrepareBasicShader(ref BasicEffect _basicEffect)
        {
            int width = Main.instance.GraphicsDevice.Viewport.Width;
            int height = Main.instance.GraphicsDevice.Viewport.Height;
            Vector2 zoom = Main.GameViewMatrix.Zoom;
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
            Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);
            _basicEffect.View = view;
            _basicEffect.Projection = projection;
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
        }

        private static void CreateStarVerticies(ref VertexPositionColorTexture[] verticies, Vector2 position, float rotation, float scale, Color color)
        {
            int currentIndex = 0;
            for (float i = rotation; i < 6.28f + rotation; i += 6.28f / 5f)
            {
                //[0,-0.85], [0.247, −0.080], [-0.247, −0.080]
                Vector2 pointPosition = new Vector2(0,-0.85f).RotatedBy(i);
                Vector2 uv = (pointPosition + new Vector2(1,1)) / 2f;
                pointPosition *= scale;
                AddVertex(ref verticies, ref currentIndex,pointPosition + position,color,uv);
                
                pointPosition = new Vector2(0.19f, -0.26f).RotatedBy(i);
                uv = (pointPosition + new Vector2(1,1)) / 2f;
                pointPosition *= scale;
                AddVertex(ref verticies, ref currentIndex,pointPosition + position,color,uv);

                pointPosition = new Vector2(-0.19f, -0.26f).RotatedBy(i);
                uv = (pointPosition + new Vector2(1,1)) / 2f;
                pointPosition *= scale;
                AddVertex(ref verticies, ref currentIndex, pointPosition + position,color,uv);

                 pointPosition = new Vector2(0.19f, -0.26f).RotatedBy(i);
                uv = (pointPosition + new Vector2(1,1)) / 2f;
                pointPosition *= scale;
                AddVertex(ref verticies, ref currentIndex,pointPosition + position,color,uv);

                 pointPosition = new Vector2(0f, 0f).RotatedBy(i);
                uv = (pointPosition + new Vector2(1,1)) / 2f;
                pointPosition *= scale;
                AddVertex(ref verticies, ref currentIndex,pointPosition + position,color,uv);

                 pointPosition = new Vector2(-0.19f, -0.26f).RotatedBy(i);
                uv = (pointPosition + new Vector2(1,1)) / 2f;
                pointPosition *= scale;
                AddVertex(ref verticies, ref currentIndex,pointPosition + position,color,uv);
            }
        }
        private static void AddVertex(ref VertexPositionColorTexture[] verticies, ref int currentIndex, Vector2 position, Color color, Vector2 uv)
        {
            if (currentIndex < verticies.Length)
                verticies[currentIndex++] = new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
        }
    }
}
