using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class BlueprintTwins : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twins Blueprint");
			Tooltip.SetDefault("They can't kill what they can't see... They see everything.");
		}


		public override void SetDefaults()
		{
            item.width = 94;
			item.height = 62;
            item.value = 15000;
            item.rare = 6;

            item.maxStack = 99;

			item.useTurn = true;
			item.autoReuse = true;
            item.consumable = true;

            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;


            item.createTile = mod.TileType("TwinsPrint");
		}
	}
}