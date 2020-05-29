using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
    public class RockCandy : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rock Candy");
			Tooltip.SetDefault("'Sweet and incredibly tough!'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 22;
            item.rare = 5;
            item.maxStack = 99;
            item.noUseGraphic = true;
			item.useStyle = 2;
            item.useTime = item.useAnimation = 30;
			
			item.buffType = BuffID.Endurance;
			item.buffTime = 9600;
            item.noMelee = true;
            item.consumable = true;
			item.UseSound = SoundID.Item2;
            item.autoReuse = false;

        }
    }
}
