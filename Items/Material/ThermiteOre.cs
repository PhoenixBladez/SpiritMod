using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class ThermiteOre : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermite Ore");
			Tooltip.SetDefault("'It has an extremely molten exterior'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;

            item.maxStack = 999;
            item.rare = 8;
            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("ThermiteOre");
        }
    }
}
