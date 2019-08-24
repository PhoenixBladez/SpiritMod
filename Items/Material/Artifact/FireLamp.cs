using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class FireLamp : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Lamp");
			Tooltip.SetDefault("'It flickers dangerously and mystically'");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.value = 500;
            item.rare = 1;
            item.maxStack = 1;
        }
    }
}