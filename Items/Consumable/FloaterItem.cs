using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace SpiritMod.Items.Consumable
{
    public class FloaterItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Floater");
			Tooltip.SetDefault("'A beacon at the beach'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 20;
            item.rare = 2;
            item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 3, 0);
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = true;

        }

        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, mod.NPCType("Floater1"));
            return true;
        }

    }
}
