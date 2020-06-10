using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CryoliteArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class CryoliteHead : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cryolite Helmet");
            Tooltip.SetDefault("Increases melee speed by 13%");
        }

        public override void SetDefaults() {
            item.width = 38;
            item.height = 26;
            item.value = 10000;
            item.rare = 3;
            item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<CryoliteBody>() && legs.type == ModContent.ItemType<CryoliteLegs>();
        }
        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Generates an icy aura that slows nearby enemies\nThis aura expands gradually, but resets if the player is hurt";
            player.GetSpiritPlayer().cryoSet = true;
        }

        public override void UpdateEquip(Player player) {
            player.meleeSpeed += 0.13f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
