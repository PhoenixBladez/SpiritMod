using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Crystal");
			Tooltip.SetDefault("'An oddly toxic metal'");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = 100;
			item.rare = ItemRarityID.Blue;

			item.maxStack = 999;
		}
	}
}
