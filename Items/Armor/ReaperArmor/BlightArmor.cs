using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReaperArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class BlightArmor : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Reaper's Breastplate");
            Tooltip.SetDefault("11% increased damage\n6% increased critical strike chance");
        }

        public override void SetDefaults() {
            item.width = 34;
            item.height = 24;
            item.value = Item.buyPrice(gold: 12);
            item.rare = 8;
            item.defense = 22;
        }

        public override void UpdateEquip(Player player) {
			player.allDamage += 0.11f;
            player.magicCrit += 6;
            player.meleeCrit += 6;
            player.rangedCrit += 6;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CursedFire>(), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
