using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class KingRock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King’s Stone");
			Tooltip.SetDefault("Magic attacks may cause Prismatic Bolts to rain from the sky");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = 9;

			item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<MyPlayer>(mod).KingRock = true;
		}
	}
}
