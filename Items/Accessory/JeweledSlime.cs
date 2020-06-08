
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class JeweledSlime : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Prismatic Lens");
            Tooltip.SetDefault("Ranged attacks may inflict a multitude of debuffs on foes");

        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.rare = 5;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().geodeRanged = true;
        }
    }
}
