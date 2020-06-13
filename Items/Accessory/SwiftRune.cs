using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class SwiftRune : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Swiftness Rune");
            Tooltip.SetDefault("Massively increases unassisted aerial agility");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVerticalRect(5, 5, new Rectangle(0, 2, 28, 38)));
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 38;
            item.value = Item.buyPrice(gold: 5);
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
