using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;


namespace SpiritMod.Items.Material.Artifact
{
    public class BatteryCore : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Battery Core");
			Tooltip.SetDefault("'The equivalent of a million AA batteries'");
        }


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = 500;
            item.rare = 6;
            item.maxStack = 1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}