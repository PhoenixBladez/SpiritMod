using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class ManaShard : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mana Shard");
            Tooltip.SetDefault("A conduit for mana");
        }


        public override void SetDefaults() {
            item.width = 24;
            item.height = 30;
            item.value = 100;
            item.rare = 1;

            item.maxStack = 999;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BandofRegeneration, 1);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.AddIngredient(ItemID.TissueSample, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(ItemID.BandofStarpower, 1);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.BandofRegeneration, 1);
            recipe2.AddIngredient(ItemID.FallenStar, 3);
            recipe2.AddIngredient(ItemID.ShadowScale, 2);
            recipe.AddTile(TileID.Anvils);
            recipe2.SetResult(ItemID.BandofStarpower, 1);
            recipe2.AddRecipe();
        }
    }
}