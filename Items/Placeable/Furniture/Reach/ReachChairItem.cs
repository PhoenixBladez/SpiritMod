using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture.Reach;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
	public class ReachChairItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark Chair");
		}


		public override void SetDefaults()
		{
			item.width = 64;
			item.height = 34;
			item.value = 150;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<ReachChair>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}