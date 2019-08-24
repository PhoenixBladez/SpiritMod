using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class TideStone : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tide Stone");
			Tooltip.SetDefault("'Unearthed from the Murky Depths'");
        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.value = 500;
            item.rare = 3;
            item.maxStack = 1;
        }
    }
}