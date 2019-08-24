using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SoulOrbTerrarium : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Orb Terrarium");
		}


		public override void SetDefaults()
		{
            item.width = 64;
			item.height = 34;
            item.value = 150;

            item.maxStack = 99;

            item.useStyle = 1;
			item.useTime = 15;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createTile = mod.TileType("SoulOrbTerrarium");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"SoulOrbItem", 1);
			recipe.AddIngredient(2208, 1);
            recipe.SetResult(this);
			recipe.AddRecipe();            
        }
	}
}