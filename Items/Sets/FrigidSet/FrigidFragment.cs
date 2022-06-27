using Terraria;
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
			Item.width = 24;
			Item.height = 28;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			var recipe = Mod.CreateRecipe(ItemID.FrostburnArrow, 15);
			recipe.AddIngredient(ItemID.WoodenArrow, 15);
			recipe.AddIngredient(this, 1);
			recipe.Register();
		}
	}
}