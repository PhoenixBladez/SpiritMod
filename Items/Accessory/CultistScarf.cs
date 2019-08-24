using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class CultistScarf : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderweave Scarf");
            Tooltip.SetDefault("Reduces mana usage by 20% when under half health\nIncreases maximum mana by 120 when above half health\nIncreases magic critical strike chance by 9%\nMagic attacks occasionally release bolts of powerful Ancient Magic that bounce off of walls");

        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 10;
            item.value = Item.buyPrice(0, 90, 0, 0);

            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicCrit += 9;
            player.GetModPlayer<MyPlayer>(mod).cultistScarf = true;
            if (player.statLife < player.statLifeMax2 / 2)
            {
                player.manaCost -= 0.20f;
            }
            else if (player.statLife > player.statLifeMax2 / 2)
            {
                player.statManaMax2 += 120;
            }
        }
    }
}
