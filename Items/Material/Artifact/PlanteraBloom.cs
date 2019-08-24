using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class PlanteraBloom : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plantera's Bloom");
			Tooltip.SetDefault("'Beautiful, yet foul smelling'");
        }


        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 34;
            item.value = 500;
            item.rare = 7;
            item.maxStack = 1;
        }
    }
}