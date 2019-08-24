using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class SteamParts : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Fragments");
			Tooltip.SetDefault("'Powered by celestial energy'");
		}


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 24;
            item.value = 800;
            item.rare = 3;

            item.maxStack = 999;
        }
    }
}
