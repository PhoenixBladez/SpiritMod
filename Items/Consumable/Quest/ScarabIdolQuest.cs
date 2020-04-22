using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Quest
{
    public class ScarabIdolQuest : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Decrepit Idol");
			Tooltip.SetDefault("Quest Item\n'It's an ancient artifact that resembles a scarab beetle'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.rare = -11;
            item.maxStack = 99;
        }
    }
}
