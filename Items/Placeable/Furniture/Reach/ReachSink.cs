using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReachSinkTile = SpiritMod.Tiles.Furniture.Reach.ReachSink;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachSink : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Sink");
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

			Item.createTile = ModContent.TileType<ReachSinkTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 5);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}