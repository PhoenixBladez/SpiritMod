
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    [AutoloadEquip(EquipType.Back)]
    public class CloakOfSpirit : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cloak Of Spirit");
            Tooltip.SetDefault("Minions have a chance to return life\nMinions do 10% less damage");
        }
        public override void SetDefaults() {
            item.width = 30;
            item.height = 28;
            item.rare = 6;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().SpiritCloak = true;
            player.minionDamage *= 0.9f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CloakOfHealing>());
            recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
