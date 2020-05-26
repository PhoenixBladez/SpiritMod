using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Material
{
    public class MoonStone : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Gem");
			Tooltip.SetDefault("'Holds a far away power'");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 1000;
            item.rare = 4;

            item.maxStack = 999;            
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}