using SpiritMod.Items.Placeable.Tiles;
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
			item.width = 32;
			item.height = 28;
			item.value = 500;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ShiverbarkTableTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 8);
			recipe.AddIngredient(ModContent.ItemType<CreepingIce>(), 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}