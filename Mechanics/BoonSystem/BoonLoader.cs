using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SpiritMod.Mechanics.BoonSystem
{
	public static class BoonLoader
	{
		public static List<Boon> LoadedBoons = new List<Boon>();
		public static void Load()
		{
			LoadedBoons = new List<Boon>();
			foreach (Type type in SpiritMod.Instance.Code.GetTypes())
			{
				if (!type.IsAbstract && type.IsSubclassOf(typeof(Boon)))
				{
					var instance = Activator.CreateInstance(type);
					LoadedBoons.Add(instance as Boon);
				}
			}
		}

		public static void Unload() => LoadedBoons = null;
	}
}