using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class PrintProbe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destroyer Blueprint");
			Tooltip.SetDefault("WARNING: Release may cause seismic anomalies exceeding 10f.");
		}


		public override void SetDefaults()
		{
            item.width = 94;
			item.height = 62;
            item.value = 15000;
            item.rare = 6;

            item.maxStack = 99;

            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ProbePrint");
		}
	}
}