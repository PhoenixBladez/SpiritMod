using Terraria.ID;
using Terraria.ModLoader;
using ReachClockTile = SpiritMod.Tiles.Furniture.Reach.ReachClockTile;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachClockItem : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Clock");
        }


        public override void SetDefaults() {
            item.width = 32;
            item.height = 28;
            item.value = 500;

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<ReachClockTile>();
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark", 10);
            recipe.AddIngredient(ItemID.IronBar, 3);
            recipe.anyIronBar = true;
            recipe.AddIngredient(ItemID.Glass, 6);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}