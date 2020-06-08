using Terraria.ID;
using Terraria.ModLoader;
using ReachCandleTile = SpiritMod.Tiles.Furniture.Reach.ReachCandle;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachCandle : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Candle");
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

            item.createTile = ModContent.TileType<ReachCandleTile>();
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark", 4);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}