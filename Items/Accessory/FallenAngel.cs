using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class FallenAngel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Angelic Sigil");
			Tooltip.SetDefault("Magic attacks may also shoot out an angelic spark\nThis spark deals more damage the less mana the player has left");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
            item.defense = 1;
			item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{

            player.GetModPlayer<MyPlayer>(mod).manaWings = true;
        }

	}
}
