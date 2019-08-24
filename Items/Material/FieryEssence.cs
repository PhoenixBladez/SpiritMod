using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material
{
    public class FieryEssence : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Essence");
			Tooltip.SetDefault("'The Essence of the Malevolent'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.AnimatesAsSoul[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = item.height = 22;
            item.maxStack = 999;
            item.rare = 6;
        }
    }
}
