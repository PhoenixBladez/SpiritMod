using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Back)]
    public class DesertSlab : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Slab");
			Tooltip.SetDefault("Provides immunity to the 'Mighty Wind' debuff during Sandstorms");
		}


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
			item.defense = 1;
            item.accessory = true;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
        }
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ZoneDesert)
            {
                player.buffImmune[BuffID.WindPushed] = true;
            }
		}
    }
}
