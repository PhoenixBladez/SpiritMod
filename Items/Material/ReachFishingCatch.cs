using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class ReachFishingCatch : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxophyll");
			Tooltip.SetDefault("'It reeks of poison'");
		}


        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 1000;
            item.rare = 1;

            item.maxStack = 999;
        }
    }
}
