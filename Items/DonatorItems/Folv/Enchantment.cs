using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class Enchantment : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Forgotten Enchantment");
            Tooltip.SetDefault("'A runic inscription for a particular sword'");
        }


        public override void SetDefaults() {
            item.width = 28;
            item.height = 30;
            item.maxStack = 999;
            item.rare = 6;
        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
    }
}
