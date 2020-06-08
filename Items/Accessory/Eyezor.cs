
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class Eyezor : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Scrying Eye");
            Tooltip.SetDefault("Magic critical strikes may deal extra damage to foes\nIncreases magic critical strike chance by 4%");

        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 6;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().eyezorEye = true;
            player.magicCrit += 4;
        }
    }
}
