using Terraria.ID;
using Terraria.ModLoader;
using ReachPianoTile = SpiritMod.Tiles.Furniture.Reach.ReachPiano;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachPiano : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Piano");
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

            item.createTile = ModContent.TileType<ReachPianoTile>();
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 4);
            recipe.AddIngredient(null, "AncientBark", 15);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.anyIronBar = true;
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}