using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class BloodWard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Ward");
			Tooltip.SetDefault("Enemies around you are inflicted with Blood Corruption");
		}


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
			item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 2;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetModPlayer<MyPlayer>(mod).Ward = true;
        }

	}
}
