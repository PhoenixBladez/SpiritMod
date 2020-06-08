
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class SepulchrePendant : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Necrotic Pendant");
            Tooltip.SetDefault("Getting hit occasionally lengthens immunity frames");

        }


        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 1;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().sepulchreCharm = true;
        }
    }
}
