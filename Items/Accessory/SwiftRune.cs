
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class SwiftRune : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Swiftness Rune");
            Tooltip.SetDefault("Increases movement speed by 7%\nIf jumping or falling, this boost is increased by 50%");
        }


        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 2;

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.moveSpeed += .07f;
            player.maxRunSpeed += 0.02f;
            if(player.velocity.Y != 0) {
                player.moveSpeed += .035f;
                player.maxRunSpeed += 0.01f;
            }
        }
    }
}
