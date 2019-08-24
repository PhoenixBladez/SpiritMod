using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class OldLeather : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Old Leather");
			Tooltip.SetDefault("'Musty, but useful'");
		}


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 24;
            item.value = 100;
            item.rare = 1;

            item.maxStack = 999;
        }
    }
}
