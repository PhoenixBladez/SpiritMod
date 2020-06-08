using Terraria.ID;
using Terraria.ModLoader;
using BarkWallWall = SpiritMod.Tiles.Walls.Natural.BarkWall;

namespace SpiritMod.Items.Placeable.Walls
{
    public class BarkWall : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Wall");
        }


        public override void SetDefaults() {
            item.width = 12;
            item.height = 12;

            item.maxStack = 999;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 7;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createWall = ModContent.WallType<BarkWallWall>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark");
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(this, 4);
            recipe1.AddTile(TileID.WorkBenches);
            recipe1.SetResult(null, "AncientBark");
            recipe1.AddRecipe();
        }
    }
}