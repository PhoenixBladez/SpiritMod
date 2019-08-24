using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
    public class WheezerScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wheezer Scale");
            Tooltip.SetDefault("Melee hits on foes may cause them to emit a cloud of poisonous gas\nIncreases melee critical strike chance by 5%");

        }


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 1, 40, 0);
            item.rare = 2;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).wheezeScale = true;
            player.meleeCrit += 5;
        }
    }
}
