using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Walls
{
	public class SepulchreWallItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sepulchre Wall");
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

			item.createWall = mod.WallType("SepuclhreWallTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SepulchreBrickItem");
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(this, 4);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.SetResult(null, "SepulchreBrickItem");
			recipe1.AddRecipe();
		}
	}
}