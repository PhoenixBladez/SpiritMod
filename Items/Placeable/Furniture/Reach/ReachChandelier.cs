using Terraria.ID;
using Terraria.ModLoader;
using ReachChandelierTile = SpiritMod.Tiles.Furniture.Reach.ReachChandelier;

namespace SpiritMod.Items.Placeable.Furniture.Reach
{
    public class ReachChandelier : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elderbark Chandelier");
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

            item.createTile = ModContent.TileType<ReachChandelierTile>();
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark", 4);
            recipe.AddIngredient(ItemID.Torch, 4);
            recipe.AddIngredient(ItemID.Chain, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}