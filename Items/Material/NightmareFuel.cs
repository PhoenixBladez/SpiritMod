using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class NightmareFuel : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Fuel");
			Tooltip.SetDefault("'The stuff of... well... nightmares'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.value = 5000;
            item.rare = 8;

            item.maxStack = 999;
        }

        public override void AddRecipes() 
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FleshClump", 3);
            recipe.AddIngredient(ItemID.Ectoplasm);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}