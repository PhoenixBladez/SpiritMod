using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material
{
	public class StarPiece : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Piece");
			Tooltip.SetDefault("'A Cosmic Shard'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 100;
            item.rare = 5;

            item.maxStack = 999;
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}