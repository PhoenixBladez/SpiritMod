using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarDrops
{
	public class ReachFishingCatch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxophyll");
			Tooltip.SetDefault("'It reeks of poison'");
		}


		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;

			item.maxStack = 999;
		}
	}
}
