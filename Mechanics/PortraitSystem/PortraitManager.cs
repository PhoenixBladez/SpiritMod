using System;
using System.Collections.Generic;

namespace SpiritMod.Mechanics.PortraitSystem
{
	public class PortraitManager
	{
		private static readonly Dictionary<int, BasePortrait> portraits = new Dictionary<int, BasePortrait>();

		public static void Load() //Load all portraits and add them to the dict
		{
			var types = typeof(BasePortrait).Assembly.GetTypes();
			foreach (var type in types)
			{
				if (type.IsSubclassOf(typeof(BasePortrait)) && !type.IsAbstract)
				{
					var p = Activator.CreateInstance(type, true) as BasePortrait;
					portraits.Add(p.ID, p);
				}
			}
		}

		public static void Unload() => portraits.Clear();

		public static BasePortrait GetPortrait(int ID) => portraits[ID];
		public static bool HasPortrait(int ID) => portraits.ContainsKey(ID);
	}
}
