using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class StarLegs : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Astralite Leggings");
            Tooltip.SetDefault("Increases ranged damage by 6% and movement speed by 8%");
        }
        int timer = 0;
        public override void SetDefaults() {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 5;
        }
        public override void UpdateEquip(Player player) {
            player.moveSpeed += 0.08f;
            player.maxRunSpeed += 0.05f;
            player.rangedDamage += .06f;
        }
        public override void ArmorSetShadows(Player player) {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<SteamParts>(), 8);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
