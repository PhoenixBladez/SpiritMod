using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class Whetstone : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Magic Whetstone");
            Tooltip.SetDefault("'A crystal that pulses with energy'");
        }
        public override void SetDefaults() {
            item.width = item.height = 16;
            item.maxStack = 999;
            item.rare = 4;
        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
    }
}
