using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class ShamanBand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shaman's Bracelet");
			Tooltip.SetDefault("Magic attacks may burn hit enemies");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
			item.value = Item.buyPrice(0, 0, 11, 0);
            item.rare = 5;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetModPlayer<MyPlayer>(mod).shamanBand = true;
        }

	}
}
