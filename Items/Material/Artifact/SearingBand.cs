using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class SearingBand : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Band");
			Tooltip.SetDefault("'Emblazoned with fiery runes'");
        }


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
            item.value = 500;
            item.rare = 6;
            item.maxStack = 1;
        }
    }
}