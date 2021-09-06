using Microsoft.Xna.Framework;

namespace SpiritMod.Utilities
{
	public static class IterateVector2Method
	{
		public delegate void IterateAction(ref Vector2 item, int index, float progress);

		public static void IterateArray(this Vector2[] array, IterateAction action) 
		{
			for(int i = 0; i < array.Length; i++)
				action.Invoke(ref array[i], i, i / (float)array.Length);
		}
	}
}
