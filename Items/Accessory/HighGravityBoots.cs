using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shoes)]
    public class HighGravityBoots : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("High Gravity Boots");
			Tooltip.SetDefault("Increases your gravity");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 20;
            item.value = Item.buyPrice(0, 0, 4, 0);
            item.rare = 1;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			if (Main.rand.Next(5) == 1)
			{
				player.velocity.Y+=0.07f;
				Dust.NewDustPerfect(new Vector2(player.position.X + Main.rand.Next(player.width), player.position.Y + player.height - Main.rand.Next(7)), 206, Vector2.Zero);
			}
        }
    }
}
