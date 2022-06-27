using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using StreamScaleChairTile = SpiritMod.Tiles.Furniture.StreamScale.StreamScaleChairTile;

namespace SpiritMod.Items.Placeable.Furniture.StreamScale
{
	public class StreamScaleChair : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Scale Chair");
		}


		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 32;
			Item.value = 200;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<StreamScaleChairTile>();
		}
		//public override void AddRecipes()
		//{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 4);
			//recipe.AddTile(TileID.HeavyWorkBench);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		//}
	}
}