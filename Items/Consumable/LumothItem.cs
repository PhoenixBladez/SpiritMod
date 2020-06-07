using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace SpiritMod.Items.Consumable
{
    public class LumothItem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumoth");
			Tooltip.SetDefault("'Mechanically stunning'");
		}

        public override void SetDefaults()
        {
            item.width = item.height = 32;
            item.rare = 2;
            item.maxStack = 99;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 0, 7, 0);
            item.useTime = item.useAnimation = 20;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = true;

        }
        public override bool UseItem(Player player)
        {
            NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Lumoth>());
            return true;
        }

        }
}
