using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FieryArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class ObsidiusPlate : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Slag Tyrant's Platemail");
            Tooltip.SetDefault("Increases minion damage by 5%\nIncreases your max number of minions by 1");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/FieryArmor/ObsidiusPlate_Glow");

        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 7;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) {
            glowMaskColor = Color.White;
        }
        public override void UpdateEquip(Player player) {
            player.maxMinions += 1;
            player.minionDamage += .05f;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 17);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
