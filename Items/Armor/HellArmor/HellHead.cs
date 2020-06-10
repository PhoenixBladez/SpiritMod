using SpiritMod.Items.Material;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HellArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class HellHead : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Malevolent Cowl");
            Tooltip.SetDefault("Increases ranged damage by 18% and increases ranged critical strike chance by 7%");
        }


        int timer = 0;
        public override void SetDefaults() {
            item.width = 20;
            item.height = 18;
            item.value = 46000;
            item.rare = 6;
            item.defense = 11;
        }
        public override void UpdateEquip(Player player) {
            player.rangedDamage += 0.18f;
            player.rangedCrit += 7;

        }
        public override void ArmorSetShadows(Player player) {
            player.armorEffectDrawShadow = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<HellBody>() && legs.type == ModContent.ItemType<HellLegs>();
        }
        public override void UpdateArmorSet(Player player) {

            player.setBonus = "Ranged attacks occasionally triggers explosions around the player, raining down fireballs";
            player.GetSpiritPlayer().hellSet = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<FieryEssence>(), 16);
            recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}