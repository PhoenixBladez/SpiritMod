using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShiverbarkTableTile = SpiritMod.Tiles.Furniture.Shiverbark.ShiverbarkTableTile;

namespace SpiritMod.Items.Placeable.Furniture.Shiverbark
{
	public class ShiverbarkTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shiverbark Table");
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

			Item.createTile = ModContent.TileType<ShiverbarkTableTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddIngredient(ModContent.ItemType<CreepingIce>(), 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}