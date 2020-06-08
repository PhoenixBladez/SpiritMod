using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class StoneBody : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Stone Plate");
            Tooltip.SetDefault("Decreases movement speed by 5%");

        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 20;
            item.value = 600;
            item.rare = 1;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player) {
            player.moveSpeed -= .05f;
            player.maxRunSpeed -= 0.05f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}