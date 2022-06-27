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
			Item.width = 14;
			Item.height = 20;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 1, 0, 0);

		}
	}
}
