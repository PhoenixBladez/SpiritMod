using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class FossilFlower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fossil Flower");
			Tooltip.SetDefault("Increases melee damage and melee speed by 12% when underground");
		}


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.defense = 2;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            if (player.ZoneRockLayerHeight)
            {
                player.meleeDamage += .12f;
                player.meleeSpeed += .12f;
            }
        }

	}
}
