using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class NetherCrystal : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nether Crystal");
			Tooltip.SetDefault("'Brimming with bright souls'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.rare = 5;

            item.maxStack = 999;
        }
    }
}
