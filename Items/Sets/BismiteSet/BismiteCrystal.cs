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
			Item.width = 24;
			Item.height = 28;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;

			Item.maxStack = 999;
		}
	}
}
