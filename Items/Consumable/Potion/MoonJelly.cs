using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class MoonJelly : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly");
			Tooltip.SetDefault("Gives high regeneration.");
		}


        public override void SetDefaults()
        {
            item.width = 20; 
            item.height = 30;
            item.rare = 5;
            item.maxStack = 30;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;

            item.buffType = mod.BuffType("MoonBlessing");
            item.buffTime = 1000;

            item.UseSound = SoundID.Item3;
        }
		
		public override bool CanUseItem(Player player)
		{
			if (player.FindBuffIndex(BuffID.PotionSickness)>=0)
			{
				return false;
			}
			return true;
			
		}
      public override bool UseItem(Player player)
        {
			player.AddBuff(BuffID.PotionSickness, 3600);
			
            
            return true;
        }
    }
}
