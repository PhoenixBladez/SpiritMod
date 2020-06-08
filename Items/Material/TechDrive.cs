using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class TechDrive : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Tech Drive");
            Tooltip.SetDefault("'How does a goblin stumble upon such advanced circuitry?'");
        }


        public override void SetDefaults() {
            item.width = 42;
            item.height = 24;
            item.value = 100;
            item.rare = 2;

            item.maxStack = 999;
        }
    }
}
