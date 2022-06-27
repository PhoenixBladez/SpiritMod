using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AcidClockTile = SpiritMod.Tiles.Furniture.Acid.AcidClockTile;

namespace SpiritMod.Items.Placeable.Furniture.Acid
{
	public class AcidClock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Clock");
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

			Item.createTile = ModContent.TileType<AcidClockTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 10);
			recipe.AddIngredient(ItemID.Glass, 6);
			recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}