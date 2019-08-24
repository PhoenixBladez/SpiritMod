using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class UnrefinedRuneStone : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unrefined Rune Stone");
			Tooltip.SetDefault("'The runes seem to splinter into infinite colors'");
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