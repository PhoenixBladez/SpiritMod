using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class CryoliteOre : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Ore");
			Tooltip.SetDefault("'Veins of metal intertwined with ice'");
		}


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;

            item.maxStack = 999;

            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;
            item.rare = 3;
            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("CryoliteOreTile");
        }
    }
}
