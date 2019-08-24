using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
    public class HellEater : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fiery Maw");
            Tooltip.SetDefault("Ranged attacks may shoot out fiery spit that explode upon hitting enemies\nIncreases ranged damage by 4%");

        }


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = 3;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).fireMaw = true;
            player.rangedDamage += 0.03f;
        }
    }
}
