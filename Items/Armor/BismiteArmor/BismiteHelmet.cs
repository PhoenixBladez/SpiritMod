using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BismiteArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class BismiteHelmet : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bismite Helmet");
            Tooltip.SetDefault("Increases movement speed by 4%");

        }


        int timer = 0;
        public override void SetDefaults() {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 1;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player) {
            player.moveSpeed += .04f;
        }
        public override void UpdateArmorSet(Player player) {
            player.GetSpiritPlayer().bismiteSet = true;
            player.setBonus = "Not getting hit builds up stacks of Virulence\nVirulence charges up every 10 seconds\nStriking while Virulence is charged releases a toxic explosion\nGetting hit depletes Virulence entirely and releases a smaller blast";
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) {
            return body.type == ModContent.ItemType<BismiteChestplate>() && legs.type == ModContent.ItemType<BismiteLeggings>();
        }
        public override void ArmorSetShadows(Player player) {
            if(player.GetSpiritPlayer().virulence <= 0f) {
                player.armorEffectDrawShadow = true;
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
