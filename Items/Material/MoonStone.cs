using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
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
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 3));
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 1000;
            item.rare = 4;
            item.scale = .8f;
            item.maxStack = 999;            
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color (200, 200, 200, 100);
        }
    }
}