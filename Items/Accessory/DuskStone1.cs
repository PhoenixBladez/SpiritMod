using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class DuskStone1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Stone");
			Tooltip.SetDefault("Increases ranged damage by 8%\nIncreases ranged critical strike chance the less health you have\nRanged attacks may inflict 'Moon Burn'");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
			item.value = Item.buyPrice(0, 8, 0, 0);
            item.rare = 3;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.rangedDamage += 0.08f;
            float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 7f;
            player.rangedCrit += (int)defBoost;

            player.GetModPlayer<MyPlayer>(mod).moonStone = true;
        }

	}
}
