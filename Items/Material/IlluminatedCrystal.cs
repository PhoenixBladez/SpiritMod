using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class IlluminatedCrystal : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminated Crystal");
			Tooltip.SetDefault("'The crystal is humming with arcane energy'");
		}


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.value = 100;
            item.rare = 7;

            item.maxStack = 999;
        }
    }
}