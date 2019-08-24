using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class AssassinMagazine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assassin's Magazine");
			Tooltip.SetDefault("Increases ranged damage by 2%\nIncreases arrow damage by 4% when moving\nIncreases bullet damage by 4% when standing still");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
            item.defense = 1;
			item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.02f;

            if (player.velocity.X != 0)
            {
                player.arrowDamage += 0.04f;
            }
            else if (player.velocity.Y != 0)
            {
                player.arrowDamage += 0.04f;
            }
            else
            {
                player.bulletDamage += 0.04f;
            }
        }
    }
}
