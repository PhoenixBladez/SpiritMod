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
			Item.width = 26;
			Item.height = 28;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;

			Item.maxStack = 999;
		}
	}
}
