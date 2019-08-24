using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class SkullStickItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skull Stick");
			Tooltip.SetDefault("'Must've belonged to an ancient tribe'");
		}


		public override void SetDefaults()
		{
            item.width = 94;
			item.height = 62;
            item.rare = 1;

            item.maxStack = 99;

			item.useTurn = true;
			item.autoReuse = true;
            item.consumable = true;

            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;


            item.createTile = mod.TileType("SkullStick");
		}
	}
}