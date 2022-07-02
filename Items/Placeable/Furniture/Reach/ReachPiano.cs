using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReachPianoTile = SpiritMod.Tiles.Furniture.Reach.ReachPiano;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachPiano : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Piano");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = 500;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<ReachPianoTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 4);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 15);
			recipe.AddIngredient(ItemID.Book, 1);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 4);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
}