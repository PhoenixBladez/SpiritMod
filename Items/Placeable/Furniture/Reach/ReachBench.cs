using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture.Reach;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachBench : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Workbench");
        }


        public override void SetDefaults() {
            item.width = 44;
            item.height = 25;
            item.value = 150;

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<ReachBenchTile>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}