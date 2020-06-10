using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FrigidArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class FrigidChestplate : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Frigid Plate");
            Tooltip.SetDefault("Increases critical strike chance by 3%");

        }


        int timer = 0;

        public override void SetDefaults() {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 1;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player) {
            player.meleeCrit += 3;
            player.magicCrit += 3;
            player.rangedCrit += 3;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
