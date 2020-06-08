using Terraria.ID;
using Terraria.ModLoader;
using EssenceDistorterTile = SpiritMod.Tiles.Furniture.EssenceDistorter;

namespace SpiritMod.Items.Placeable.Furniture
{
    public class EssenceDistorter : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Essence Distorter");
            Tooltip.SetDefault("'Where essences are warped and merged'");
        }


        public override void SetDefaults() {
            item.width = item.height = 16;
            item.maxStack = 1;
            item.rare = 6;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 25;

            item.autoReuse = true;
            item.consumable = true;


            item.createTile = ModContent.TileType<EssenceDistorterTile>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuneEssence");
            recipe.AddIngredient(null, "TidalEssence");
            recipe.AddIngredient(null, "FieryEssence");
            recipe.AddIngredient(null, "IcyEssence");
            recipe.AddIngredient(null, "PrimevalEssence");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
