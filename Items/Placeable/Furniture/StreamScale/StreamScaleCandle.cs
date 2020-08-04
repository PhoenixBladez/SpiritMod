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
			item.width = 16;
			item.height = 16;
			item.value = 500;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<StreamScaleCandleTile>();
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