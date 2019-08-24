using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class GraniteChunk : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Granite Chunk");
			Tooltip.SetDefault("'Pieces of granite infused with ore'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.value = 5000;

            item.maxStack = 999;
            item.rare = 2;
            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;


            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("GraniteOre");
        }
    }
}