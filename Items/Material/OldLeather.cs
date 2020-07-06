using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class OldLeather : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Leather");
			Tooltip.SetDefault("'Musty, but useful'");
		}


		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 24;
			item.value = 500;
			item.rare = ItemRarityID.Blue;

			item.maxStack = 999;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.Leather);
			recipe.AddRecipe();
		}
	}
}
