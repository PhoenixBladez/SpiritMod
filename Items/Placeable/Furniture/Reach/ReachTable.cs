using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;
using ReachTableTile = SpiritMod.Tiles.Furniture.Reach.ReachTable;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachTable : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Table");
        }


        public override void SetDefaults() {
            item.width = 64;
            item.height = 34;
            item.value = 150;

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<ReachTableTile>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}