using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class Cogflower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cogflower");
			Tooltip.SetDefault("Increases maximum mana by 30\nIncreases magic critical strike chance by 5%");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 3;

			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.statManaMax2 += 30;
			player.magicCrit += 5;
		}
	}
}
