using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class MarbleChunk : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Marble Chunk");
			Tooltip.SetDefault("'Contains fragments of past civilizations'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.maxStack = 999;
            item.rare = 2;
            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;

            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("MarbleOre");
        }
    }
}