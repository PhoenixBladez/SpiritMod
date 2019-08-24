using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class Talon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon");
			Tooltip.SetDefault("'Ouch! It's sharp! \n Used to craft Talon gear'");
		}


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 24;
            item.value = 100;
            item.rare = 2;

            item.maxStack = 999;
        }
    }
}
