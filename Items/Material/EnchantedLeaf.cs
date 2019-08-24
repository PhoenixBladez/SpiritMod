using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class EnchantedLeaf : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Leaf");
			Tooltip.SetDefault("'Blessed with the magic of druids'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.maxStack = 999;
            item.value = 500;
            item.rare = 1;
        }
    }
}
