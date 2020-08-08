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
			item.width = 32;
			item.height = 18;
			item.value = 500;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<StreamScaleBenchTile>();
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