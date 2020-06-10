using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
    public class SpiritDoorItem : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Duskwood Door");
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

            item.createTile = ModContent.TileType<SpiritDoorClosed>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<SpiritWoodItem>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}