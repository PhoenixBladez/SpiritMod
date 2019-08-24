using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class Material : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Shard");
			Tooltip.SetDefault("'Glistening with purified souls'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 34;
            item.rare = 8;

            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritOre", 3);
            recipe.AddIngredient(null, "Rune", 3);
            recipe.AddIngredient(null, "SoulShred", 3);
            recipe.AddIngredient(ItemID.Ectoplasm);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}
