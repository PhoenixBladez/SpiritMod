using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class DuskStone : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Shard");
			Tooltip.SetDefault("'The stone sparkles with twilight energies'\nInvolved in the crafting of Dusk Armor");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.maxStack = 999;
            item.rare = 5;
        }
    }
}
