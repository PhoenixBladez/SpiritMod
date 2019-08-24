using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class SpiritCrystal : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Crystal");
			Tooltip.SetDefault("'Filled with ancient magic'");
		}


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 24;
            item.value = 100;
            item.rare = 5;

            item.maxStack = 999;
        }
    }
}
