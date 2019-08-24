using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
    public class FieryTrident : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infernal Flame");
            Tooltip.SetDefault("Melee critical strikes may cause enemies to explode\nIncreases melee damage by 5%");
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 1, 40, 0);
            item.rare = 5;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>(mod).infernalFlame = true;
            player.meleeDamage += .05f;
        }
    }
}
