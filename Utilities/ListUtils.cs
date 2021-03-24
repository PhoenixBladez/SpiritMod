using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Utilities
{
	public static class ListUtils
	{
		public static void AddWithCondition<T>(this List<T> list, T item, bool condition)
		{
			if (condition) {
				list.Add(item);
			}
		}

		/// <summary>
		/// Swaps the positions of two elements in a list, given their indexes
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index"></param>
		/// <param name="index2"></param>
		public static void Swap<T>(this List<T> list, int index, int index2)
		{
			var tempvar = list[index];
			list[index] = list[index2];
			list[index2] = tempvar;
		}

		/// <summary>
		/// Randomly swaps all elements in a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void Randomize<T>(this List<T> list)
		{
			for(int i = 0; i < list.Count; i++) 
				list.Swap(Main.rand.Next(list.Count), Main.rand.Next(list.Count));
		}
	}
}
