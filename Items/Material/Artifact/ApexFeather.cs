using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class ApexFeather : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apex Feather");
			Tooltip.SetDefault("'It belongs to the top predator'");
        }


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 28;
            item.value = 500;
            item.rare = 8;
            item.maxStack = 1;
        }
    }
}