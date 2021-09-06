using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		public static void SetBasicEffectMatrices(ref BasicEffect effect, Vector2 zoom)
		{
			GetWorldViewProjection(zoom, out Matrix view, out Matrix projection);

			effect.View = view;
			effect.Projection = projection;
		}

		public static void SetEffectMatrices(ref Effect effect)
		{
			GetWorldViewProjection(out Matrix view, out Matrix projection);

			if (effect.HasParameter("WorldViewProjection"))
				effect.Parameters["WorldViewProjection"].SetValue(view * projection);
		}

		public static void GetWorldViewProjection(out Matrix view, out Matrix projection) => GetWorldViewProjection(Main.GameViewMatrix.Zoom, out view, out projection);

		public static void GetWorldViewProjection(Vector2 zoom, out Matrix view, out Matrix projection)
		{
			int width = Main.graphics.GraphicsDevice.Viewport.Width;
			int height = Main.graphics.GraphicsDevice.Viewport.Height;

			view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
						  Matrix.CreateTranslation(width / 2f, height / -2f, 0) * Matrix.CreateRotationZ(MathHelper.Pi) *
						  Matrix.CreateScale(zoom.X, zoom.Y, 1f);

			projection = Matrix.CreateOrthographic(width, height, 0, 1000);
		}
	}
}
