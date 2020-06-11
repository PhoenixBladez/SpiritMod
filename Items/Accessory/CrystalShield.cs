
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory.Leather;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class CrystalShield : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Crystal Shield");
            Tooltip.SetDefault("Walking leaves an aura of damaging crystals");
        }
        public override void SetDefaults() {
            item.width = 30;
            item.height = 28;
            item.rare = 5;
            item.defense = 3;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().CrystalShield = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ClatterboneShield>(), 1);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddIngredient(ItemID.HallowedBar, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
