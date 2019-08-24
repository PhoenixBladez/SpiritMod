using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class AncientBark : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elderbark");
			Tooltip.SetDefault("'Thousands of years old'");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 16;
            item.maxStack = 999;
            item.value = 800;
            item.rare = 1;
            item.useStyle = 1;
			item.useTime = 7;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;
			
						item.createTile = mod.TileType("BarkTileTile");
        }
    }
}
