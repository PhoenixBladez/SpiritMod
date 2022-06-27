using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using StreamScaleBenchTile = SpiritMod.Tiles.Furniture.StreamScale.StreamScaleBenchTile;

namespace SpiritMod.Items.Placeable.Furniture.StreamScale
{
	public class StreamScaleBench : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Scale Work Bench");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 18;
			Item.value = 500;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<StreamScaleBenchTile>();
		}
		//public override void AddRecipes()
		//{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 14);
			//recipe.AddTile(TileID.HeavyWorkBench);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		//}
	}
}