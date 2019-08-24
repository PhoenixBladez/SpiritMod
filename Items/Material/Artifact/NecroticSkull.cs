using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class NecroticSkull : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necrotic Skull");
			Tooltip.SetDefault("'Remains of an ancient adventurer'");
        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 500;
            item.rare = 3;
            item.maxStack = 1;
        }
    }
}