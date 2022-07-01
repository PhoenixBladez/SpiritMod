using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	/// <summary>
	/// Static helper class containing a method for quickly drawing godrays using spritebatch
	/// </summary>
    public static class DrawGodray
	{
		public static void DrawGodrays(SpriteBatch spriteBatch, Vector2 position, Color rayColor, float baseLength, float width, int numRays)
		{
			for (int i = 0; i < numRays; i++)
			{
				Texture2D ray = ModContent.Request<Texture2D>("Textures/Ray");
				float rotation = i * (MathHelper.TwoPi / numRays) + (Main.GlobalTimeWrappedHourly * (((i % 3) + 1f) / 3)); //Half of rays rotate faster, so it looks less like a rotating static image
				rotation -= MathHelper.PiOver2;

				float length = baseLength * (float)(Math.Sin((Main.GlobalTimeWrappedHourly + i) * 2) / 5 + 1); //arbitrary sine function to fluctuate length between rays over time, change later to be less bad?
				Vector2 rayscale = new Vector2(width / ray.Width, length / ray.Height);
				spriteBatch.Draw(ray, position, null, rayColor, rotation,
					new Vector2(ray.Width / 2, 0), rayscale, SpriteEffects.None, 0);
			}
		}
	}
}