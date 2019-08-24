using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Walls
{
	public class SpiritWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Wall");
		}


		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;

			item.maxStack = 999;

            item.useStyle = 1;
			item.useTime = 7;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createWall = mod.WallType("SpiritWall");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpiritWoodItem");
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}