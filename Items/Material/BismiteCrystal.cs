using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class BismiteCrystal : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Crystal");
			Tooltip.SetDefault("'Even though it looks metallic, it has a putrid smell...'");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 100;
            item.rare = 1;

            item.maxStack = 999;
        }
    }
}
