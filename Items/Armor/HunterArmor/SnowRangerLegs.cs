
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.HunterArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SnowRangerLegs : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fur Legwraps");
        }
        public override void SetDefaults() {
            item.width = 18;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 2;

            item.vanity = true;
        }
    }
}
