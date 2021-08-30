using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using System.Collections.Generic;

namespace SpiritMod
{
    static partial class Helpers
    {
       public static bool HasParameter(this Effect effect, string parameterName)
        {
            foreach (EffectParameter parameter in effect.Parameters)
            {
                if (parameter.Name == parameterName)
                {
                    return true;
                }
            }

            return false;
		}

		public static void UpdateBasicEffect(ref BasicEffect effect, Vector2 zoom)
		{
			int width = Main.graphics.GraphicsDevice.Viewport.Width;
			int height = Main.graphics.GraphicsDevice.Viewport.Height;

			Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
						  Matrix.CreateTranslation(width / 2f, height / -2f, 0) * Matrix.CreateRotationZ(MathHelper.Pi) *
						  Matrix.CreateScale(zoom.X, zoom.Y, 1f);

			Matrix projection = Matrix.CreateOrthographic(width, height, 0, 1000);

			effect.View = view;
			effect.Projection = projection;
		}
	}
}
