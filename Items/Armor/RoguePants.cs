using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class RoguePants : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Rogue Greaves");
            Tooltip.SetDefault("Increases movement speed by 5%");

        }
        public override void SetDefaults() {
            item.width = 22;
            item.height = 18;
            item.value = Terraria.Item.buyPrice(0, 0, 40, 0);
            item.value = 500;
            item.rare = 1;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player) {
            player.moveSpeed += 0.05f;
        }
    }
}