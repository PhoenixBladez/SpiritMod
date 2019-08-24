using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class ScarabCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Charm");
			Tooltip.SetDefault("Increases minion damage by 8% and max number of minions by 1");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = 2;

			item.accessory = true;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.minionDamage += 0.08f;
            player.maxMinions += 1;
		}
	}
}
