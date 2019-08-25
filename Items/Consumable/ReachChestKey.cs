using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ReachChestKey : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gnarled Key");
			Tooltip.SetDefault("Opens Briar Chests in the Reach");
		}


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 28;
            item.rare = 0;
            item.maxStack = 99;
			item.value = 0;
           // item.useStyle = 4;
           // item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            //item.consumable = true;
            //item.autoReuse = false;

       //     item.UseSound = SoundID.Item43;
        }

       
    }
}
