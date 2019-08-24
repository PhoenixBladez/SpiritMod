using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class FloranOre : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Ore");
			Tooltip.SetDefault("'From another star's Earth'");
		}


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;

            item.maxStack = 999;

            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;
            item.rare = 2;
            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("FloranOreTile");
        }
    }
}
