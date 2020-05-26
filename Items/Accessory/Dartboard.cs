using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class Dartboard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dartboard");
			Tooltip.SetDefault("Decreases damage dealt by 13%\nIncreases critical strike chance by 15%\n'Right on the mark'");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
			item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage -=  .13f;
            player.magicDamage -= .13f;
            player.rangedDamage -= .13f;
            player.minionDamage -= .13f;
            player.meleeCrit += 15;
            player.magicCrit += 15;
            player.rangedCrit += 15;

        }
    }
}
