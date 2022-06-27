using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReachClockTile = SpiritMod.Tiles.Furniture.Reach.ReachClockTile;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachClockItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Clock");
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

			Item.createTile = ModContent.TileType<ReachClockTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 10);
			recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.anyIronBar = true;
			recipe.AddIngredient(ItemID.Glass, 6);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
}