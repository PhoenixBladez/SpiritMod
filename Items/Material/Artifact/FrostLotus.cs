using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class FrostLotus : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Lotus");
			Tooltip.SetDefault("'A fragile beauty'");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.value = 500;
            item.rare = 1;
            item.maxStack = 1;
        }
    }
}