
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.Shield)]
    public class BismiteShield : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Noxious Targe");
            Tooltip.SetDefault("Being struck by an enemy poisons them\nIncreases defense by 1 for every poisoned enemy near the player\nThis effect stacks up to five times");
        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 28;
            item.rare = 2;
            item.defense = 2;
            item.melee = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().bismiteShield = true;

            player.statDefense += 1 * player.GetSpiritPlayer().bismiteShieldStacks;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LeatherShield>(), 1);
            recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 6);
            recipe.AddRecipeGroup("SpiritMod:EvilMaterial1", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
