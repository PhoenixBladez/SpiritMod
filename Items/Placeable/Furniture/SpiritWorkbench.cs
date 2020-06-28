using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritWorkbenchTile = SpiritMod.Tiles.Furniture.SpiritWorkbench;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SpiritWorkbench : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Workbench");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 25;
			item.value = 150;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<SpiritWorkbenchTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SpiritWoodItem>(), 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}