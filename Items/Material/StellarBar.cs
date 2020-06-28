using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class StellarBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Bar");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 5;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = ModContent.TileType<Tiles.Furniture.StellarBar>();
			item.maxStack = 999;
			item.consumable = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarPiece>());
			recipe.AddIngredient(ItemID.TitaniumBar);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(ModContent.ItemType<StarPiece>());
			recipe2.AddIngredient(ItemID.AdamantiteBar);
			recipe2.AddTile(TileID.AdamantiteForge);
			recipe2.SetResult(this);
			recipe2.AddRecipe();
		}
	}
}