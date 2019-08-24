using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class DemonLens : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demonic Retina");
			Tooltip.SetDefault("'Ancient and all seeing'");
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