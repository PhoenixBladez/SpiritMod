using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class EmptyCodex : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Arcane Codex");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.maxStack = 999;
			item.value = 500;
			item.rare = 2;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
