using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;
using ReachLanternTile = SpiritMod.Tiles.Furniture.Reach.ReachLanternTile;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachLantern : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Lantern");
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

            item.createTile = ModContent.TileType<ReachLanternTile>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 4);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}