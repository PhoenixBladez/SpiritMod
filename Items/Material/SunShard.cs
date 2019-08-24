using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material
{
	public class SunShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Shard");
			Tooltip.SetDefault("'A fragment of Solar Deities'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.value = 100;
            item.rare = 8;

            item.maxStack = 999;
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}