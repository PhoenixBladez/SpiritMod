using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class Carapace : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardened Carapace");
			Tooltip.SetDefault("'Fossilized shells'");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 100;
            item.rare = 1;

            item.maxStack = 999;
        }
    }
}