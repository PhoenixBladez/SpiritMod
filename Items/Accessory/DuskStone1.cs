
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class DuskStone1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dusk Stone");
            Tooltip.SetDefault("8% increased ranged damage\nIncreases ranged critical strike chance the less health you have\nRanged attacks may burn enemies with lunar rays");
        }


        public override void SetDefaults() {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.rare = 3;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.rangedDamage += 0.08f;
            float defBoost = (player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 10f;
            player.rangedCrit += (int)defBoost;

            player.GetSpiritPlayer().moonStone = true;
        }

    }
}
