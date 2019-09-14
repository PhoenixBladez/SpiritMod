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
			DisplayName.SetDefault("Pendant of the Warm Winds");
			Tooltip.SetDefault("Press and hold 'up' to reduce falling speed\nProvides immunity to the 'Chilled' debuff");
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
			player.GetSpiritPlayer().scarabCharm = true;
			player.buffImmune[BuffID.Chilled] = true;
		}
	}
}
