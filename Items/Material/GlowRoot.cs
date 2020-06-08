using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class GlowRoot : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Glowroot");
        }


        public override void SetDefaults() {
            item.width = 24;
            item.height = 28;
            item.value = 300;
            item.rare = 1;

            item.maxStack = 999;
        }
    }
}