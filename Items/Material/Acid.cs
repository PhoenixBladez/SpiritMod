using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class Acid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Acid");
			Tooltip.SetDefault("'Extremely potent'");
		}


		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 24;
			item.value = 100;
			item.rare = ItemRarityID.Pink;

			item.maxStack = 999;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.VialofVenom, 1);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
}
