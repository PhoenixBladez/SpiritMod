using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class CarvedRock : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernal Rock");
			Tooltip.SetDefault("'A rock laced with veins of magma'");
		}


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 14;
            item.value = 800;
            item.rare = 3;

            item.maxStack = 999;
        }
    }
}
