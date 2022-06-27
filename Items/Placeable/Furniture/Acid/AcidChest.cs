using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AcidChestTile = SpiritMod.Tiles.Furniture.Acid.AcidChestTile;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Chest");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = 200;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<AcidChestTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 8);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}