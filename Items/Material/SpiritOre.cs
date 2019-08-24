using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class SpiritOre : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Ore");
			Tooltip.SetDefault("'Spirit-infused metal'");
		}


        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 12;

            item.maxStack = 999;
            item.rare = 5;
            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("SpiritOreTile");
        }
    }
}
