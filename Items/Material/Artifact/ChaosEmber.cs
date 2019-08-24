using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class ChaosEmber : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Ember");
			Tooltip.SetDefault("'It burns malevolently'");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.value = 500;
            item.rare = 2;
            item.maxStack = 1;
        }
    }
}