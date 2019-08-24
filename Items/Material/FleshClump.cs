using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class FleshClump : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Clump");
			Tooltip.SetDefault("'Gross...'");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 42;
            item.value = 100;
            item.rare = 4;

            item.maxStack = 999;
        }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 2);
            recipe.AddIngredient(ItemID.Ichor);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
} 
