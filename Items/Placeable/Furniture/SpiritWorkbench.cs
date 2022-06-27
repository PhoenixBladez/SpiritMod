using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritWorkbenchTile = SpiritMod.Tiles.Furniture.SpiritWorkbench;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SpiritWorkbench : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Duskwood Work Bench");

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 25;
			Item.value = 150;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<SpiritWorkbenchTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritWoodItem>(), 10);
			recipe.Register();
		}
	}
}