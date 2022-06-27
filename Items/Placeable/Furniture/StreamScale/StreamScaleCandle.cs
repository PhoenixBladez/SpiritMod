using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using StreamScaleCandleTile = SpiritMod.Tiles.Furniture.StreamScale.StreamScaleCandleTile;

namespace SpiritMod.Items.Placeable.Furniture.StreamScale
{
	public class StreamScaleCandle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Scale Candle");
		}


		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.value = 500;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<StreamScaleCandleTile>();
		}
		//public override void AddRecipes()
		//{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 4);
			//recipe.AddIngredient(ItemID.Torch, 1);
			//recipe.AddTile(TileID.HeavyWorkBench);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		//}
	}
}