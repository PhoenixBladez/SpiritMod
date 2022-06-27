using SpiritMod.Items.Placeable.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using StreamScaleLanternTile = SpiritMod.Tiles.Furniture.StreamScale.StreamScaleLanternTile;

namespace SpiritMod.Items.Placeable.Furniture.StreamScale
{
	public class StreamScaleLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Scale Lantern");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 28;
			Item.value = 200;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<StreamScaleLanternTile>();
		}
		//public override void AddRecipes()
		//{
			//ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ModContent.ItemType<AcidBrick>(), 6);
			//recipe.AddIngredient(ItemID.Torch, 1);
			//recipe.AddTile(TileID.HeavyWorkBench);
			//recipe.SetResult(this);
			//recipe.AddRecipe();
		//}
	}
}