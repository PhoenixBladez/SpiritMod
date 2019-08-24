using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class InfernalAppendage : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torment Shard");
			Tooltip.SetDefault("'Filled with suffering'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.maxStack = 999;
            item.rare = 4;
        }
    }
}
