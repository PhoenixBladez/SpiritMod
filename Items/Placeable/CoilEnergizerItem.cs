using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
	public class CoilEnergizerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Energizer");
            Tooltip.SetDefault("Creates an electric field that energizes nearby players\nThis");
		}


		public override void SetDefaults()
		{
            item.width = 28;
			item.height = 22;
            item.value = Item.sellPrice(0, 0, 10, 0); ;

            item.maxStack = 99;

            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createTile = mod.TileType("CoilEnergizerTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"TechDrive", 10);
            recipe.SetResult(this);
			recipe.AddRecipe();            
        }
	}
}