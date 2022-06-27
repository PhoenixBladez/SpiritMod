using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class ScrapItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Scrap Block");
		}


		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<ScrapTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(30);
			recipe.AddIngredient(ModContent.ItemType<SpaceJunkItem>(), 2);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.Register();
		}
	}
}