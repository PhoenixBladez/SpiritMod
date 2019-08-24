using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class GildedIdol : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Idol");
			Tooltip.SetDefault("'Honoring the Deities of old...'");
        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 8000;
            item.rare = 1;
            item.maxStack = 1;
        }
    }
}