using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class Acid : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrosive Acid");
			Tooltip.SetDefault("'Extremely potent'");
		}


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 24;
            item.value = 100;
            item.rare = 5;

            item.maxStack = 999;
        }
    }
}
