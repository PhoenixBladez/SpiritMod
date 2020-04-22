using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class Tumblesoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tumblesoul");
			Tooltip.SetDefault("Increases movement speed by 10% while on sand");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 5));
        }


		public override void SetDefaults()
		{
			item.width = 34;
            item.height = 36;
			item.value = Item.sellPrice(0, 0, 30, 0);
            item.rare = 1;
            item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetSpiritPlayer().tumbleSoul = true;
        }
    }
}
