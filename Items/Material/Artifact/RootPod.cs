using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class RootPod : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aged Root Pod");
			Tooltip.SetDefault("'Ripe for the picking'");
        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 100;
            item.rare = 1;
            item.maxStack = 1;
        }
    }
}