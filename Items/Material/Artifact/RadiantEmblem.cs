using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class RadiantEmblem : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiant Emblem");
			Tooltip.SetDefault("'It heralds powerful magic'");
        }


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.value = 500;
            item.rare = 7;
            item.maxStack = 1;
        }
    }
}