using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class DungeonSpirit : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Spirit");
			Tooltip.SetDefault("'It has had millenia to mature'\nRenders you drunk, lowering defense by 10 and increasing melee damage and melee speed by 12%\nIncreases melee critical strike chance by 3%");
		}


        public override void SetDefaults()
        {
            item.width = 20; 
            item.height = 30;
            item.rare = 8;
            item.maxStack = 30;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;

            item.buffType = mod.BuffType("Crunk");
            item.buffTime = 36000;

            item.UseSound = SoundID.Item3;
        }
    }
}
