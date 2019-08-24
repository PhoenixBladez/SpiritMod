using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class VortexEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Emblem");
			Tooltip.SetDefault("Increases ranged damage by 25% and ranged critical strike chance by 10%");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 8;

			item.accessory = true;

			item.defense = 2;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.rangedDamage += 0.25f;
			player.rangedCrit += 10;
		}
	}
}
