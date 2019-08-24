using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class CursedMedallion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Medallion");
			Tooltip.SetDefault("'Untold riches can be gained, at a cost...'");
        }


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 40;
            item.value = 500;
            item.rare = 6;
            item.maxStack = 1;
        }
    }
}