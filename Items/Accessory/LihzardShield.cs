using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class LihzardShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Shield");
			Tooltip.SetDefault("Greatly reduces damage taken when standing still.");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.value = Item.buyPrice(0, 14, 0, 0);
            item.rare = 7;

            item.accessory = true;

            item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Math.Abs(player.velocity.Y) < 0.05 && Math.Abs(player.velocity.Y) < 0.05)
            {
                player.endurance += .30f;
            }
        }
    }
}
