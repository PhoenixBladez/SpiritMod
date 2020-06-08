using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SeraphArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class SeraphHelm : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Seraph's Crown");
            Tooltip.SetDefault("Increases melee damage by 12%");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/SeraphArmor/SeraphHelm_Glow");
        }
        public override void SetDefaults() {
            item.width = 28;
            item.height = 24;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.defense = 10;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<SeraphArmor>() && legs.type == ModContent.ItemType<SeraphLegs>();
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) {
            glowMaskColor = Color.White;
        }
        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Being near enemies increases life regen, increases melee speed\nand reduces mana cost by 6% per enemy\nThis effect stacks three times";
            player.GetSpiritPlayer().astralSet = true;
        }
        public override void UpdateEquip(Player player) {
            player.meleeDamage += .12f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}