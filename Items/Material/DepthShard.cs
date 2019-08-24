using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class DepthShard : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Shard");
			Tooltip.SetDefault("'Metal unearthed from the Depths of the ocean'");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 100;
            item.rare = 5;

            item.maxStack = 999;
        }
    }
}