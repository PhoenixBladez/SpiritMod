using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class SpiritKoi : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Koi");
			Tooltip.SetDefault("'Is it past its expiry date?'");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 42;
            item.value = 100;
            item.rare = 4;

            item.maxStack = 999;
        }
    }
} 
