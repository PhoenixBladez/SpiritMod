using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	public class OlympiumToken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympium Token");
			Tooltip.SetDefault("May be of interest to a collector...");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = 0;
			item.rare = ItemRarityID.LightRed;

			item.maxStack = 999;
		}
	}
}
