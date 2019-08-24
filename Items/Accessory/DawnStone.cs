using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class DawnStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dawn Stone");
			Tooltip.SetDefault("Increases melee damage by 8%\nIncreases melee critical strike chance the less health you have\nMelee attacks may inflict 'Solar Burn,' which slightly reduces enemy defense");
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
            player.meleeDamage += 0.08f;
            float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 7f;
            player.meleeCrit += (int)defBoost;

            player.GetModPlayer<MyPlayer>(mod).sunStone = true;
        }

	}
}
