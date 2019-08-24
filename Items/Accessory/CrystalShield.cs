using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class CrystalShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Shield");
			Tooltip.SetDefault("Walking leaves an aura of damaging crystals");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 7;
			item.expert = true;
            item.defense = 4;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).CrystalShield = true;
        }
    }
}
