
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class AceOfSpades : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ace of Spades");
            Tooltip.SetDefault("If a crit can kill an enemy, a crit is garunteed.");
        }


        public override void SetDefaults() {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetSpiritPlayer().AceOfSpades = true;
        }

    }
}
