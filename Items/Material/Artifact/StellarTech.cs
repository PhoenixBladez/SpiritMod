using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class StellarTech : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Apparatus");
			Tooltip.SetDefault("'An ingenious invention with no visible use'");
        }


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.value = 500;
            item.rare = 3;
            item.maxStack = 1;
        }
    }
}