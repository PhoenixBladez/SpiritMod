using Terraria;
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
			Item.width = 42;
			Item.height = 24;
			Item.value = 100;
			Item.rare = ItemRarityID.Pink;

			Item.maxStack = 999;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(5);
			recipe.AddIngredient(ItemID.VialofVenom, 1);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
