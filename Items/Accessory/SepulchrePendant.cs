using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
    public class SepulchrePendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necrotic Pendant");
            Tooltip.SetDefault("Getting hit occasionbally lengthens immunity frames");

        }


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 1;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).sepulchreCharm = true;
        }
    }
}
