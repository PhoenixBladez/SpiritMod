using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace SpiritMod.Utilities
{
    public static class PulseDraw
	{
		public delegate void DelegateAction(Vector2 positionOffset, float opacityMod);

		/// <summary>
		/// Helper method for the pulsing spritebatch effect.
		/// </summary>
		/// <param name="timerInput"></param>
		/// <param name="numToDraw"></param>
		/// <param name="strength"></param>
		/// <param name="action"></param>
		public static void DrawPulseEffect(float timerInput, int numToDraw, float strength, DelegateAction action)
		{
			float timer = ((float)Math.Sin(timerInput) / 2) + 0.5f;

			for(int i = 0; i < numToDraw; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / numToDraw) * timer * strength;
				action.Invoke(offset, 1 - timer);
			}
		}
	}
}