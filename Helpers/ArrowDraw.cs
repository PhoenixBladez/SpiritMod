using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace SpiritMod
{
    public static class ArrowDraw
    {
        //public delegate void ApplyParameters(Effect effect);
        public static void DrawArrowBasic(BasicEffect effect, Vector2 position, float rotation, float length, float width, Color color, float headRotation, float headLength)
        {
            if (Main.dedServ) return;
            VertexPositionColorTexture[] verticies = new VertexPositionColorTexture[10];

             int currentIndex = 0;
            CreateRectangleVerticies(ref verticies, ref currentIndex, position, rotation, color, length, width);
			CreateTriangleVertices(ref verticies, ref currentIndex, position + (rotation.ToRotationVector2() * length), rotation, color, length, width);

			PrepareBasicShader(ref effect);

            Main.instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verticies, 0, 3);
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

        private static void CreateRectangleVerticies(ref VertexPositionColorTexture[] verticies, ref int currentIndex, Vector2 position, float rotation, Color color, float length, float width)
        {
            Vector2 direction = rotation.ToRotationVector2();
            width /= 2;
            Vector2 firstUp = position + (direction.RotatedBy(1.57f) * width);
            Vector2 firstDown = position - (direction.RotatedBy(1.57f) * width);

            Vector2 position2 = position + (length * direction);

            Vector2 secondUp = position2 + (direction.RotatedBy(1.57f) * width);
            Vector2 secondDown = position2 - (direction.RotatedBy(1.57f) * width);

            AddVertex(ref verticies, ref currentIndex, firstUp, color, new Vector2(0, 0));
            AddVertex(ref verticies, ref currentIndex, secondUp, color, new Vector2(1, 0));
            AddVertex(ref verticies, ref currentIndex, secondDown, color, new Vector2(1, 1));
            
            AddVertex(ref verticies, ref currentIndex, secondDown, color, new Vector2(1, 1));
            AddVertex(ref verticies, ref currentIndex, firstDown, color, new Vector2(0, 1));
            AddVertex(ref verticies, ref currentIndex, firstUp, color, new Vector2(0, 0));
        }

		private static void CreateTriangleVertices(ref VertexPositionColorTexture[] verticies, ref int currentIndex, Vector2 position, float rotation, Color color, float length, float width)
		{
			Vector2 direction = -rotation.ToRotationVector2();
			width /= 2;
			Vector2 firstUp = position + (direction.RotatedBy(1.57f) * width);
			Vector2 firstDown = position - (direction.RotatedBy(1.57f) * width);
			Vector2 tip = position - (direction * length / 3);

			AddVertex(ref verticies, ref currentIndex, tip, color, new Vector2(0.5f, 1));
			AddVertex(ref verticies, ref currentIndex, firstUp, color, new Vector2(0, 0));
			AddVertex(ref verticies, ref currentIndex, firstDown, color, new Vector2(1, 0));
		}

		private static void AddVertex(ref VertexPositionColorTexture[] verticies, ref int currentIndex, Vector2 position, Color color, Vector2 uv)
        {
            if (currentIndex < verticies.Length)
                verticies[currentIndex++] = new VertexPositionColorTexture(new Vector3(position - Main.screenPosition, 0f), color, uv);
        }
    }
}
