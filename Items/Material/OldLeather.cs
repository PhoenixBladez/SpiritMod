using Terraria;
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
			Item.width = 42;
			Item.height = 24;
			Item.value = 500;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 999;
		}

		public override void AddRecipes()
		{
			Recipe recipe = Mod.CreateRecipe(ItemID.Leather);
			recipe.AddIngredient(this, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
