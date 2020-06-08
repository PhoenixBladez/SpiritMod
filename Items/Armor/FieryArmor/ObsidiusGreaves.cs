using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FieryArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ObsidiusGreaves : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Slag Tyrant's Greaves");
            Tooltip.SetDefault("Increases minion damage by 6%\nIncreases maximum number of minions by 1");

        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 39, 0);
            item.rare = 3;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player) {
            player.minionDamage += 0.06f;
            player.maxMinions += 1;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
