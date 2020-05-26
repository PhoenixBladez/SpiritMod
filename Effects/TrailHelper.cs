using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace SpiritMod.Effects
{
    public static class TrailHelper
    {
        private static Mod _mod;
        private static BasicEffect _effect;

        public static void Load(GraphicsDevice device, Mod mod)
        {
            _mod = mod;
            _effect = new BasicEffect(device);
        }

        public static void Unload()
        {
            _mod = null;
            _effect = null;
        }

        public static void DrawTrail(SpriteBatch spriteBatch, Vector2 screenPos, List<Vector2> points, Color color, float maxWidth)
        {
            GraphicsDevice device = spriteBatch.GraphicsDevice;

            if (points.Count <= 1) return;

            int currentIndex = 0;
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[(points.Count - 1) * 6];

            void AddVertex(Vector2 position, Vector2 uv)
            {
                float hue = (Main.GlobalTime * 5f + uv.X * 0.3f) % MathHelper.TwoPi;
                vertices[currentIndex++] = new VertexPositionColorTexture(new Vector3(position - screenPos, 0f), color, uv);
            }

            float halfWidth = maxWidth * 0.5f;

            Vector2 startNormal = CurveNormal(points, 0);
            Vector2 prevClockwise = points[0] + startNormal * halfWidth;
            Vector2 prevCClockwise = points[0] - startNormal * halfWidth;

            for (int i = 1; i < points.Count; i++)
            {
                float width = halfWidth * (1f - (i / (float)(points.Count - 1)));

                Vector2 normal = CurveNormal(points, i);
                Vector2 clockwise = points[i] + normal * width;
                Vector2 cclockwise = points[i] - normal * width;

                AddVertex(clockwise, Vector2.UnitX * i);
                AddVertex(prevClockwise, Vector2.UnitX * (i - 1));
                AddVertex(prevCClockwise, new Vector2(i - 1, 1f));

                AddVertex(clockwise, Vector2.UnitX * i);
                AddVertex(prevCClockwise, new Vector2(i - 1, 1f));
                AddVertex(cclockwise, new Vector2(i, 1f));

                prevClockwise = clockwise;
                prevCClockwise = cclockwise;
            }

            _effect.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(Main.screenWidth / 2, Main.screenHeight / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi);
            _effect.Projection = Matrix.CreateOrthographic(Main.screenWidth, Main.screenHeight, 0, 100);
            _effect.VertexColorEnabled = true;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, (points.Count - 1) * 2);
            }
        }

        private static Vector2 CurveNormal(List<Vector2> points, int index)
        {
            if (points.Count == 1) return points[0];

            if (index == 0)
            {
                return Clockwise90(Vector2.Normalize(points[1] - points[0]));
            }
            if (index == points.Count - 1)
            {
                return Clockwise90(Vector2.Normalize(points[index] - points[index - 1]));
            }
            return Clockwise90(Vector2.Normalize(points[index + 1] - points[index - 1]));
        }

        public static Color ColorFromHSL(float h, float s, float l)
        {
            h /= MathHelper.TwoPi;

            float r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    float temp2;
                    if (l < 0.5f)
                        temp2 = l * (1f + s);
                    else
                        temp2 = l + s - (l * s);

                    float temp1 = 2f * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 0.33333333f);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 0.33333333f);
                }
            }
            return new Color(r, g, b);

        }

        private static float GetColorComponent(float temp1, float temp2, float temp3)
        {
            if (temp3 < 0f)
                temp3 += 1f;
            else if (temp3 > 1f)
                temp3 -= 1f;

            if (temp3 < 0.166666667f)
                return temp1 + (temp2 - temp1) * 6f * temp3;
            else if (temp3 < 0.5f)
                return temp2;
            else if (temp3 < 0.66666666f)
                return temp1 + ((temp2 - temp1) * (0.66666666f - temp3) * 6f);
            else
                return temp1;
        }

        public static Vector2 Clockwise90(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }
    }
}
