using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class WornSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worn Adventurer's Sword");
			Tooltip.SetDefault("'It's worn, but could have a use'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.maxStack = 999;
			item.value = 500;
			item.value = Terraria.Item.buyPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Green;
		}
	}
}
