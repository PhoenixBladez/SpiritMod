
using SpiritMod.Items.Accessory;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    [AutoloadEquip(EquipType.Shield)]
    public class Firewall : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Firewall");
            Tooltip.SetDefault("Negates knockback \nDouble tap right to dash");
        }
        public override void SetDefaults() {
            item.width = 30;
            item.height = 28;
            item.rare = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.accessory = true;
            item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().firewall = true;
            player.noKnockback = true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ObsidianShield, 1);
            recipe.AddIngredient(ItemID.EoCShield, 1);
            recipe.AddIngredient(ModContent.ItemType<InfernalShield>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
