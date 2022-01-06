using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
namespace SpiritMod.Items.Sets.PirateStuff
{
	class PirateKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Secret Key");
			Tooltip.SetDefault("'The pirates must have hidden treasure somewhere...'");
		}
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 20;
			item.maxStack = 99;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 1, 0, 0);

		}
	}
}
