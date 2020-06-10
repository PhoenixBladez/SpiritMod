using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class StarMask : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Astralite Visor");
            Tooltip.SetDefault("Increases ranged damage by 5%\nLeave a trail of stars where you walk");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/StarArmor/StarMask_Glow");
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) {
            glowMaskColor = Color.White;
        }
        int timer = 0;

        public override void SetDefaults() {
            item.width = 22;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 30, 0);
            item.rare = 3;
            item.defense = 6;
        }
        public override void UpdateEquip(Player player) {
            player.rangedDamage += .05f;
            if(player.velocity.X != 0f) {
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + player.height - 4f), player.width, 0, 226);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].scale *= .4f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void UpdateArmorSet(Player player) {
            player.setBonus = "Press the 'Armor Bonus' hotkey to deploy an energy field at the cursor position\nThis field lasts for five seconds and supercharges all ranged projectiles that pass through it\n12 second cooldown";
            player.GetSpiritPlayer().starSet = true;
            player.endurance += 0.05f;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<StarPlate>() && legs.type == ModContent.ItemType<StarLegs>();
        }
        public override void ArmorSetShadows(Player player) {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<SteamParts>(), 7);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
