using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GraniteArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class GraniteHelm : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Granite Visor");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/GraniteArmor/GraniteHelm_Glow");
            Tooltip.SetDefault("Increases jump height slightly");

        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 2;
            item.defense = 6;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) {
            glowMaskColor = Color.White;
        }
        public override void UpdateEquip(Player player) {
            player.jumpSpeedBoost += 1f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<GraniteChest>() && legs.type == ModContent.ItemType<GraniteLegs>();
        }
        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Press 'Down' while falling to stomp downward\nHitting the ground releases a shockwave that scales with height\n10 second cooldown";
            player.GetSpiritPlayer().graniteSet = true;
        }
        public override void ArmorSetShadows(Player player) {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
