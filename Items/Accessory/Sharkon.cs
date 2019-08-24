using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class Sharkon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharkon Fin");
			Tooltip.SetDefault("Increases ranged damage and movement speed by 10% when underwater\nIncreases defense by 2 when out of the water");
		}


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            if (player.wet)
            {
                player.rangedDamage += .10f;
                player.moveSpeed += .10f;
            }
            else
            {
                player.statDefense += 2;
            }
        }

	}
}
