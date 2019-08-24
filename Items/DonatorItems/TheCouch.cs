using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class TheCouch : ModItem
    {
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Couch");
			Tooltip.SetDefault("Defense is increased, but movement speed reduced, when nearby\n'Hello Terraria Enthusiasts'\n ~Donator Item~");
		}


        public override void SetDefaults()
		{
            item.width = 52;
			item.height = 30;
            item.value = 50000;

            item.maxStack = 99;
			item.rare = 4;
            item.useStyle = 1;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createTile = mod.TileType("TheCouch");
		}

    }
}