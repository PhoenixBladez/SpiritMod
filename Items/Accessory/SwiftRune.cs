
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class SwiftRune : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Swiftness Rune");
            Tooltip.SetDefault("Massively increases unassisted aerial agility");
        }


        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 2;

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(player.velocity.Y != 0 && player.wings <= 0 && !player.mount.Active) {
                player.runAcceleration *= 2f;
                player.maxRunSpeed *= 1.5f;
            }
        }
    }
}
