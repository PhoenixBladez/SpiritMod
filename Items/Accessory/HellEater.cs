
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class HellEater : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Fiery Maw");
            Tooltip.SetDefault("Magic attacks may shoot out fiery spit that explode upon hitting enemies\nIncreases magic damage by 7%");

        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 1, 50, 0);
            item.rare = 3;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().fireMaw = true;
            player.magicDamage += 0.07f;
        }
    }
}
