using SpiritMod.Items.Sets.HuskstalkSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReachTableTile = SpiritMod.Tiles.Furniture.Reach.ReachTable;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Table");
		}


		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 34;
			Item.value = 150;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<ReachTableTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}