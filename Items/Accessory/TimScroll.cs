using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class TimScroll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tim's Scroll of Fumbling");
			Tooltip.SetDefault("'Who knew Skeletons could write?'\nMagic attacks may inflict random debuffs on foes\nMagic attacks may shoot out a random projectile");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
			item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = 3;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetModPlayer<MyPlayer>(mod).timScroll = true;
        }

	}
}
