using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class Bauble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winter's Bauble");
			Tooltip.SetDefault("When under half health, damage taken is reduced by 10% and movement speed is increased by 5%\n When under half health, you are also surrounded by a shield that nullifies projectiles for 6 seconds\n Two minute cooldown");
		}


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
			item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 5;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).Bauble = true;
            if (player.statLife <= player.statLifeMax2 / 2)
            {
                player.endurance += .10f;
				player.moveSpeed += 0.05f;
            }
        }

	}
}
