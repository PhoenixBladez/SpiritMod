
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class AceOfHearts: ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ace of Hearts");
            Tooltip.SetDefault("Enemies killed by a critical hit always drop hearts");
        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().AceOfHearts = true;
        }

    }
}
