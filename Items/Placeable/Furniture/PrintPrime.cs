using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class PrintPrime : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron Prime Blueprint");
			Tooltip.SetDefault("Fore-Warned is Four-Armed.");
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

            item.createTile = mod.TileType("PrimePrint");
		}
	}
}