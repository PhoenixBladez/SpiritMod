using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class PMicrobe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plaguebearer Microbe");
			Tooltip.SetDefault("Increases max life by 10 at the cost of 1 defense");
		}


		public override void SetDefaults()
		{
            item.width = 28;
			item.height = 24;
            item.value = Item.buyPrice(0, 0, 75, 0);
			item.rare = 1;

			item.accessory = true;
        }

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.statLifeMax2 += 10;
            player.statDefense -= 1;
        }
	}
}
