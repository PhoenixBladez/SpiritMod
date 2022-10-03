using Terraria.ID;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.World
{
	public static class TileSets
	{
		public static int[] Mosses = new int[] { TileID.ArgonMoss, TileID.BlueMoss, TileID.BrownMoss, TileID.GreenMoss, TileID.KryptonMoss, TileID.LavaMoss, TileID.PurpleMoss, TileID.RedMoss, TileID.XenonMoss };

		public static T[] With<T>(this T[] array, T[] otherArray)
		{
			List<T> newArr = array.ToList();
			newArr.AddRange(otherArray);
			return newArr.ToArray();
		}

		public static T[] With<T>(this T[] array, T newItem)
		{
			List<T> newArr = array.ToList();
			newArr.Add(newItem);
			return newArr.ToArray();
		}
	}
}
