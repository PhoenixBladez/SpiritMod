using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FrigidSet
{
	public class FrigidFragment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Fragment");
			Tooltip.SetDefault("'Cold to the touch'");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = 100;
			item.rare = ItemRarityID.Blue;

			item.maxStack = 999;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 15);
			recipe.AddIngredient(this, 1);
			recipe.SetResult(ItemID.FrostburnArrow, 15);
			recipe.AddRecipe();
		}
	}
}