using Microsoft.Xna.Framework;
using Terraria;

namespace SpiritMod.Utilities
{
    public static class DrawAberration
	{
		public delegate void DelegateAction(Vector2 positionOffset, Color colorMod);

		public static void DrawChromaticAberration(Vector2 direction, float strength, DelegateAction action)
		{
			for (int i = -1; i <= 1; i++)
			{
				Color aberrationColor = Color.White;
				switch (i)
				{
					case -1:
						aberrationColor = new Color(255, 0, 0, 0);
						break;
					case 0:
						aberrationColor = new Color(0, 255, 0, 0);
						break;
					case 1:
						aberrationColor = new Color(0, 0, 255, 0);
						break;
				}

				Vector2 offset = direction.RotatedBy(MathHelper.PiOver2) * i;
				offset *= strength;

				action.Invoke(offset, aberrationColor);
			}
		}
	}
}