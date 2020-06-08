using Terraria.ID;
using Terraria.ModLoader;
using ReachSinkTile = SpiritMod.Tiles.Furniture.Reach.ReachSink;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachSink : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Sink");
        }


        public override void SetDefaults() {
            item.width = 32;
            item.height = 28;
            item.value = 200;

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<ReachSinkTile>();
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark", 5);
            recipe.AddIngredient(ItemID.IronBar, 1);
            recipe.anyIronBar = true;
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}