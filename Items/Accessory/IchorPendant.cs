using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class IchorPendant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Pendant");
			Tooltip.SetDefault("Increased melee damage by 6%\nWeapons have a 10% chance to inflict Ichor");
		}


		public override void SetDefaults()
		{
            item.width = 18;
            item.height = 18;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = 4;

			item.accessory = true;

			item.defense = 0;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MyPlayer>(mod).IchorPendant = true;
			player.meleeDamage *= 1.06f;
		}
	}
}
