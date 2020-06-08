using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class LeatherGlove : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Leather Strikers");
            Tooltip.SetDefault("Slightly increases weapon speed");
        }
        public override void SetDefaults() {
            item.width = 26;
            item.height = 34;
            item.rare = 1;
            item.value = 1200;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().leatherGlove = true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 6);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
