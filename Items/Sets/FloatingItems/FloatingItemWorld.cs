using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace SpiritMod.Items.Sets.FloatingItems
{
	public class FloatingItemWorld : ModWorld
	{
		public WeightedRandom<int> floatingItemPool = new WeightedRandom<int>();

		public override void Initialize()
		{
			var types = typeof(FloatingItem).Assembly.GetTypes();
			foreach (var type in types)
			{
				if (typeof(FloatingItem).IsAssignableFrom(type) && !type.IsAbstract)
				{
					var v = Activator.CreateInstance(type) as FloatingItem;
					if (!floatingItemPool.elements.Any(x => x.Item1 == mod.ItemType(type.Name)))
						floatingItemPool.Add(mod.ItemType(type.Name), v.SpawnWeight);
				}
			}
		}

		public override void PreUpdate()
		{
			if (Main.rand.NextBool(3500))
			{
				int x = Main.rand.Next(600, Main.maxTilesX);
				if (Main.rand.NextBool(2))
					x = Main.rand.Next(Main.maxTilesX * 15, Main.maxTilesX * 16 - 600);

				int y = (int)(Main.worldSurface * 0.35) + 400;
				for (; Framing.GetTileSafely(x / 16, y / 16).liquid < 200; y += 16)
				{
					if (y / 16 > Main.worldSurface) //If we somehow miss all water, exit
						return;
				}
				y += 40;

				ItemUtils.NewItemWithSync(Main.myPlayer, x, y, 4, 4, floatingItemPool);
			}
		}
	}
}
