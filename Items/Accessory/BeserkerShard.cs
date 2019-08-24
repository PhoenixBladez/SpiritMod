using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
    public class BeserkerShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shard of the Berserker");
            Tooltip.SetDefault("Increases armor penetration by 5");

        }


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 1;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 5;
        }
    }
}
