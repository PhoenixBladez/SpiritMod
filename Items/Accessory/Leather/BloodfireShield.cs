
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.Shield)]
    public class BloodfireShield : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Sanguine Scutum");
            Tooltip.SetDefault("Reduces life regen to 0\nIncreases damage reduction by 3% for every nearby enemy\nThis effect stacks up to five times");
        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 28;
            item.rare = 2;
            item.defense = 1;
            item.melee = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().bloodfireShield = true;

            player.endurance += .03f * player.GetSpiritPlayer().bloodfireShieldStacks;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LeatherShield>(), 1);
            recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 6);
            recipe.AddRecipeGroup("SpiritMod:EvilMaterial1", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
