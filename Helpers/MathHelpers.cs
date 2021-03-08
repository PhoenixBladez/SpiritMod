using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using System.Collections.Generic;

namespace SpiritMod
{
    public static partial class Helpers
    {
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static Vector2 Size(this Texture2D texture) => new Vector2(texture.Width, texture.Height);
        public static Vector2 ForDraw(this Vector2 vec) => vec - Main.screenPosition;
        private static float X(float t,
     float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 3) +
                x1 * 3 * t * Math.Pow(1 - t, 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow(1 - t, 3) +
                 y1 * 3 * t * Math.Pow(1 - t, 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }
        private static float X(float t,
   float x0, float x1, float x2)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 2) +
                x1 * 2 * t * (1 - t) +
                x2 * Math.Pow(t, 2)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2)
        {
            return (float)(
                y0 * Math.Pow(1 - t, 2) +
                y1 * 2 * t * (1 - t) +
                y2 * Math.Pow(t, 2)
            );
        }
        public static Vector2 TraverseBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, Vector2 c2, float t)
        {
            float x = X(t, startingPos.X, c1.X, c2.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, c2.Y, endPoints.Y);
            return new Vector2(x, y);
        }
        public static Vector2 TraverseBezier(Vector2 endPoints, Vector2 startingPos, Vector2 c1, float t)
        {
            float x = X(t, startingPos.X, c1.X, endPoints.X);
            float y = Y(t, startingPos.Y, c1.Y, endPoints.Y);
            return new Vector2(x, y);
        }
    }
}
