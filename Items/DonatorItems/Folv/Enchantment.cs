using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class Enchantment : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forgotten Enchantment");
			Tooltip.SetDefault("Runic inscription for a particular sword. \n  ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
            item.maxStack = 999;
            item.rare = 6;
        }
    }
}
