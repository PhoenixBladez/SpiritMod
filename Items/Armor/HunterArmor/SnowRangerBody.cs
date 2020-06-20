
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HunterArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class SnowRangerBody : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fur Coverings");
        }
        public override void SetDefaults() {
            item.width = 30;
            item.height = 30;
            item.value = Item.sellPrice(0, 0, 30, 0);
            item.rare = 2;

            item.vanity = true;
        }
    }
}
