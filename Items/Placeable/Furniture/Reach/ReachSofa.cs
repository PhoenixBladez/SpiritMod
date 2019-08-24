using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachSofa : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Sofa");
		}


		public override void SetDefaults()
		{
            item.width = 64;
			item.height = 34;
            item.value = 150;

            item.maxStack = 99;

            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createTile = mod.TileType("ReachSofa");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AncientBark", 5);
						recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
			recipe.AddRecipe();            
        }
	}
}