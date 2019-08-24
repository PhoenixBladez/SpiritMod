using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class SolarEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Emblem");
			Tooltip.SetDefault("Increases melee critical chance by 5, melee speed by 15%, and melee damage by 16%");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 8;

			item.accessory = true;

			item.defense = 4;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 5;
			player.meleeSpeed += 0.15f;
			player.meleeDamage += 0.16f;
		}
	}
}
