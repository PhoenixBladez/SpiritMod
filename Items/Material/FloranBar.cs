using Terraria.ID;
using Terraria.ModLoader;
using FloranBarTile = SpiritMod.Tiles.Furniture.FloranBar;

namespace SpiritMod.Items.Material
{
	public class FloranBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Ingot");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = 4050;
			item.rare = ItemRarityID.Green;
			item.createTile = ModContent.TileType<FloranBarTile>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FloranOre>(), 3);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}