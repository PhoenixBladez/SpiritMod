using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShiverbarkTotemTile = SpiritMod.Tiles.Furniture.Shiverbark.ShiverbarkTotemTile;

namespace SpiritMod.Items.Placeable.Furniture.Shiverbark
{
	public class ShiverbarkTotem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shiverbark Totem");
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

			Item.createTile = ModContent.TileType<ShiverbarkTotemTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddIngredient(ModContent.ItemType<CreepingIce>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}