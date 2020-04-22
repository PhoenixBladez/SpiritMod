using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class ReachBrooch : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forsworn Pendant");
			Tooltip.SetDefault("Increases critical strike chance by 4%\nAllows for increased night vision in the Briar");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = Item.buyPrice(0, 0, 2, 0);
            item.rare = 2;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
            player.meleeCrit += 4;
            player.GetSpiritPlayer().reachBrooch = true;
        }
    }
}
