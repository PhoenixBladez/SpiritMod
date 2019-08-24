using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class SoulBloom : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soulbloom");
        }


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 22;
            item.value = 100;
            item.rare = 5;

            item.maxStack = 999;
        }
    }
} 
