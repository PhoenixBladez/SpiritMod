/*using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class TreemanStatue : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Statue of the Old Gods");
			 Tooltip.SetDefault("Provides the effects of a Workbench, Altar of Creation, Potion Crafting Station, and Bookcase\n'The Old Ones will protect you'");

		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
            item.value = 20500;

            item.maxStack = 99;
			item.rare = 7;

            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createTile = mod.TileType("TreemanStatue");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AncientBark", 20);
						recipe.AddIngredient(null,"EnchantedLeaf", 20);
																		recipe.AddIngredient(ItemID.GoldCoin, 10);
						recipe.AddIngredient(ItemID.Book, 5);
												recipe.AddIngredient(ItemID.Bone, 5);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this, 1);
			recipe.AddRecipe();            
        }
	}
}*/