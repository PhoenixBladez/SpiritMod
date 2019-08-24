using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class PlatinumSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Platinum Broadsword");
			Tooltip.SetDefault("'It's worn, but could have a use'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.maxStack = 999;
            item.value = 500;
            item.value = Terraria.Item.buyPrice(0, 5, 0, 0);
            item.rare = 2;
        }
    }
}
