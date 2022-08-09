using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using System.Collections.Generic;

namespace SpiritMod
{
    partial class Helpers
    {
        public static Texture2D RadialMask => ModContent.Request<Texture2D>("SpiritMod/Effects/Masks/CircleGradient", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public static void DrawAdditive(Texture2D tex, Vector2 position, Color colour, float scale)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void DrawBezier(Texture2D Tex, Vector2 endPoints, Vector2 startingPos, Vector2 c1, float addonPerUse, float rotDis = 0f, float scale = 1, bool TrueRotation = false)
        {
            float width = Tex.Width;
            float length = (startingPos - endPoints).Length();
            float chainsPerUse = (width / length) * addonPerUse;
            for (float i = 0; i <= 1; i += chainsPerUse)
            {
                Vector2 distBetween;
                float projTrueRotation;
                if (i != 0)
                {
                    float x = X(i, startingPos.X, c1.X, endPoints.X);
                    float y = Y(i, startingPos.Y, c1.Y, endPoints.Y);

                    distBetween = new Vector2(x -
                    X(i - chainsPerUse, startingPos.X, c1.X, endPoints.X),
                    y -
                    Y(i - chainsPerUse, startingPos.Y, c1.Y, endPoints.Y));
                    projTrueRotation = distBetween.ToRotation() - MathHelper.PiOver2 + rotDis;

                    Main.spriteBatch.Draw(
                    Tex,
                    new Vector2(x, y).ForDraw(),
                    Tex.Bounds,
                    Lighting.GetColor((int)(x / 16), (int)(y / 16)),
                    TrueRotation ? 0 : projTrueRotation,
                    new Vector2(Tex.Width * 0.5f, Tex.Height * 0.5f), scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}
