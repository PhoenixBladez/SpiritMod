using Terraria.ID;

namespace SpiritMod.Items.Halloween
{
	public class GoldCandy : CandyBase
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Candy");
			Tooltip.SetDefault("Can't be eaten, but may sell for a lot!");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 30;
			Item.value = 50000;
		}
	}
}
