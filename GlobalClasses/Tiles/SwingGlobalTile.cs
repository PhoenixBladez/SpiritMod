using SpiritMod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Tiles
{
	internal class SwingGlobalTile : GlobalTile
	{
		public List<int> Vines = new List<int>();
		public List<int> Swings = new List<int>();

		public void Load(Mod mod)
		{
			var types = typeof(SwingGlobalTile).Assembly.GetTypes();
			foreach (var type in types)
			{
				if (typeof(ModTile).IsAssignableFrom(type))
				{
					var tag = (TileTagAttribute)Attribute.GetCustomAttribute(type, typeof(TileTagAttribute));

					if (tag == null || tag.Tags.Length == 0)
						continue;

					if (tag.Tags.Contains(TileTags.VineSway))
						Vines.Add(mod.Find<ModTile>(type.Name).Type);

					if (tag.Tags.Contains(TileTags.ChandelierSway))
						Swings.Add(mod.Find<ModTile>(type.Name).Type);
				}
			}
		}
	}
}
